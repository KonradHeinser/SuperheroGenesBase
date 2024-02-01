﻿using Verse;
using RimWorld;
using System.Collections.Generic;

namespace SuperHeroGenesBase
{
    public class HediffCompProperties_SeverityByMentalState : HediffCompProperties
    {
        public float defaultSeverity = 1;

        public float draftedSeverity = 2;

        public float fightingSeverity = 2; // This handles autoattacking without being drafted

        public List<MentalStateEffect> mentalStateEffects;

        public HediffCompProperties_SeverityByMentalState()
        {
            compClass = typeof(HediffComp_SeverityByMentalState);
        }
    }
}
