using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class HediffCompProperties_SeverityChangePerAttack : HediffCompProperties
    {
        public float changePerAttack;

        public float changePerMeleeAttack;

        public float changePerRangedAttack;

        public HediffCompProperties_SeverityChangePerAttack()
        {
            compClass = typeof(HediffComp_SeverityChangePerAttack);
        }
    }
}
