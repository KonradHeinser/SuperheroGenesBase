using System.Collections.Generic;
using Verse;
using RimWorld;

namespace SuperHeroGenesBase
{
    public class CompProperties_AbilityAbilityValidator : CompProperties_AbilityEffect
    {
        // Adds custom requirements for an ability to be usable on a target
        public bool disableGizmo = false; // Disables gizmo when caster or map conditions prevent the ability from working

        public List<GeneDef> targetHasAnyOfGenes;
        public List<GeneDef> targetHasAllOfGenes;
        public List<GeneDef> targetHasNoneOfGenes;

        public List<HediffDef> targetHasAnyOfHediffs;
        public List<HediffDef> targetHasAllOfHediffs;
        public List<HediffDef> targetHasNoneOfHediffs;

        // % Light
        public float minTargetLightLevel = 0f;
        public float maxTargetLightLevel = 1f;
        public float minCasterLightLevel = 0f;
        public float maxCasterLightLevel = 1f;

        // % of progress through the day
        public float minPartOfDay = 0f;
        public float maxPartOfDay = 1f;

        public CompProperties_AbilityAbilityValidator()
        {
        }
    }
}
