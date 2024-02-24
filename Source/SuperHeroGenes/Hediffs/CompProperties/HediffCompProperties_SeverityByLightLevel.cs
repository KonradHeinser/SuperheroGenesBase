using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class HediffCompProperties_SeverityByLightLevel : HediffCompProperties
    {
        public SimpleCurve lightToSeverityCurve;

        public SimpleCurve timeToSeverityCurve; // Back up plan

        public HediffCompProperties_SeverityByLightLevel()
        {
            compClass = typeof(HediffComp_SeverityByLightLevel);
        }
    }
}
