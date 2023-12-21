using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using RimWorld;

namespace SuperHeroGenesBase
{
    public class JobGiver_AITeleportEscapeEnemies : ThinkNode_JobGiver
    {
        private AbilityDef ability;

        private static List<Thing> tmpHostileSpots = new List<Thing>();

        protected override Job TryGiveJob(Pawn pawn)
        {
            Ability ability = pawn.abilities?.GetAbility(this.ability);
            if (ability == null || !ability.CanCast || ability.comps.NullOrEmpty())
            {
                return null;
            }
            float range = 0f;
            foreach (AbilityComp comp in ability.comps)
            {
                if (comp is CompAbilityEffect_Teleport compAbilityEffect_Teleport)
                {
                    if (compAbilityEffect_Teleport.Props.range > 0) range = compAbilityEffect_Teleport.Props.range;
                    else range = compAbilityEffect_Teleport.Props.randomRange.RandomInRange;
                    break;
                }
            }
            if (TryFindRelocatePosition(pawn, out var relocatePosition, 1f))
            {
                return ability.GetJob(pawn, relocatePosition);
            }
            return null;
        }

        private bool TryFindRelocatePosition(Pawn pawn, out IntVec3 relocatePosition, float maxDistance)
        {
            tmpHostileSpots.Clear();
            tmpHostileSpots.AddRange(from a in pawn.Map.attackTargetsCache.GetPotentialTargetsFor(pawn)
                                     where !a.ThreatDisabled(pawn)
                                     select a.Thing);
            Ability jump = pawn.abilities?.GetAbility(ability);
            relocatePosition = CellFinderLoose.GetFallbackDest(pawn, tmpHostileSpots, maxDistance, 5f, 5f, 20, (IntVec3 c) => jump.verb.ValidateTarget(c, showMessages: false));
            tmpHostileSpots.Clear();
            return relocatePosition.IsValid;
        }

        public override ThinkNode DeepCopy(bool resolve = true)
        {
            JobGiver_AITeleportEscapeEnemies obj = (JobGiver_AITeleportEscapeEnemies)base.DeepCopy(resolve);
            obj.ability = ability;
            return obj;
        }
    }
}
