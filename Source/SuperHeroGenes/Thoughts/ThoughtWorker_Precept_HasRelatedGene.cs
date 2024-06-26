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
            {
                return ThoughtState.Inactive;
            }
            SHGExtension extension = def.GetModExtension<SHGExtension>();
            if (!extension.checkNotPresent)
            {
                return HasRelatedGene(p, extension.relatedGene);
            }
            else
            {
                return !HasRelatedGene(p, extension.relatedGene);
            }
        }

        public static bool HasRelatedGene(Pawn pawn, GeneDef relatedGene)
        {
            if (!ModsConfig.BiotechActive || pawn.genes == null)
            {
                return false;
            }
            return SHGUtilities.HasRelatedGene(pawn, relatedGene);
        }
    }
}

