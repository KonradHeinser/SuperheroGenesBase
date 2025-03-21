using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    [DefOf]
    public static class SHGDefOf
    {
        public static HediffDef SecondHeart;
        public static HistoryEventDef SHG_PropagateSuperGene;
        public static GeneDef SuperHeroBase;
        public static StatDef SHG_HemomancyProficiency;

        public static HediffDef SHG_EverEvolving_Foodless;
        public static HediffDef SHG_EverEvolving_Enlightenment;
        [MayRequire("EBSG.Framework")]
        public static HediffDef SHG_EverEvolving_Lethality;
    }
}
