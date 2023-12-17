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
            else if (setting == "expensiveBase")
            {
                if (SuperheroGenes_Settings.expensiveBase && active != null)
                {
                    return active.Apply(xml);
                }
            }
            else if (setting == "supersEverywhere")
            {
                if (SuperheroGenes_Settings.supersEverywhere && active != null)
                {
                    return active.Apply(xml);
                }
            }
            else if (setting == "activatableSuperGenes")
            {
                if (SuperheroGenes_Settings.activatableSuperGenes && active != null)
                {
                    return active.Apply(xml);
                }
            }
            else if (setting == "interruptibleActivatables")
            {
                if (SuperheroGenes_Settings.activatableSuperGenes && SuperheroGenes_Settings.interruptibleActivatables && active != null)
                {
                    return active.Apply(xml);
                }
            }
            else if (setting == "middleGrounds")
            {
                if (SuperheroGenes_Settings.middleGrounds && active != null)
                {
                    return active.Apply(xml);
                }
            }
            else if (setting == "allGrounds")
            {
                if (SuperheroGenes_Settings.middleGrounds && SuperheroGenes_Settings.allGrounds && active != null)
                {
                    return active.Apply(xml);
                }
            }

            // AI stuff

            else if (setting == "poolUsage")
            {
                if (SuperheroGenes_Settings.poolUsage && active != null)
                {
                    return active.Apply(xml);
                }
            }

            else if (setting == "automaticHealer")
            {
                if (SuperheroGenes_Settings.automaticHealer && active != null)
                {
                    return active.Apply(xml);
                }
            }

            else if (setting == "medievalVillains")
            {
                if (SuperheroGenes_Settings.medievalVillains && active != null)
                {
                    return active.Apply(xml);
                }
            }
            else if (setting == "vengefulOne")
            {
                if (!SuperheroGenes_Settings.vengefulOne && inactive != null)
                {
                    return inactive.Apply(xml);
                }
            }
            else if (setting != null) Log.Error("A patch is using a setting that is either mispelled or unhandled");
            else Log.Error("A patch is using this mod's settings, but doesn't specify which one.");
            return true;
        }
    }
}
