using Verse;
using RimWorld;

namespace SuperHeroGenesBase
{
    public class JobGiver_AICastAbilityOnOwnLocation : JobGiver_AICastAbility
    {
        protected override LocalTargetInfo GetTarget(Pawn caster, Ability ability)
        {
            if (ability.CanApplyOn(caster.Position))
                return caster.Position;
            return LocalTargetInfo.Invalid;
        }
    }
}
