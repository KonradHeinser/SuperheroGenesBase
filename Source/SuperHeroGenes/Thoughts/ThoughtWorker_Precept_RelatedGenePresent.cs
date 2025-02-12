using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class ThoughtWorker_Precept_RelatedGenePresent : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {
            if (!ModsConfig.BiotechActive || !ModsConfig.IdeologyActive || p.IsBloodfeeder())
                return ThoughtState.Inactive;

            SHGExtension extension = def.GetModExtension<SHGExtension>();
            if (!extension.checkNotPresent)
            {
                foreach (Pawn item in p.MapHeld.mapPawns.AllPawnsSpawned)
                    if (SHGUtilities.HasAnyOfRelatedGene(item, extension.relatedGenes) && (item.IsPrisonerOfColony || item.IsSlaveOfColony || item.IsColonist))
                        return ThoughtState.ActiveDefault;

                return ThoughtState.Inactive;
            }
            else
            {
                foreach (Pawn item in p.MapHeld.mapPawns.AllPawnsSpawned)
                    if (!SHGUtilities.HasAnyOfRelatedGene(item, extension.relatedGenes) && (item.IsPrisonerOfColony || item.IsSlaveOfColony || item.IsColonist))
                        return ThoughtState.ActiveDefault;

                return ThoughtState.Inactive;
            }
        }
    }
}
