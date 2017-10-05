using LiveSplit.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using CrashNSaneLoadDetector;
using System.Threading;

namespace LiveSplit.UI.Components
{
    class CrashNSTLoadRemovalComponent : IComponent
    {
        public string ComponentName
        {
            get { return "Crash NST Load Removal"; }
        }
        public GraphicsCache Cache { get; set; }


        public float PaddingBottom { get { return 0; } }
        public float PaddingTop { get { return 0; } }
        public float PaddingLeft { get { return 0; } }
        public float PaddingRight { get { return 0; } }

        public bool Refresh { get; set; }

        public IDictionary<string, Action> ContextMenuControls { get; protected set; }
		
		public CrashNSTLoadRemovalSettings settings { get; set; }

		private bool isLoading = false;
		private int matchingBins = 0;

		private TimerModel timer;
		private bool timerStarted = false;

		public enum CrashNSTState
		{
			RUNNING,
			LOADING
		}

		private CrashNSTState NSTState = CrashNSTState.RUNNING;
		private int runningFrames = 0;
		private int pausedFrames = 0;
		private int pausedFramesSegment = 0;
		private string GameName = "";
		private string GameCategory = "";
		private int NumberOfSplits = 0;
		private List<string> SplitNames;
		private DateTime lastTime;
		private DateTime segmentTimeStart;
		private LiveSplitState liveSplitState;
		private Thread captureThread;
		private bool threadRunning = false;
		private double framesSum = 0.0;
		private int framesSumRounded = 0;
		private int framesSinceLastManualSplit = 0;
		private bool LastSplitSkip = false;
		
		private HighResolutionTimer.HighResolutionTimer highResTimer;
		private List<int> NumberOfLoadsPerSplit;

		public CrashNSTLoadRemovalComponent(LiveSplitState state)
		{
			
			GameName = state.Run.GameName;
			GameCategory = state.Run.CategoryName;
			NumberOfSplits = state.Run.Count;
			SplitNames = new List<string>();

			foreach(var split in state.Run)
			{
				SplitNames.Add(split.Name);
			}

			liveSplitState = state;
			NumberOfLoadsPerSplit = new List<int>();
			InitNumberOfLoadsFromState();
			
			settings = new CrashNSTLoadRemovalSettings(state);
			lastTime = DateTime.Now;
			segmentTimeStart = DateTime.Now;
			timer = new TimerModel { CurrentState = state };
			timer.CurrentState.OnStart += timer_OnStart;
			timer.CurrentState.OnReset += timer_OnReset;
			timer.CurrentState.OnSkipSplit += timer_OnSkipSplit;
			timer.CurrentState.OnSplit += timer_OnSplit;
			timer.CurrentState.OnUndoSplit += timer_OnUndoSplit;
			timer.CurrentState.OnPause += timer_OnPause;
			timer.CurrentState.OnResume += timer_OnResume;
			highResTimer = new HighResolutionTimer.HighResolutionTimer(5.0f);
			highResTimer.Elapsed += (s, e) => { CaptureLoads(); };
		}

		private void timer_OnResume(object sender, EventArgs e)
		{
			timerStarted = true;
		}

		private void timer_OnPause(object sender, EventArgs e)
		{
			timerStarted = false;
		}

		private void InitNumberOfLoadsFromState()
		{
			NumberOfLoadsPerSplit = new List<int>();
			NumberOfLoadsPerSplit.Clear();

			if(liveSplitState == null)
			{
				return;
			}

			foreach (var split in liveSplitState.Run)
			{
				NumberOfLoadsPerSplit.Add(0);
			}

			//Quicker way to prevent OOB on last split as I'm not sure if the index will go over if the run finishes
			NumberOfLoadsPerSplit.Add(99999);
		}

		private int CumulativeNumberOfLoadsForSplitIndex(int splitIndex)
		{
			int numberOfLoads = 0;

			for(int idx = 0; (idx < NumberOfLoadsPerSplit.Count && idx <= splitIndex) ; idx++)
			{
				numberOfLoads += NumberOfLoadsPerSplit[idx];
			}
			return numberOfLoads;
		}

		private void CaptureLoads()
		{
			if (timerStarted)
			{
				framesSinceLastManualSplit++;
				//Console.WriteLine("TIME NOW: {0}", DateTime.Now - lastTime);
				//Console.WriteLine("TIME DIFF START: {0}", DateTime.Now - lastTime);
				lastTime = DateTime.Now;
				//Capture image using the settings defined for the component
				Bitmap capture = settings.CaptureImage();

				//Feed the image to the feature detection
				var features = FeatureDetector.featuresFromBitmap(capture);
				int tempMatchingBins = 0;
				bool wasLoading = isLoading;
				isLoading = FeatureDetector.compareFeatureVector(features.ToArray(), FeatureDetector.listOfFeatureVectorsEng, out tempMatchingBins, false);
				matchingBins = tempMatchingBins;

				timer.CurrentState.IsGameTimePaused = isLoading;

				if (isLoading && !wasLoading)
				{
					segmentTimeStart = DateTime.Now;
				}

				if (isLoading)
				{
					pausedFramesSegment++; 
				}

				if (wasLoading && !isLoading)
				{
					TimeSpan delta = (DateTime.Now - segmentTimeStart);
					framesSum += delta.TotalSeconds * 60.0f;
					int framesRounded = Convert.ToInt32(delta.TotalSeconds * 60.0f);
					framesSumRounded += framesRounded;
					Console.WriteLine("SEGMENT FRAMES: {0}, fromTime (@60fps) {1}, timeDelta {2}, totalFrames {3}, fromTime(int) {4}, totalFrames(int) {5}",
						pausedFramesSegment, delta.TotalSeconds,
						delta.TotalSeconds * 60.0f, framesSum, framesRounded, framesSumRounded);
					pausedFramesSegment = 0;
				}


				if (settings.AutoSplitterEnabled && !(settings.AutoSplitterDisableOnSkipUntilSplit && LastSplitSkip))
				{
					//This is just so that if the detection is not correct by a single frame, it still only splits if a few successive frames are loading
					if (isLoading && NSTState == CrashNSTState.RUNNING)
					{
						pausedFrames++;
						runningFrames = 0;
					}
					else if (!isLoading && NSTState == CrashNSTState.LOADING)
					{
						runningFrames++;
						pausedFrames = 0;
					}

					if (NSTState == CrashNSTState.RUNNING && pausedFrames >= settings.AutoSplitterJitterToleranceFrames)
					{
						runningFrames = 0;
						pausedFrames = 0;
						//We enter pause.
						NSTState = CrashNSTState.LOADING;
						if (framesSinceLastManualSplit >= settings.AutoSplitterManualSplitDelayFrames)
						{
							NumberOfLoadsPerSplit[liveSplitState.CurrentSplitIndex]++;

							if (CumulativeNumberOfLoadsForSplitIndex(liveSplitState.CurrentSplitIndex) >= settings.GetCumulativeNumberOfLoadsForSplit(liveSplitState.CurrentSplit.Name))
							{
							
									timer.Split();
							
							
							}
						}

					}
					else if (NSTState == CrashNSTState.LOADING && runningFrames >= settings.AutoSplitterJitterToleranceFrames)
					{
						runningFrames = 0;
						pausedFrames = 0;
						//We enter runnning.
						NSTState = CrashNSTState.RUNNING;
					}
				}


				//Console.WriteLine("TIME TAKEN FOR DETECTION: {0}", DateTime.Now - lastTime);
			}
		}

		private void timer_OnUndoSplit(object sender, EventArgs e)
		{
			//skippedPauses -= settings.GetAutoSplitNumberOfLoadsForSplit(liveSplitState.Run[liveSplitState.CurrentSplitIndex + 1].Name);
			runningFrames = 0;
			pausedFrames = 0;
			
			//If we undo a split that already has met the required number of loads, we probably want the number to reset.
			if(NumberOfLoadsPerSplit[liveSplitState.CurrentSplitIndex] >= settings.GetAutoSplitNumberOfLoadsForSplit(liveSplitState.CurrentSplit.Name))
			{
				NumberOfLoadsPerSplit[liveSplitState.CurrentSplitIndex] = 0;
			}

			//Otherwise - we're fine. If it is a split that was skipped earlier, we still keep track of how we're standing.
			

		}

		private void timer_OnSplit(object sender, EventArgs e)
		{
			runningFrames = 0;
			pausedFrames = 0;
			framesSinceLastManualSplit = 0;
			//If we split, we add all remaining loads to the last split.
			//This means that the autosplitter now starts at 0 loads on the next split.
			//This is just necessary for manual splits, as automatic splits will always have a difference of 0.
			var loadsRequiredTotal = settings.GetCumulativeNumberOfLoadsForSplit(liveSplitState.Run[liveSplitState.CurrentSplitIndex - 1].Name);
			var loadsCurrentTotal = CumulativeNumberOfLoadsForSplitIndex(liveSplitState.CurrentSplitIndex - 1);
			NumberOfLoadsPerSplit[liveSplitState.CurrentSplitIndex - 1] += loadsRequiredTotal - loadsCurrentTotal;

			LastSplitSkip = false;
		}

		private void timer_OnSkipSplit(object sender, EventArgs e)
		{

			runningFrames = 0;
			pausedFrames = 0;

			//We don't need to do anything here - we just keep track of loads per split now.
			LastSplitSkip = true;

			/*if(settings.AutoSplitterDisableOnSkipUntilSplit)
			{
				NumberOfLoadsPerSplit[liveSplitState.CurrentSplitIndex - 1] = 0;
			}*/
		}

		private void timer_OnReset(object sender, TimerPhase value)
		{
			timerStarted = false;
			runningFrames = 0;
			pausedFrames = 0;
			framesSinceLastManualSplit = 0;
			threadRunning = false;
			LastSplitSkip = false;
			highResTimer.Stop(joinThread:false);
			InitNumberOfLoadsFromState();
		}

		void timer_OnStart(object sender, EventArgs e)
		{
			InitNumberOfLoadsFromState();
			timer.InitializeGameTime();
			runningFrames = 0;
			framesSinceLastManualSplit = 0;
			pausedFrames = 0;
			timerStarted = true;
			threadRunning = true;
			//StartCaptureThread();
			//highResTimer.Start();
		}

		void StartCaptureThread()
		{
			captureThread = new Thread(() =>
			{
				System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
				while (threadRunning)
				{
					//watch.Restart();
					CaptureLoads();
					//TODO: test rounding of framecounts in output, more importantly:
					//TEST FINAL TIME TO SEE IF IT IS ACCURATE WITH THIS,
					//THEN ADD SLEEPS FOR PERFORMANCE
					//THEN ADJUST FOR BETTER PERFORMANCE

					/*Thread.Sleep(Math.Max((int)(captureDelay - watch.Elapsed.TotalMilliseconds - 1), 0));
					while(captureDelay - watch.Elapsed.TotalMilliseconds >= 0)
					{
						;
					}*/
				}
			});
			captureThread.Start();
		}


		private bool SplitsAreDifferent(LiveSplitState newState)
		{
			//If GameName / Category is different
			if (GameName != newState.Run.GameName || GameCategory != newState.Run.CategoryName)
			{
				GameName = newState.Run.GameName;
				GameCategory = newState.Run.CategoryName;
				return true;
			}

			//If number of splits is different
			if(newState.Run.Count != liveSplitState.Run.Count)
			{
				NumberOfSplits = newState.Run.Count;
				return true;
			}

			//Check if any split name is different.
			for(int splitIdx = 0; splitIdx < newState.Run.Count; splitIdx++)
			{
				if (newState.Run[splitIdx].Name != SplitNames[splitIdx])
				{
					
					SplitNames = new List<string>();

					foreach (var split in newState.Run)
					{
						SplitNames.Add(split.Name);
					}

					return true;
				}
					
			}

			return false;
		}
		public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
			if (SplitsAreDifferent(state))
			{
				settings.ChangeAutoSplitSettingsToGameName(GameName, GameCategory);
			}
			liveSplitState = state;
			/*
			liveSplitState = state;
			if (GameName != state.Run.GameName || GameCategory != state.Run.CategoryName)
			{
				//Reload settings for different game or category
				GameName = state.Run.GameName;
				GameCategory = state.Run.CategoryName;

				settings.ChangeAutoSplitSettingsToGameName(GameName, GameCategory);
			}
			*/



				CaptureLoads();


		}

        public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion)
        {        
           
        }

        public void DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion)
        {
           
        }
       
        public float VerticalHeight
        {
            get { return 0; }
        }

        public float MinimumWidth
        {
            get { return 0; }
        }

        public float HorizontalWidth
        {
            get { return 0; }
        }

        public float MinimumHeight
        {
            get { return 0; }
        }

		public System.Xml.XmlNode GetSettings(System.Xml.XmlDocument document)
		{
			return settings.GetSettings(document);
		}

		public System.Windows.Forms.Control GetSettingsControl(UI.LayoutMode mode)
		{
			return settings;
		}

		public void SetSettings(System.Xml.XmlNode settings)
		{
			this.settings.SetSettings(settings);
		}

		public void RenameComparison(string oldName, string newName)
        {
        }

        public void Dispose()
        {
			timer.CurrentState.OnStart -= timer_OnStart;
		}
    }
}
