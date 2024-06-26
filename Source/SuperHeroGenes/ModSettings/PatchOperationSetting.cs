﻿using System.Collections.Generic;
using System.Xml;
using Verse;

namespace SuperHeroGenesBase
{
    public class PatchOperationSetting : PatchOperation
    {
        private PatchOperation active = null;

        private PatchOperation inactive = null;

        public string setting;

        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (setting == "condensedMeteors")
            {
                if (!SuperheroGenes_Settings.condensedMeteors && inactive != null) return inactive.Apply(xml);
            }
            else if (setting == "multipleArchetypes")
            {
                if (SuperheroGenes_Settings.multipleMutations && active != null) return active.Apply(xml);
            }
            else if (setting == "multipleMutations")
            {
                if (SuperheroGenes_Settings.multipleArchetypes && active != null) return active.Apply(xml);
            }
            else if (setting == "expensiveBase")
            {
                if (SuperheroGenes_Settings.expensiveBase && active != null) return active.Apply(xml);
            }
            else if (setting == "supersEverywhere")
            {
                if (SuperheroGenes_Settings.supersEverywhere && active != null) return active.Apply(xml);
            }
            else if (setting == "activatableSuperGenes")
            {
                if (SuperheroGenes_Settings.activatableSuperGenes && active != null) return active.Apply(xml);
            }
            else if (setting == "interruptibleActivatables")
            {
                if (SuperheroGenes_Settings.activatableSuperGenes && SuperheroGenes_Settings.interruptibleActivatables && active != null) return active.Apply(xml);
            }
            else if (setting == "middleGrounds")
            {
                if (SuperheroGenes_Settings.middleGrounds && active != null) return active.Apply(xml);
            }
            else if (setting == "allGrounds")
            {
                if (SuperheroGenes_Settings.middleGrounds && SuperheroGenes_Settings.allGrounds && active != null) return active.Apply(xml);
            }
            else if (setting == "radiomancerOvercharge")
            {
                if (!SuperheroGenes_Settings.radiomancerOvercharge && inactive != null) return inactive.Apply(xml);
            }

            // AI stuff

            else if (setting == "poolUsage")
            {
                if (SuperheroGenes_Settings.poolUsage && active != null) return active.Apply(xml);
            }

            else if (setting == "automaticHealer")
            {
                if (SuperheroGenes_Settings.automaticHealer && active != null) return active.Apply(xml);
            }

            else if (setting == "automaticDefense")
            {
                if (SuperheroGenes_Settings.automaticDefense && active != null) return active.Apply(xml);
            }

            else if (setting == "automaticDefenseDrafted")
            {
                if (SuperheroGenes_Settings.automaticDefenseDrafted && active != null) return active.Apply(xml);
            }

            else if (setting == "automaticBuffs")
            {
                if (SuperheroGenes_Settings.automaticBuffs && active != null) return active.Apply(xml);
            }

            else if (setting == "automaticDebuffs")
            {
                if (SuperheroGenes_Settings.automaticDebuffs && active != null) return active.Apply(xml);
            }

            else if (setting == "automaticDebuffsDrafted")
            {
                if (SuperheroGenes_Settings.automaticDebuffsDrafted && active != null) return active.Apply(xml);
            }

            else if (setting == "automaticOffense")
            {
                if (SuperheroGenes_Settings.automaticOffense && active != null) return active.Apply(xml);
            }

            else if (setting == "automaticOffenseDrafted")
            {
                if (SuperheroGenes_Settings.automaticOffenseDrafted && active != null) return active.Apply(xml);
            }

            else if (setting == "automaticFleeing")
            {
                if (SuperheroGenes_Settings.automaticFleeing && active != null) return active.Apply(xml);
            }

            // Villains and Stereotypes stuff
            else if (setting == "medievalVillains")
            {
                if (SuperheroGenes_Settings.medievalVillains && active != null) return active.Apply(xml);
            }
            else if (setting == "vengefulOne")
            {
                if (!SuperheroGenes_Settings.vengefulOne && inactive != null) return inactive.Apply(xml);
            }


            else if (setting != null) Log.Error("A patch is using a setting that is either mispelled or unhandled");
            else Log.Error("A patch is using this mod's settings, but doesn't specify which one.");
            return true;
        }
    }
}
