using System.Collections.Generic;
using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class SHGExtension : DefModExtension
    {
        public List<GeneDef> relatedGenes; // Genes to check for
        public bool checkNotPresent = false; // Check if the pawn does NOT have relatedGene
        public int ClotCheckInterval = 360; // Shouldn't be below 60, but I won't force it. This just determines how often it tries to heal, so a lower number means it's less likely that there will be a delay in clotting
        public float minTendQuality = 0.2f; // Never below 0
        public float maxTendQuality = 0.7f; // Never over 1
        public List<HediffsToParts> hediffsToApply;
        public bool vanishingGene = false;
        public bool noHediffRemoval = false;
        public SimpleCurve peopleToMoodCurve;
        public float radius = 19.9f;
        public List<GeneDef> conflictingGenes; // List of very specific genes to make it incompatible with
        public HediffsToParts hediffOnConsumption;

        public HediffDef hediff;
        public SimpleCurve curve;

        public List<AbilityAndGeneLink> geneAbilities;

        // Used in ThoughtWorker_Gene_GeneSocial
        public bool compoundingHatred = false; // When true, each gene that is found in checked genes increases the stage
        public List<GeneDef> checkedGenes; // Genes checked for opinions
        public List<GeneDef> nullifyingGenes; // Genes checked for early nullification. These cause the thought to never appear
        public List<GeneDef> requiredGenes; // The observer musthave one of these genes to feel anything. Acts as a reverse nullifyingGenes
    }
}
