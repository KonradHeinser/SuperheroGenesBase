using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class ThoughtWorker_Precept_GeneExtension_Social : ThoughtWorker_Precept_Social
    {
        protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
        {
            if (!ModsConfig.BiotechActive || !ModsConfig.IdeologyActive)
                return ThoughtState.Inactive;

            SHGExtension extension = def.GetModExtension<SHGExtension>();
            if (!extension.checkNotPresent)
                return SHGUtilities.HasAnyOfRelatedGene(otherPawn, extension.relatedGenes);
            return !SHGUtilities.HasAnyOfRelatedGene(otherPawn, extension.relatedGenes);
        }
    }
}
