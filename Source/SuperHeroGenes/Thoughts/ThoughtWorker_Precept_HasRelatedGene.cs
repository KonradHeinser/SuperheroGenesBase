using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class ThoughtWorker_Precept_HasRelatedGene : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {
            if (!ModsConfig.BiotechActive || !ModsConfig.IdeologyActive)
                return ThoughtState.Inactive;

            SHGExtension extension = def.GetModExtension<SHGExtension>();

            if (!extension.checkNotPresent)
                return SHGUtilities.HasAnyOfRelatedGene(p, extension.relatedGenes);

            return !SHGUtilities.HasAnyOfRelatedGene(p, extension.relatedGenes);
        }
    }
}

