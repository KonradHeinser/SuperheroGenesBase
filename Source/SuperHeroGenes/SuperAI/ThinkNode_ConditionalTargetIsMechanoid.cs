using Verse;
using Verse.AI;

namespace SuperHeroGenesBase
{
    public class ThinkNode_ConditionalTargetIsMechanoid : ThinkNode_Conditional
    {
        protected override bool Satisfied(Pawn pawn)
        {
            Thing enemy = SHGUtilities.GetCurrentTarget(pawn);
            if (enemy == null) return false;
            if (enemy is Pawn target)
            {
                return target.RaceProps.IsMechanoid;
            }
            return false;
        }
    }
}
