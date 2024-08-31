using Verse;
using RimWorld;

namespace SuperHeroGenesBase
{
    public class CompGiveHediffsToColonistsOnDestroy : ThingComp
    {
        private CompProperties_GiveHediffsToColonistsOnDestroy Props => (CompProperties_GiveHediffsToColonistsOnDestroy)props;

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            if ((mode == DestroyMode.Vanish && Props.ignoreOnVanish) || (Props.onlyWhenKilled && mode != DestroyMode.KillFinalize && mode != DestroyMode.KillFinalizeLeavingsOnly) || previousMap == null)
            {
                return;
            }
            if (!Props.message.NullOrEmpty())
            {
                Messages.Message(Props.message, new TargetInfo(parent.Position, previousMap), MessageTypeDefOf.PositiveEvent);
            }
            foreach (Pawn item in previousMap.mapPawns.AllPawnsSpawned)
            {
                if (item.IsColonist)
                    item.health.AddHediff(Props.hediff);
            }
        }
    }
}
