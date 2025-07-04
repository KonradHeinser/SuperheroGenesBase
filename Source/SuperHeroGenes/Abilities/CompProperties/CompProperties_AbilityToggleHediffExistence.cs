using System;
using System.Collections.Generic;
using System.Linq;

using RimWorld;
using Verse;
using Verse.AI;

namespace SuperHeroGenesBase
{
    public class CompProperties_AbilityToggleHediffExistence : CompProperties_AbilityEffect
    {
        public HediffsToParts hediff;

        public CompProperties_AbilityToggleHediffExistence()
        {
            compClass = typeof(CompAbilityEffect_ToggleHediffExistence);
        }
    }
}
