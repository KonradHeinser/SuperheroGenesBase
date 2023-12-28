using Verse;
using Verse.AI;

namespace SuperHeroGenesBase
{
    public class ThinkNode_ConditionalUsingRangedWeapon : ThinkNode_Conditional
    {
        protected override bool Satisfied(Pawn pawn)
        {
            return pawn.equipment.Primary != null && pawn.equipment.Primary.def.IsRangedWeapon;
        }
    }
}
