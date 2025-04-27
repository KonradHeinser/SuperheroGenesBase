using System.Xml;
using Verse;

namespace SuperHeroGenesBase
{
    public class PatchOperationSetting : PatchOperation
    {
        private PatchOperation active = null;

        private PatchOperation inactive = null;

        private PatchOperation option0 = null;

        private PatchOperation option1 = null;

        private PatchOperation option2 = null;

        private PatchOperation option3 = null;

        private PatchOperation option4 = null;

        private PatchOperation option5 = null;

        public string setting;

        protected override bool ApplyWorker(XmlDocument xml)
        {
            if (setting == "condensedMeteors")
            {
                if (!SuperheroGenes_Settings.condensedMeteors && inactive != null) return inactive.Apply(xml);
            }
            else if (setting == "multipleMutations")
            {
                if (SuperheroGenes_Settings.multipleMutations && active != null) return active.Apply(xml);
            }
            else if (setting == "multipleArchetypes")
            {
                if (SuperheroGenes_Settings.multipleArchetypes && active != null) return active.Apply(xml);
                if (!SuperheroGenes_Settings.multipleArchetypes && inactive != null) return inactive.Apply(xml);
            }
            else if (setting == "expensiveBase")
            {
                if (SuperheroGenes_Settings.expensiveBase && active != null) return active.Apply(xml);
            }
            else if (setting == "supersEverywhere")
            {
                if (SuperheroGenes_Settings.supersEverywhere && active != null) return active.Apply(xml);
            }
            else if (setting == "archetypesEverywhere")
            {
                if (SuperheroGenes_Settings.supersEverywhere && SuperheroGenes_Settings.archetypesEverywhere && active != null) return active.Apply(xml);
            }
            else if (setting == "mutationsAnywhere")
            {
                if (SuperheroGenes_Settings.supersEverywhere && SuperheroGenes_Settings.mutationsAnywhere && active != null) return active.Apply(xml);
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
            else if (setting == "superDrugNoTrader")
            {
                if (SuperheroGenes_Settings.superDrugNoTrader && active != null) return active.Apply(xml);
            }
            else if (setting == "superDrugNoReward")
            {
                if (SuperheroGenes_Settings.superDrugNoReward && active != null) return active.Apply(xml);
            }
            else if (setting == "antiSupeDisease")
            {
                if (SuperheroGenes_Settings.antiSupeDisease && active != null) return active.Apply(xml);
            }
            else if (setting == "baseAbilityCooldown")
            {
                int cooldownSetting = SuperheroGenes_Settings.baseAbilityCooldown;
                if (cooldownSetting == 0 && option0 != null) return option0.Apply(xml);
                if (cooldownSetting == 1 && option1 != null) return option1.Apply(xml);
                if (cooldownSetting == 2 && option2 != null) return option2.Apply(xml);
                if (cooldownSetting == 3 && option3 != null) return option3.Apply(xml);
                if (cooldownSetting == 4 && option4 != null) return option4.Apply(xml);
                if (cooldownSetting == 5 && option5 != null) return option5.Apply(xml);
            }
            else if (setting == "noPsionicNeurotrainers")
            {
                if (SuperheroGenes_Settings.noPsionicNeurotrainers && active != null) return active.Apply(xml);
            }
            else if (setting == "disableEvolvingHemomancers")
            {
                if (SuperheroGenes_Settings.disableEvolvingHemomancers && active != null) return active.Apply(xml);
            }

            // AI stuff

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

            else if (setting == "automaticRadioPurge")
            {
                if (SuperheroGenes_Settings.automaticRadioPurge && active != null) return active.Apply(xml);
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

            // Hero Organization Stuff

            else if (setting == "medievalHeroes")
            {
                if (SuperheroGenes_Settings.medievalHeroes && active != null) return active.Apply(xml);
            }
            else if (setting == "leagueGathering")
            {
                if (SuperheroGenes_Settings.leagueGathering && active != null) return active.Apply(xml);
            }
            else if (setting == "radiantQuests")
            {
                if (SuperheroGenes_Settings.radiantQuests && active != null) return active.Apply(xml);
            }

            else if (setting != null) Log.Error($"A patch is {setting}, which is either mispelled or unhandled");
            else Log.Error("A patch is using this mod's settings, but doesn't specify which one.");
            return true;
        }
    }
}
