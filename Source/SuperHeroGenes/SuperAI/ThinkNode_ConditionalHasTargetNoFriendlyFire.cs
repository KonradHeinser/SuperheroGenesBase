using Verse;
using Verse.AI;
using System.Linq;
using Verse.AI.Group;
using System.Collections.Generic;
using RimWorld;

namespace SuperHeroGenesBase
{
    public class ThinkNode_ConditionalHasTargetNoFriendlyFire : ThinkNode_Conditional
    {
        private float minRadius = -1;
        private AbilityDef ability = null;
        protected override bool Satisfied(Pawn pawn)
        {
            if (pawn.mindState.enemyTarget == null || (ability == null && minRadius < 0)) return false;
            Thing target = pawn.mindState.enemyTarget;
            List<Pawn> list = pawn.Map.mapPawns.AllPawnsSpawned;
            list.SortBy((Pawn c) => c.Position.DistanceToSquared(target.Position));

            float safetyRange = 0;
            if (ability != null && ability.EffectRadius > 0)
            {
                if (ability.verbProperties.warmupTime > 5) return false; // There are too many things to try to predict to accept long charge times
                if (ability.verbProperties.warmupTime > 1) safetyRange = ability.EffectRadius * 1.2f * ability.verbProperties.warmupTime; // Factors in warmup time when it may be a significant factor
                else safetyRange = ability.EffectRadius * 1.2f; // The 1.2 is an extra "just to be safe"
            }
            if (minRadius > safetyRange) safetyRange = minRadius;

            foreach (Pawn p in list)
            {
                if (p.Position.DistanceToSquared(target.Position) > minRadius) break; // Due to earlier sorting, every pawn beyond this point can be ignored
                if (p.Faction != null && !p.Faction.HostileTo(pawn.Faction)) return false; // If the pawn belongs to a non-hostile faction, treat it as friendly fire
                if (!p.HostileTo(pawn)) return false; // Secondary check for general hostility. This is intended to cover factionless things like animals while hopefully avoiding weird scenarios
            }

            return true; // If no non-enemies were found, fire away
        }
    }
}
