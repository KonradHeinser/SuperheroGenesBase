using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class ThoughtWorker_Bloodthirsty : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            Need_BloodThirst need_BloodThirst = p.needs?.TryGetNeed<Need_BloodThirst>();
            return need_BloodThirst != null && need_BloodThirst.CurLevel <= 0.3f;
        }
    }
}
