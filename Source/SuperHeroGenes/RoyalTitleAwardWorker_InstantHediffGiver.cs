using Verse;
using RimWorld;

namespace SuperHeroGenesBase
{
    public class RoyalTitleAwardWorker_InstantHediffGiver : RoyalTitleAwardWorker_Instant
    {
        public override void OnPreAward(Pawn pawn, Faction faction, RoyalTitleDef currentTitle, RoyalTitleDef newTitle)
        {
            if (currentTitle != null && currentTitle.HasModExtension<SHGExtension>())
            {
                SHGExtension extension = currentTitle.GetModExtension<SHGExtension>();
                SHGUtilities.RemoveHediffsFromParts(pawn, extension.hediffsToApply);
            }

            if (newTitle != null && newTitle.HasModExtension<SHGExtension>())
            {
                SHGExtension extension = newTitle.GetModExtension<SHGExtension>();
                SHGUtilities.AddHediffsToParts(pawn, extension.hediffsToApply);
            }
        }
    }
}
