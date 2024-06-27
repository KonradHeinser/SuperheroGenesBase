using System;
using Verse;
using RimWorld;

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
                return SHGUtilities.HasRelatedGene(otherPawn, extension.relatedGene);
            return !SHGUtilities.HasRelatedGene(otherPawn, extension.relatedGene);
        }
    }
}
