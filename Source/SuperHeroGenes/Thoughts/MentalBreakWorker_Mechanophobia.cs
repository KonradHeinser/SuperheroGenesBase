﻿using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace SuperHeroGenesBase
{
    public class MentalBreakWorker_Mechanophobia : MentalBreakWorker
    {
        public override bool BreakCanOccur(Pawn pawn)
        {
            if (!pawn.Spawned || !base.BreakCanOccur(pawn)) return false;
            return pawn.Map.mapPawns.AllPawnsSpawned.Where((arg) => arg.Spawned &&
                arg.RaceProps.IsMechanoid && !arg.DeadOrDowned && arg.Awake() &&
                arg.Position.DistanceTo(pawn.Position) <= def.GetModExtension<SHGExtension>().radius).Any();
        }
    }
}
