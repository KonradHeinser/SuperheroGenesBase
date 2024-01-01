using Verse.AI;
using Verse;

namespace SuperHeroGenesBase
{
    public class ThinkNode_ConditionalNearbyEnemyTarget : ThinkNode_Conditional
    {
        private float searchRadius = 50f;
        protected override bool Satisfied(Pawn pawn)
        {
            return SHGUtilities.GetCurrentTarget(pawn, searchRadius: searchRadius) != null;
        }
    }
}
