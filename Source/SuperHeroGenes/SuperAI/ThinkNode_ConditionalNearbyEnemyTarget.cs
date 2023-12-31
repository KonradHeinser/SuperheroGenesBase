using Verse.AI;
using Verse;

namespace SuperHeroGenesBase
{
    public class ThinkNode_ConditionalNearbyEnemyTarget : ThinkNode_Conditional
    {
        protected override bool Satisfied(Pawn pawn)
        {
            return SHGUtilities.GetCurrentTarget(pawn) != null;
        }
    }
}
