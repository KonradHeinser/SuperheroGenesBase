﻿using Verse;
using Verse.AI;
using System.Collections.Generic;

namespace SuperHeroGenesBase
{
    public class ThinkNode_ConditionalTendableAlly : ThinkNode_Conditional
    {
        protected override bool Satisfied(Pawn pawn)
        {
            List<Pawn> allies = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
            foreach (Pawn ally in allies) // Then look for any tendable hediff
            {
                if (ally.health.hediffSet.HasTendableHediff()) return true;
            }
            return false;
        }
    }
}
