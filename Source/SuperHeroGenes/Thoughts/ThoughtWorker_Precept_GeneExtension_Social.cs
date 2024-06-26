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
            {
                return ThoughtState.Inactive;
            }
            SHGExtension extension = def.GetModExtension<SHGExtension>();
            if (!extension.checkNotPresent)
            {
                return HasRelatedGene(otherPawn, extension.relatedGene);
            }
            else
            {
                return !HasRelatedGene(otherPawn, extension.relatedGene);
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
