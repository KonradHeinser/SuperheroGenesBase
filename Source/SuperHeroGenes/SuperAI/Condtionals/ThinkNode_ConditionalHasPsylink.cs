using Verse;
using Verse.AI;

namespace SuperHeroGenesBase
{
    public class ThinkNode_ConditionalHasPsylink : ThinkNode_Conditional
    {
        protected override bool Satisfied(Pawn pawn)
        {
            return pawn.HasPsylink;
        }
    }
}
