using LiveSplit.Model;
using LiveSplit.PokemonRedBlue;
using LiveSplit.UI.Components;
using System;

[assembly: ComponentFactory(typeof(CrashNSTLoadRemovalFactory))]

namespace LiveSplit.PokemonRedBlue
{
    public class CrashNSTLoadRemovalFactory : IComponentFactory
    {
        public string ComponentName
        {
            get { return "Crash NST Load Removal"; }
        }

        public ComponentCategory Category
        {
            get { return ComponentCategory.Control; }
        }

        public string Description
        {
            get { return "Automatically detects and removes loads (GameTime) for the Crash N Sane Trilogy."; }
        }

        public IComponent Create(LiveSplitState state)
        {
            return new CrashNSTLoadRemovalComponent(state);
        }

        public string UpdateName
        {
            get { return ComponentName; }
        }
		public string UpdateURL => "https://raw.githubusercontent.com/thomasneff/LiveSplit.CrashNSTLoadRemoval/master/";
		public string XMLURL => UpdateURL + "update.LiveSplit.CrashNSTLoadRemoval.xml";
		

        public Version Version
        {
            get { return Version.Parse("2.3"); }
        }
    }
}
