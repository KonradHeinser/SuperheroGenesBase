using UnityEngine;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class DRGExtension : DefModExtension
    {
        // Tied to ResourceGene
        public bool isMainGene;
        public List<ThingDef> resourcePacks; // This is for the main resource packs, and should not include generic ingestibles
        public HediffDef cravingHediff;
        public StatDef gainStat;
        public float maximum = 1f; // This is the default for Hemogen
        public StatDef maxStat; // Overrides maximum
        public StatDef maxFactorStat; // Works with either
        public bool resourcePacksAllowed = true;
        public Color barColor = new ColorInt(138, 3, 3).ToColor;
        public Color BarHighlightColor = new ColorInt(145, 42, 42).ToColor;
        public ThingDef iconThing = ThingDefOf.HemogenPack;

        // Ingestion stuff - Offsets resource based on type
        public bool checkIngestion = false; // The only ingestible hemogen checks is humanlike
        public float humanlikeIngestionEffect = 0f; // For hemogen this is 0.0375
        public float genericMeatIngestionEffect = 0f;
        public float eggIngestionEffect = 0f;
        public float drugIngestionEffect = 0f;
        public float corpseIngestionEffect = 0f;

        // Tied to ResourceDrainGene
        public GeneDef mainResourceGene;

    }
}
