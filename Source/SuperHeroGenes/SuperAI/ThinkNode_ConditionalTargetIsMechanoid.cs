using Verse;
using Verse.AI;

namespace SuperHeroGenesBase
{
    public class ThinkNode_ConditionalTargetIsMechanoid : ThinkNode_Conditional
    {
        protected override bool Satisfied(Pawn pawn)
        {
            if (pawn.mindState.enemyTarget == null) return false;
            try
            {
                Pawn target = (Pawn)pawn.mindState.enemyTarget;
                return target.RaceProps.IsMechanoid;
            }
            catch
            {
                return false;
            }
            
        }
    }
}
