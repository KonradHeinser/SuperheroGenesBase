﻿using Verse;
using Verse.AI;
using System.Collections.Generic;
using RimWorld;

namespace SuperHeroGenesBase
{
    public class ThinkNode_ConditionalTooManyNearbyThreats : ThinkNode_Conditional
    {
        private float dangerRadius = 5; // Defaults to checking for any nearby melee enemy
        private int minCount = 1;
        protected override bool Satisfied(Pawn pawn)
        {
            if (!pawn.Spawned || pawn.Faction == null) return false;
            Map map = pawn.Map;
            if (!GenHostility.AnyHostileActiveThreatTo(map, pawn.Faction, false, false)) return false;
            List<Pawn> list = pawn.Map.mapPawns.AllPawns;
            list.SortBy((Pawn c) => c.Position.DistanceToSquared(pawn.Position));
            int count = 0;
            foreach (Pawn p in list)
            {
                if (p.Position.DistanceTo(pawn.Position) > dangerRadius) break;
                if (p.Downed || p.Dead || !p.HostileTo(pawn)) continue;
                CompCanBeDormant comp = p.GetComp<CompCanBeDormant>();
                if (comp != null && !comp.Awake) continue;
                count++;
                if (count >= minCount) return true;
            }
            return false;
        }
    }
}
