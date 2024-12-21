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
        public bool noHediffRemoval = false;
        public SimpleCurve peopleToMoodCurve;
        public float radius = 19.9f;
        public List<GeneDef> conflictingGenes; // List of very specific genes to make it incompatible with
        public float maxWaterDistance = 14.9f;
        public HediffsToParts hediffOnConsumption;

        public float speed = 0.00025f;
        public StatDef relatedStat;

        public List<AbilityAndGeneLink> geneAbilities;
        public NeedDef need;

        // Used in ThoughtWorker_Gene_GeneSocial
        public bool compoundingHatred = false; // When true, each gene that is found in checked genes increases the stage
        public int maxStages = 1; // Required if compoundingHatred is true
        public List<GeneDef> checkedGenes; // Genes checked for opinions
        public List<GeneDef> nullifyingGenes; // Genes checked for early nullification. These cause the thought to never appear
        public List<GeneDef> requiredGenes; // The observer musthave one of these genes to feel anything. Acts as a reverse nullifyingGenes

        public List<FactionDef> factions; // For anything that needs a list of factions. Currently not used

        public FactionDef faction; // For the tribute class, or anything else that needs a single faction
        public string category; // For the tribute class, or anything else that needs a category listing
        public string descriptionOverrideA; // For the tribute acceptance, or anything else that needs a partial description override
        public string descriptionOverrideB; // For the tribute acceptance, or anything else that needs a partial description override
        public string descriptionOverrideC; // For the tribute acceptance, or anything else that needs a partial description override
    }
}
