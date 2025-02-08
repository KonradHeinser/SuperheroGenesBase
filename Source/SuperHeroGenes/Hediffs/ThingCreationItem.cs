using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class ThingCreationItem
    {
        public ThingDef thing;

        public int count = 1;

        public float chance = 1f;

        public ThingDef stuff;

        public QualityCategory quality = QualityCategory.Normal;

        public FloatRange severities = new FloatRange(0f, 9999f);

        public float weight = 1f;
    }
}
