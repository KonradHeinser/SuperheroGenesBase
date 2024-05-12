using Verse;
using System.Collections.Generic;

namespace SuperHeroGenesBase
{
    public class HediffCompProperties_HediffsOnRemoval : HediffCompProperties
    {
        public List<HediffToGive> hediffsToGive;

        public HediffCompProperties_HediffsOnRemoval()
        {
            compClass = typeof(HediffComp_HediffsOnRemoval);
        }
    }
}
