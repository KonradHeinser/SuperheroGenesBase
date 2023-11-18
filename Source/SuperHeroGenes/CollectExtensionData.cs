using Verse;
using System;
using RimWorld;
using System.Collections.Generic;
using System.Linq;

namespace SuperHeroGenesBase
{
    [StaticConstructorOnStartup]
    public static class CollectExtensionData
    {
        static CollectExtensionData()
        {
            foreach (ThoughtDef thoughtDef in DefDatabase<ThoughtDef>.AllDefsListForReading)
            {
                SHGExtension thoughtExtension = thoughtDef.GetModExtension<SHGExtension>();
            }
            foreach (GeneDef geneDef in DefDatabase<GeneDef>.AllDefsListForReading)
            {
                SHGExtension geneExtension = geneDef.GetModExtension<SHGExtension>();
            }
        }

        public static GeneDef relatedGene;
        public static bool checkNotPresent = false;
        public static int ClotCheckInterval = 360;
        public static float minTendQuality = 0.2f;
        public static float maxTendQuality = 0.7f;
    }
}
