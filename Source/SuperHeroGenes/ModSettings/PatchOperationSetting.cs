using System.Collections.Generic;
using System.Xml;
using Verse;

namespace SuperHeroGenesBase
{
    public class PatchOperationSetting : PatchOperation
    {
        private PatchOperation active = null;

        private PatchOperation inactive = null;

        private List<PatchOperation> options = new List<PatchOperation>();

        public string setting;

        protected override bool ApplyWorker(XmlDocument xml)
        {
            // This line goes through the settings class to see if it's a setting that exists
            // If it is, it'll grab the value and assign it to value
            // If it isn't, value will be null and we tell the user something went wrong
            var value = typeof(SuperheroGenes_Settings).GetField(setting)?.GetValue(typeof(SuperheroGenes_Settings));

            if (value != null) // This happens if the setting provided exists
            {
                switch (value)
                {
                    // Handling toggles
                    case bool toggle:
                    {
                        if (toggle)
                        {
                            if (active != null)
                                return active.Apply(xml);
                        }
                        else
                        if (inactive != null)
                            return inactive.Apply(xml);

                        break;
                    }
                    // Handling "dropdowns"
                    case int num:
                    {
                        if (options.Count > num && options[num] != null && options[num].GetType() != typeof(PatchOperation))
                            return options[num].Apply(xml);
                        break;
                    }
                }
            }
            else if (setting != null) 
                Log.Error($"A patch is using {setting}, which is either misspelled, nonexistent, or unhandled");
            else 
                Log.Error("A patch is trying to use this mod's settings, but doesn't specify which one.");
            return true;
        }
    }
}
