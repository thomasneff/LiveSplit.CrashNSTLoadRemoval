using LiveSplit.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using CrashNSaneLoadDetector;

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
		private int numberOfPauses = 0;
		private string GameName = "";
		private string GameCategory = "";

		public CrashNSTLoadRemovalComponent(LiveSplitState state)
		{
			GameName = state.Run.GameName;
			GameCategory = state.Run.CategoryName;
			settings = new CrashNSTLoadRemovalSettings(state);

			timer = new TimerModel { CurrentState = state };
			timer.CurrentState.OnStart += timer_OnStart;
			timer.CurrentState.OnReset += timer_OnReset;
			timer.CurrentState.OnSkipSplit += timer_OnSkipSplit;
		}

		private void timer_OnSkipSplit(object sender, EventArgs e)
		{
			runningFrames = 0;
			pausedFrames = 0;
			numberOfPauses = 0;
		}

		private void timer_OnReset(object sender, TimerPhase value)
		{
			timerStarted = false;
		}

		void timer_OnStart(object sender, EventArgs e)
		{
			timer.InitializeGameTime();
			timerStarted = true;
		}


		public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {

			if(GameName != state.Run.GameName || GameCategory != state.Run.CategoryName)
			{
				//Reload settings for different game or category
				GameName = state.Run.GameName;
				GameCategory = state.Run.CategoryName;

				settings.ChangeAutoSplitSettingsToGameName(GameName, GameCategory);
			}


			if (timerStarted)
			{
				//Capture image using the settings defined for the component
				Bitmap capture = settings.CaptureImage();

				//Feed the image to the feature detection
				var features = FeatureDetector.featuresFromBitmap(capture);
				int tempMatchingBins = 0;
				isLoading = FeatureDetector.compareFeatureVector(features.ToArray(), out tempMatchingBins, false);
				matchingBins = tempMatchingBins;

				timer.CurrentState.IsGameTimePaused = isLoading;

				if(settings.AutoSplitterEnabled)
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
						numberOfPauses++;

						if (numberOfPauses >= settings.GetAutoSplitNumberOfLoadsForSplit(state.CurrentSplit.Name))
						{
							timer.Split();
							numberOfPauses = 0;
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

			}


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
