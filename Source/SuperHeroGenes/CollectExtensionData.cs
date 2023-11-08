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
            foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
            {
                SHGExtension thingExtension = thingDef.GetModExtension<SHGExtension>();
            }
        }

        public static GeneDef relatedGene;
        public static bool checkNotPresent = false;
        public static List<GeneDef> requiredGenesToEquip;
        public static List<GeneDef> requireOneOfGenesToEquip;
        public static List<GeneDef> forbiddenGenesToEquip;
        public static List<XenotypeDef> requireOneOfXenotypeToEquip;
        public static List<XenotypeDef> forbiddenXenotypesToEquip;
    }
}
