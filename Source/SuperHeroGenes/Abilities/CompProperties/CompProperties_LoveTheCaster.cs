using System;
using Verse;
using RimWorld;
using System.Collections.Generic;

namespace SuperHeroGenesBase
{
    public class CompProperties_LoveTheCaster : CompProperties_AbilityEffect
    {
        public HediffDef hediffToApply;

        public CompProperties_LoveTheCaster()
        {
            compClass = typeof(CompAbilityEffect_LoveTheCaster);
        }
    }
}
