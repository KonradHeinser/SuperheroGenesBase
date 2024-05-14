using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class CompProperties_AbilityTeleMoveIt : CompProperties_AbilityEffect
    {
        public float maxBodySize = 999f;

        public IntRange stunTicks;

        public float distanceToMoveIt;

        public CompProperties_AbilityTeleMoveIt()
        {
            compClass = typeof(CompAbilityEffect_TeleMoveIt);
        }
    }
}
