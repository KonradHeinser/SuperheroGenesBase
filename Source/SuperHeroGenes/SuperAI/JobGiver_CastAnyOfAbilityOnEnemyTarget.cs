using Verse;
using Verse.AI;
using RimWorld;
using System.Collections.Generic;
using System;

namespace SuperHeroGenesBase
{
    public class JobGiver_CastAnyOfAbilityOnEnemyTarget : JobGiver_AICastAbility
    {
        private List<AbilityDef> abilities = null;
        List<Ability> presentAbilities = new List<Ability>();
        static Random rnd = new Random();
        Thing currentEnemy = null;
        Ability chosenAbility = null;

        protected override Job TryGiveJob(Pawn pawn)
        {
            presentAbilities.Clear();
            chosenAbility = null;

            currentEnemy = SHGUtilities.GetCurrentTarget(pawn);
            if (currentEnemy == null) return null;

            IntVec3 enemyPosition = currentEnemy.Position;
            bool los = GenSight.LineOfSight(pawn.Position, enemyPosition, pawn.Map);
            foreach (AbilityDef abilityDef in abilities)
            {
                if (pawn.CurJobDef == abilityDef.jobDef) return null;
                Ability tempAbility = pawn.abilities?.GetAbility(abilityDef);
                if (tempAbility != null && tempAbility.CanCast)
                {
                    if (!tempAbility.ValidateGlobalTarget(currentEnemy)) continue;
                    if(tempAbility.verb.verbProps.requireLineOfSight && !los) continue;
                    bool flag = false;
                    if (currentEnemy is Pawn otherPawn) 
                    {
                        foreach (AbilityComp compAbility in tempAbility.comps)
                        {
                            if (compAbility is CompAbilityEffect_GiveHediff comp)
                            {
                                if (SHGUtilities.HasHediff(otherPawn, comp.Props.hediffDef))
                                {
                                    flag = true;
                                    break;
                                }
                            }
                            else if (compAbility is CompAbilityEffect_GiveMultipleHediffs multiComp)
                            {
                                foreach (HediffToGive hediff in multiComp.Props.hediffsToGive)
                                {
                                    if (SHGUtilities.HasHediff(otherPawn, hediff.hediffDef))
                                    {
                                        flag = true;
                                        break;
                                    }
                                }
                                if (flag) break;
                            }
                            else if (compAbility is CompAbilityEffect_BloodDrain bloodComp)
                            {
                                if (bloodComp.Props.replacementHediff != null && SHGUtilities.HasHediff(otherPawn, bloodComp.Props.replacementHediff))
                                {
                                    flag = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (flag) continue;
                    if (tempAbility.verb.verbProps.rangeStat != null)
                    {
                        if (enemyPosition.DistanceTo(pawn.Position) < pawn.GetStatValue(tempAbility.verb.verbProps.rangeStat))
                        {
                            presentAbilities.Add(tempAbility);
                        }
                    }
                    else if (!tempAbility.def.targetRequired && tempAbility.def.EffectRadius > 0)
                    {
                        if (enemyPosition.DistanceTo(pawn.Position) < tempAbility.def.EffectRadius)
                        {
                            presentAbilities.Add(tempAbility);
                        }
                    }
                    else
                    {
                        if (enemyPosition.DistanceTo(pawn.Position) < tempAbility.verb.verbProps.range)
                        {
                            presentAbilities.Add(tempAbility);
                        }
                    }
                }
            }
            if (presentAbilities.NullOrEmpty()) return null;
            chosenAbility = presentAbilities[rnd.Next(presentAbilities.Count)];
            Log.Message("Getting target for " + chosenAbility.def);
            LocalTargetInfo target = GetTarget(pawn, chosenAbility);
            if (!target.IsValid) return null;
            return chosenAbility.GetJob(target, target);
        }

        protected override LocalTargetInfo GetTarget(Pawn caster, Ability ability)
        {
            // If it's supposed to be cast on the caster (i.e. a burst) return the caster
            if (!ability.def.targetRequired) return new LocalTargetInfo(caster);
            // If targetting a pawn, but can't cast pawns, just target the ground
            if (currentEnemy is Pawn pawnTarget && !ability.verb.verbProps.targetParams.canTargetPawns) 
            {
                return new LocalTargetInfo(caster.mindState.enemyTarget.Position);
            }
            if (!ability.CanApplyOn(new LocalTargetInfo(currentEnemy))) return LocalTargetInfo.Invalid;
            return new LocalTargetInfo(currentEnemy);
        }
    }
}
