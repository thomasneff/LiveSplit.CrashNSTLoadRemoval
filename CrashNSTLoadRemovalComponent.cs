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


		public CrashNSTLoadRemovalComponent(LiveSplitState state)
		{
			settings = new CrashNSTLoadRemovalSettings();

			timer = new TimerModel { CurrentState = state };
			timer.CurrentState.OnStart += timer_OnStart;


		}

		void timer_OnStart(object sender, EventArgs e)
		{
			timer.InitializeGameTime();
		}


		public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
			//Capture image using the settings defined for the component
			Bitmap capture = settings.CaptureImage();

			//Feed the image to the feature detection
			var features = FeatureDetector.featuresFromBitmap(capture);
			isLoading = FeatureDetector.compareFeatureVector(features.ToArray(), out matchingBins, false);

			timer.CurrentState.IsGameTimePaused = isLoading;
			//timer.CurrentState.IsGameTimePaused = true;

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
