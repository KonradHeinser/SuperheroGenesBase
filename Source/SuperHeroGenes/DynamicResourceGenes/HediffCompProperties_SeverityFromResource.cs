using Verse;
namespace SuperHeroGenesBase
{
    public class HediffCompProperties_SeverityFromResource : HediffCompProperties
    {
        public float severityPerHourEmpty;

        public float severityPerHourResource;

        public GeneDef mainResourceGene;

        public HediffCompProperties_SeverityFromResource()
        {
            compClass = typeof(HediffComp_SeverityFromResource);
        }
    }
}
