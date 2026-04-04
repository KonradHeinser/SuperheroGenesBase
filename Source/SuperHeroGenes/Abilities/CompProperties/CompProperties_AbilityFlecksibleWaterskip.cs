using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class CompProperties_AbilityFlecksibleWaterskip : CompProperties_AbilityEffect
    {
        public FleckDef fleck;
        
        public CompProperties_AbilityFlecksibleWaterskip()
        {
            compClass = typeof(CompAbilityEffect_FlecksibleWaterskip);
        }
    }
}