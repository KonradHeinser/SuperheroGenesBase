using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class SHGExtension : DefModExtension
    {
        public GeneDef relatedGene; // Gene to check for
        public bool checkNotPresent = false; // Check if the pawn does NOT have relatedGene
        public int ClotCheckInterval = 360; // Shouldn't be below 60, but I won't force it. This just determines how often it tries to heal, so a lower number means it's less likely that there will be a delay in clotting
        public float minTendQuality = 0.2f; // Never below 0
        public float maxTendQuality = 0.7f; // Never over 1
        public List<GeneDef> requiredGenesToEquip; // Require all of these on the pawn
        public List<GeneDef> requireOneOfGenesToEquip; // Require any one of these on the pawn
        public List<GeneDef> forbiddenGenesToEquip; // Require none of these are on the pawn
        public List<XenotypeDef> requireOneOfXenotypeToEquip; // Require one of these xenotypes
        public List<XenotypeDef> forbiddenXenotypesToEquip; // Require pawn is not xenotype
    }
}
