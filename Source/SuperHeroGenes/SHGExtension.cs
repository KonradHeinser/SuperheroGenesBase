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
        public List<HediffsToParts> hediffsToApply;
        public bool vanishingGene = false;

        // Used in ThoughtWorker_Gene_GeneSocial
        public bool compoundingHatred = false; // When true, each gene that is found in checked genes increases the stage
        public int maxStages = 1; // Required if compoundingHatred is true
        public List<GeneDef> checkedGenes; // Genes checked for opinions
        public List<GeneDef> nullifyingGenes; // Genes checked for early nullification. These cause the thought to never appear
    }
}
