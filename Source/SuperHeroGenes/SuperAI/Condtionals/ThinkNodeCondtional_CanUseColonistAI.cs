using Verse;
using Verse.AI;

namespace SuperHeroGenesBase
{
    public class ThinkNode_ConditionalCanUseColonistAI : ThinkNode_Conditional
    {
        protected override bool Satisfied(Pawn pawn)
        {
            return pawn.IsColonist && !SuperheroGenes_Settings.disableColonistAI;
        }
    }
}
