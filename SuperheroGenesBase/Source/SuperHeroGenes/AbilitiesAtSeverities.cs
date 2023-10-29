﻿using System;
using RimWorld;
using Verse;
using System.Collections.Generic;

namespace SuperHeroGenesBase
{
    public class AbilitiesAtSeverities
    {
        public List<AbilityDef> abilityDefs; // If multiple abilities should be added in a set you can use this
        public AbilityDef abilityDef; // If only one ability should be added, or abilities cascade based on severity without dissappearing at higher levels, use this
        public float minSeverity = 0; // By default gives ability while hediff exists
        public float maxSeverity = 1; // By default removes ability at normal max hediff severity
    }
}
