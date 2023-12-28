using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace SuperHeroGenesBase
{
    public class ThinkNode_ConditionalHasAnyOfHediff : ThinkNode_Conditional
    {
        private List<HediffDef> hediffs = null;
        protected override bool Satisfied(Pawn pawn)
        {
            Log.Message("Checking for hediffs");
            foreach (HediffDef hediff in hediffs)
            {
                if (SHGUtilities.HasHediff(pawn, hediff)) return true;
            }
            Log.Message("None found");
            return false;
        }
    }
}
