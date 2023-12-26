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
            if (gene == null || !pawn.genes.HasGene(gene)) return false;
            try
            {
                Gene_Resource resourceGene = (Gene_Resource)pawn.genes.GetGene(gene);
                if (useTargetValue) return resourceGene.Value >= resourceGene.targetValue;
                return resourceGene.Value >= minLevel;
            }
            catch
            {
                Log.Error(gene + " doesn't appear to be a resource gene");
                return false;
            }
        }
    }
}
