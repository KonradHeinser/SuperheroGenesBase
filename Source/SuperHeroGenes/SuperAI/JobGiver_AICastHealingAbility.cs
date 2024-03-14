﻿using Verse;
using Verse.AI;
using System.Collections.Generic;
using RimWorld;
using System.Linq;

namespace SuperHeroGenesBase
{
    public class JobGiver_AICastHealingAbility : JobGiver_AICastAbility
    {
        private float bleedThreshold = 0.1f;

        private bool allTendable = true;

        private Pawn targetPawn = null;

        protected override Job TryGiveJob(Pawn pawn)
        {
            if (pawn.CurJobDef == this.ability.jobDef)
            {
                return null;
            }
            Ability ability = pawn.abilities?.GetAbility(this.ability);
            if (ability == null || !ability.CanCast)
            {
                return null;
            }
            LocalTargetInfo target = GetTarget(pawn, ability);
            if (!target.IsValid)
            {
                return null;
            }
            if (SHGUtilities.NeedToMove(ability, pawn, targetPawn)) return SHGUtilities.GoToTarget(target); // Check if you need to move closer

            // If close enough, just cast the ability
            if (!ability.def.targetRequired) return ability.GetJob(pawn, new LocalTargetInfo(pawn)); // For AoE's always centered on caster
            return ability.GetJob(targetPawn, target); // Abilities not intended for only the caster
        }

        protected override LocalTargetInfo GetTarget(Pawn caster, Ability ability)
        {
            List<Pawn> allies = caster.Map.mapPawns.SpawnedPawnsInFaction(caster.Faction);

            if (!allies.NullOrEmpty())
            {
                allies.SortBy((Pawn p) => p.Position.DistanceToSquared(caster.Position));
                foreach (Pawn ally in allies) // Prioritizes bleeding pawns
                {
                    if (ally.health.hediffSet.BleedRateTotal > bleedThreshold)
                    {
                        targetPawn = ally;
                        return new LocalTargetInfo(ally.Position);
                    }
                }
                if (allTendable) // If there's no notable bleeding but allowed to heal all wounds, look for any tendable pawn
                {
                    foreach (Pawn ally in allies) // Start with injuries as those are most likely to cause immediate issues
                    {

                        if (!ally.health.hediffSet.GetHediffsTendable().Where((Hediff h) => h.BleedRate > 0).ToList().NullOrEmpty())
                        {
                            targetPawn = ally;
                            return new LocalTargetInfo(ally.Position);
                        }
                    }
                }
            }
            return LocalTargetInfo.Invalid;
        }

        public override ThinkNode DeepCopy(bool resolve = true)
        {
            JobGiver_AICastHealingAbility obj = (JobGiver_AICastHealingAbility)base.DeepCopy(resolve);
            obj.ability = ability;
            obj.bleedThreshold = bleedThreshold;
            obj.allTendable = allTendable;
            return obj;
        }
    }
}
