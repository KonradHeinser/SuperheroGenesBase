﻿using Verse;
using System.Collections.Generic;

namespace SuperHeroGenesBase
{
    public class TerrainLinker
    {
        public TerrainDef terrain;
        public List<TerrainDef> terrains;

        public int newCost = 0;

        public List<NeedLevel> pawnNeedLevels;

        // Pawn Genes
        public List<GeneDef> pawnHasAnyOfGenes;
        public List<GeneDef> pawnHasAllOfGenes;
        public List<GeneDef> pawnHasNoneOfGenes;

        // Pawn Hediffs
        public List<HediffDef> pawnHasAnyOfHediffs;
        public List<HediffDef> pawnHasAllOfHediffs;
        public List<HediffDef> pawnHasNoneOfHediffs;

        // Pawn Checks
        public List<CapCheck> pawnCapLimiters;
        public List<SkillCheck> pawnSkillLimiters;
        public List<StatCheck> pawnStatLimiters;
    }
}
