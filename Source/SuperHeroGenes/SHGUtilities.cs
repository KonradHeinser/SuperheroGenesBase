using Verse;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;
using System;

namespace SuperHeroGenesBase
{
    public class SHGUtilities
    {
        public static List<HediffDef> ApplyHediffs(Pawn pawn, HediffDef hediff = null, List<HediffDef> hediffs = null)
        {
            List<HediffDef> addedHediffs = new List<HediffDef>();
            if (hediff != null)
            {
                Hediff checkedHediff = pawn.health.hediffSet?.GetFirstHediffOfDef(hediff);
                if (checkedHediff == null)
                {
                    addedHediffs.Add(hediff);
                    Hediff newHediff = HediffMaker.MakeHediff(hediff, pawn);
                    newHediff.Severity = 0.5f;
                    pawn.health.AddHediff(newHediff);
                }
            }
            if (!hediffs.NullOrEmpty())
            {
                foreach (HediffDef hediffDef in hediffs)
                {
                    Hediff checkedHediff = pawn.health.hediffSet?.GetFirstHediffOfDef(hediffDef);
                    if (checkedHediff == null)
                    {
                        addedHediffs.Add(hediffDef);
                        Hediff newHediff = HediffMaker.MakeHediff(hediffDef, pawn);
                        newHediff.Severity = 0.5f;
                        pawn.health.AddHediff(newHediff);
                    }
                }
            }
            return addedHediffs;
        }

        public static void RemoveHediffs(Pawn pawn, HediffDef hediff = null, List<HediffDef> hediffs = null)
        {

            if (hediff != null)
            {
                Hediff hediffToRemove = pawn.health.hediffSet.GetFirstHediffOfDef(hediff);
                if (hediffToRemove != null) pawn.health.RemoveHediff(hediffToRemove);
            }

            if (!hediffs.NullOrEmpty())
            {
                foreach (HediffDef hediffDef in hediffs)
                {
                    Hediff hediffToRemove = pawn.health.hediffSet.GetFirstHediffOfDef(hediffDef);
                    if (hediffToRemove != null) pawn.health.RemoveHediff(hediffToRemove);
                }
            }
        }

        public static bool EqualCountingDictionaries(Dictionary<string, int> dictionary1, Dictionary<string, int> dictionary2)
        {
            foreach (string phrase in dictionary1.Keys)
            {
                if (!dictionary2.ContainsKey(phrase)) return false;
                if (dictionary1[phrase] != dictionary2[phrase]) return false;
                dictionary2.Remove(phrase);
            }
            if (!dictionary2.NullOrEmpty()) return false;
            return true;
        }


        // AI stuff

        public static bool NeedToMove(Ability ability, Pawn pawn, Pawn targetPawn = null)
        {
            if (ability.verb.verbProps.rangeStat != null) // If there's a range stat and the target is at risk for becoming too far away, move closer
            {
                if (targetPawn.pather.Moving)
                {
                    if (targetPawn.Position.DistanceTo(pawn.Position) > pawn.GetStatValue(ability.verb.verbProps.rangeStat))
                    {
                        return true;
                    }
                }
                else
                {
                    if (targetPawn.Position.DistanceTo(pawn.Position) > Math.Ceiling(pawn.GetStatValue(ability.verb.verbProps.rangeStat) / 2))
                    {
                        return true;
                    }
                }
            }
            else // If there's no range stat, just try to get in normal range
            {
                if (targetPawn.pather.Moving)
                {
                    if (targetPawn.Position.DistanceTo(pawn.Position) > ability.verb.verbProps.range)
                    {
                        return true;
                    }
                }
                else
                {
                    if (targetPawn.Position.DistanceTo(pawn.Position) > Math.Ceiling(ability.verb.verbProps.range / 2))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static Job GoToTarget(LocalTargetInfo target)
        {
            Job job = JobMaker.MakeJob(JobDefOf.Goto, target);
            job.checkOverrideOnExpire = true;
            job.expiryInterval = 500;
            job.collideWithPawns = true;
            return job;
        }
    }
}
