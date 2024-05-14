﻿using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace SuperHeroGenesBase
{
    public class HediffComp_GiveHediffsToNonAlliesInRange : HediffComp
    {
        private Mote mote;
        public HediffCompProperties_GiveHediffsToNonAlliesInRange Props => (HediffCompProperties_GiveHediffsToNonAlliesInRange)props;

        public override void CompPostTick(ref float severityAdjustment)
        {
            if (!Pawn.Awake() || Pawn.health == null || Pawn.health.InPainShock || !Pawn.Spawned || (Props.onlyWhileDrafted && !Pawn.Drafted && Pawn.IsPlayerControlled))
                return;

            // Get all a list of all pawns, and a list of all allied pawns
            List<Pawn> list = parent.pawn.Map.mapPawns.AllPawns;
            list.SortBy((Pawn c) => c.Position.DistanceToSquared(Pawn.Position));
            List<Pawn> allies = Pawn.Map.mapPawns.SpawnedPawnsInFaction(Pawn.Faction);

            if (!Props.hideMoteWhenNotDrafted || Pawn.Drafted)
            {
                if (Props.mote != null && (mote == null || mote.Destroyed))
                    mote = MoteMaker.MakeAttachedOverlay(parent.pawn, Props.mote, Vector3.zero);
                if (mote != null)
                    mote.Maintain();
            }

            if (!list.NullOrEmpty())
            {
                float range = Props.rangeStat != null ? Pawn.GetStatValue(Props.rangeStat) : Props.range;
                foreach (Pawn item in list)
                {
                    if (allies.Contains(item) || (item.Faction != null && item.Faction.AllyOrNeutralTo(Pawn.Faction))) continue; // If it's an ally/non-enemy
                    if (item.Dead || item.health == null || (Props.targetingParameters != null && !Props.targetingParameters.CanTarget(item))) continue;

                    if (item.Position.DistanceTo(Pawn.Position) > range) break;
                    if (Props.psychic && item.GetStatValue(StatDefOf.PsychicSensitivity) == 0) continue;

                    Hediff hediff = item.health.hediffSet.GetFirstHediffOfDef(Props.hediff);
                    if (hediff == null)
                    {
                        hediff = item.health.AddHediff(Props.hediff, item.health.hediffSet.GetBrain());
                        hediff.Severity = Props.initialSeverity;
                        HediffComp_Link hediffComp_Link = hediff.TryGetComp<HediffComp_Link>();
                        if (hediffComp_Link != null)
                        {
                            hediffComp_Link.drawConnection = true;
                            hediffComp_Link.other = parent.pawn;
                        }
                    }
                    HediffComp_Disappears hediffComp_Disappears = hediff.TryGetComp<HediffComp_Disappears>();
                    if (hediffComp_Disappears == null)
                        Log.Error("HediffComp_GiveHediffsToNonAlliesInRange has a hediff in props which does not have a HediffComp_Disappears");
                    else
                        hediffComp_Disappears.ticksToDisappear = 20;
                }
            }
        }
    }
}
