using Verse;

namespace SuperHeroGenesBase
{
    public class ThingPatternPart
    {
        public int count = 1;

        public ThingDef thing;

        public IntVec2 relativeLocation = new IntVec2(0, 0);

        public bool allowOnBuildings = false;

        public bool skipIfBlocked = true; // When false, the location being filled or unstandable blocks the ability. Leaving this as true allows for partial successes
    }
}
