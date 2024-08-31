using Verse;
using RimWorld;

namespace SuperHeroGenesBase
{
    public class CompProperties_GiveHediffsToColonistsOnDestroy : CompProperties
    {
        public HediffDef hediff;

        [MustTranslate]
        public string message;

        public bool onlyWhenKilled;

        public bool ignoreOnVanish;

        public CompProperties_GiveHediffsToColonistsOnDestroy()
        {
            compClass = typeof(CompGiveHediffsToColonistsOnDestroy);
        }
    }
}
