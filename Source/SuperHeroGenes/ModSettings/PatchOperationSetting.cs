using System.Collections.Generic;
using System.Xml;
using Verse;

namespace SuperHeroGenesBase
{
    public class PatchOperationSetting : PatchOperation
    {
        private PatchOperation active;

        private PatchOperation inactive;

        public string setting;

        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (setting == "condensedMeteors")
            {
                if (!SuperheroGenes_Settings.condensedMeteors && inactive != null)
                {
                    return inactive.Apply(xml);
                }
            }
            else if (setting != null) Log.Error("A patch is using a setting that is either misspelled, or unhandled");
            else Log.Error("A patch is using this mod's settings, but doesn't specify which one.");
            return true;
        }
    }
}
