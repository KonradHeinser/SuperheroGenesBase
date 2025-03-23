using Verse;
using Verse.AI;

namespace SuperHeroGenesBase
{
    public class ThinkNode_ConditionalUsingRangedWeapon : ThinkNode_Conditional
    {
        private bool onlyPassIfDraftedOrAttacker = true;
        protected override bool Satisfied(Pawn pawn)
        {
            if (onlyPassIfDraftedOrAttacker && (!pawn.Drafted || !SHGUtilities.AutoAttackingColonist(pawn))) return false;
            return pawn.equipment.Primary != null && pawn.equipment.Primary.def.IsRangedWeapon;
        }
    }
}
