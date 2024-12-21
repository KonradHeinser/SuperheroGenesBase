using Verse;
using System.Collections.Generic;

namespace SuperHeroGenesBase
{
    public class HediffsToParts
    {
        public List<BodyPartDef> bodyParts;
        public HediffDef hediff;
        public bool onlyIfNew = false;
        public float severity = 0.5f;
        public List<ThingDef> validThings;
    }
}
