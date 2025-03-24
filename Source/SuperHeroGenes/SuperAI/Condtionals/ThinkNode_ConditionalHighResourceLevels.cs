using Verse;
using Verse.AI;
using RimWorld;

namespace SuperHeroGenesBase
{
    public class ThinkNode_ConditionalHighResourceLevels : ThinkNode_Conditional
    {
        private float minLevel = 0.9f;
        private bool useTargetValue = true;
        private GeneDef gene = null;
        protected override bool Satisfied(Pawn pawn)
        {
            if (!SHGUtilities.HasRelatedGene(pawn, gene)) return false;
            if (pawn.genes.GetGene(gene) is Gene_Resource resourceGene)
            {
                if (useTargetValue) return resourceGene.Value >= resourceGene.targetValue + 0.1f;
                return resourceGene.Value >= minLevel;
            }
            else
            {
                Log.Error(gene + " doesn't appear to be a resource gene");
                return false;
            }
        }
    }
}
