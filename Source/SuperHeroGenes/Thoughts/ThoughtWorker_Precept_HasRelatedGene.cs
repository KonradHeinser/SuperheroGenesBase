using System;
using Verse;
using RimWorld;

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
                return SHGUtilities.HasRelatedGene(p, extension.relatedGene);

            return !SHGUtilities.HasRelatedGene(p, extension.relatedGene);
        }
    }
}

