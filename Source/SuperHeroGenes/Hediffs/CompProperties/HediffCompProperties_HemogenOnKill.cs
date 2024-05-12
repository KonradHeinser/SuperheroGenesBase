using Verse;
using System.Collections.Generic;

namespace SuperHeroGenesBase
{
    public class HediffCompProperties_HemogenOnKill : HediffCompProperties
    {
        // Only works on things that can bleed to avoid weird situations

        public float hemogenEfficiency = 1f; // % efficiency. At 1, for every 10% of bloodloss added to the corpse, gain 0.1 hemogen, or 10 in game, while at 2, that would result in a gain of 0.2

        public float maxGainableHemogen = 1f; // Max hemogen gainable from one corpse. Max bloodloss is this divided by efficiency

        public HediffCompProperties_HemogenOnKill()
        {
            compClass = typeof(HediffComp_HemogenOnKill);
        }
    }
}
