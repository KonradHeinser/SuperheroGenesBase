using Verse;
using Verse.AI;

namespace SuperHeroGenesBase
{
    public class ThinkNode_ConditionalHasGene : ThinkNode_Conditional
    {
        public GeneDef gene;
        protected override bool Satisfied(Pawn pawn)
        {
            return pawn.genes?.HasActiveGene(gene) == true;
        }
    }
}
