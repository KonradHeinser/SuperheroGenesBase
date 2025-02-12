using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class ThoughtWorker_Precept_RelatedGeneColonist : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {
            if (!ModsConfig.BiotechActive || !ModsConfig.IdeologyActive || p.Faction == null)
                return ThoughtState.Inactive;
            
            _ = p.Ideo;
            bool flag = false;
            SHGExtension extension = def.GetModExtension<SHGExtension>();
            foreach (Pawn item in p.MapHeld.mapPawns.SpawnedPawnsInFaction(p.Faction))
            {
                if (!item.IsColonist) continue;

                if (!extension.checkNotPresent)
                {
                    if (SHGUtilities.HasAnyOfRelatedGene(item, extension.relatedGenes))
                    {
                        flag = true;
                        Precept_Role precept_Role = item.Ideo?.GetRole(item);
                        if (precept_Role != null && precept_Role.ideo == p.Ideo && precept_Role.def == PreceptDefOf.IdeoRole_Leader)
                            return ThoughtState.ActiveAtStage(2);
                    }
                }
                else
                {
                    if (item.IsColonist && !SHGUtilities.HasAnyOfRelatedGene(item, extension.relatedGenes))
                    {
                        flag = true;
                        Precept_Role precept_Role = item.Ideo?.GetRole(item);
                        if (precept_Role != null && precept_Role.ideo == p.Ideo && precept_Role.def == PreceptDefOf.IdeoRole_Leader)
                            return ThoughtState.ActiveAtStage(2);
                    }
                }
            }
            if (flag)
                return ThoughtState.ActiveAtStage(1);
            
            return ThoughtState.ActiveAtStage(0);
        }
    }
}
