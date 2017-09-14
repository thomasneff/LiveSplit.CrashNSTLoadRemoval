using LiveSplit.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
namespace LiveSplit.UI.Components
{
    class CrashNSTLoadRemovalComponent : IComponent
    {
        //public ComponentSettings Settings { get; set; }

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

  
        protected SimpleLabel Label1 = new SimpleLabel();
		public CrashNSTLoadRemovalSettings settings { get; set; }

		protected string InfoStringPokemon { get; set; }
        protected string InfoStringEncounter { get; set; }
        public CrashNSTLoadRemovalComponent()
        {
			settings = new CrashNSTLoadRemovalSettings();
            Label1 = new SimpleLabel();
            Cache = new GraphicsCache();

		}

   
        public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
           
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
        }
    }
}
