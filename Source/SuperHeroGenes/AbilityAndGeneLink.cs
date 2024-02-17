﻿using RimWorld;
using System.Collections.Generic;
using Verse;

namespace SuperHeroGenesBase
{
    public class AbilityAndGeneLink
    {
        public List<AbilityDef> abilities;
        public List<GeneDef> forbiddenGenes; // None of these are allowed
        public List<GeneDef> requiredGenes; // All of these are required
        public List<GeneDef> requireOneOfGenes; // Require any one of these on the pawn
    }
}
