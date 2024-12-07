using Verse;
using Verse.AI;

namespace SuperHeroGenesBase
{
    public class ThinkNode_ConditionalOver25YearsOld : ThinkNode_Conditional
    {
        protected override bool Satisfied(Pawn pawn)
        {
            return pawn.ageTracker.AgeBiologicalYearsFloat > 25f;
        }
    }
}
