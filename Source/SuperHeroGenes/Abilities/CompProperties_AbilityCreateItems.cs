using System.Collections.Generic;
using Verse;
using RimWorld;

namespace SuperHeroGenesBase
{
    public class CompProperties_AbilityCreateItems : CompProperties_AbilityEffect
    {
        public List<ThingPatternPart> thingPattern;

        public bool sendSkipSignal = true;

        public CompProperties_AbilityCreateItems()
        {
            compClass = typeof(CompAbilityAffect_CreateItems);
        }
    }
}
