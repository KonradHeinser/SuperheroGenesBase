using Verse;
namespace SuperHeroGenesBase
{
    public class HediffCompProperties_HediffOnKill : HediffCompProperties
    {
        public float severityForFirstKill = 1;

        public float severityPerExtraKill = 1;

        public HediffDef hediff;

        public bool allowHumanoids = true;

        public bool allowMechanoids = true;

        public bool allowDryads = true;

        public bool allowInsects = true;

        public bool allowAnimals = true;

        public HediffCompProperties_HediffOnKill()
        {
            compClass = typeof(HediffComp_HediffOnKill);
        }
    }
}
