using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;

namespace SuperHeroGenesBase
{
    public class CompProperties_AbilityResourceCost : CompProperties_AbilityEffect
    {
        public float resourceCost;

        public GeneDef mainResourceGene;

        public CompProperties_AbilityResourceCost()
        {
            compClass = typeof(CompAbilityEffect_ResourceCost);
        }

        public override IEnumerable<string> ExtraStatSummary()
        {
            yield return (string)("Cost".Translate(mainResourceGene.label) + ": ") + Mathf.RoundToInt(resourceCost * 100f);
        }
    }
}
