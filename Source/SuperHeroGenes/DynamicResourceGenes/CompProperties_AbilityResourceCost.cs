﻿using System.Collections.Generic;
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
            if (resourceCost > 0) yield return (string)("ResourceCost".Translate(mainResourceGene.resourceLabel) + ": ") + Mathf.RoundToInt(resourceCost * 100f);
            else yield return (string)("ResourceGain".Translate(mainResourceGene.resourceLabel) + ": ") + Mathf.RoundToInt(resourceCost * -100f);
        }
    }
}
