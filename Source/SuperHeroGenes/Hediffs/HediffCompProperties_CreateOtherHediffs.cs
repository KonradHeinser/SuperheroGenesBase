﻿using Verse;
using System.Collections.Generic;

namespace SuperHeroGenesBase
{
    public class HediffCompProperties_CreateOtherHediffs : HediffCompProperties
    {
        public List<HediffsAtSeverities> hediffSets; // This allows for customizing both when the hediffs are applied, and how much they are applied for
        public float minAge = 0;

        public HediffCompProperties_CreateOtherHediffs()
        {
            compClass = typeof(HediffComp_CreateOtherHediffs);
        }
    }
}
