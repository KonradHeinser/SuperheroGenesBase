using Verse;
using Verse.AI;

namespace SuperHeroGenesBase
{
    public class ThinkNodeCondtional_CanUseColonistAI : ThinkNode_Conditional
    {
        protected override bool Satisfied(Pawn pawn)
        {
            return pawn.IsColonist && !SuperheroGenes_Settings.disableColonistAI;
        }
    }
}
