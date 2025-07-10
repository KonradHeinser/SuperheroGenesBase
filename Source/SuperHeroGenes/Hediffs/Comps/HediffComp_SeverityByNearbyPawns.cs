using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;

namespace SuperHeroGenesBase
{
    public class HediffComp_SeverityByNearbyPawns : HediffComp_SetterBase
    {
        public HediffCompProperties_SeverityByNearbyPawns Props => (HediffCompProperties_SeverityByNearbyPawns)props;

        protected override bool MustBeSpawned => true;

        protected override void SetSeverity()
        {
            base.SetSeverity();

            List<Pawn> list = Pawn.Map.mapPawns.AllPawns.Where((Pawn p) => CheckPawn(p)).ToList();
            parent.Severity = list.Count;

            ticksToNextCheck = 60;
        }

        public bool CheckPawn(Pawn p)
        {
            if (p.Dead) return false;
            if (!p.RaceProps.Humanlike && Props.onlyHumanlikes) return false;
            if (p == parent.pawn)
            {
                if (!Props.includeSelf) return false;
            }
            else
            {
                if (Props.onlySameFaction && p.Faction != parent.pawn.Faction) return false;
                if (Props.onlyDifferentFaction && (p.Faction == null || p.Faction == parent.pawn.Faction)) return false;
                if (Props.onlyEnemies && !p.HostileTo(parent.pawn)) return false;
            }
            if (p.Position.DistanceTo(parent.pawn.Position) > Props.range) return false;
            return true;
        }
    }
}
