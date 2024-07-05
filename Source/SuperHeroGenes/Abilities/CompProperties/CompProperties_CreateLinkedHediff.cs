using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class CompProperties_CreateLinkedHediff : CompProperties_AbilityEffect
    {
        public HediffDef hediffOnCaster;

        public bool casterHediffOnBrain = false;

        public HediffDef hediffOnTarget;

        public bool targetHediffOnBrain = false;

        public CompProperties_CreateLinkedHediff()
        {
            compClass = typeof(CompAbilityEffect_CreateLinkedHediff);
        }
    }
}
