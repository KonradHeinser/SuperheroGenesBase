using Verse;
using RimWorld;

namespace SuperHeroGenesBase
{
    public class CompProperties_LongDistanceTeleport : CompProperties_AbilityEffect
    {
        public IntRange stunTicks;

        public EffecterDef effecterUsed;

        public EffecterDef maintainedEffect;

        public int maintainDuration = 60;

        public int maxDistance = -1;

        public EffecterDef exitEffecter;

        public SoundDef soundPlayed;

        public SoundDef exitSound;

        public bool requireAllyAtDestination = true;

        public bool bringCorpses = false;

        public bool onlyAllies = true;

        public CompProperties_LongDistanceTeleport()
        {
            compClass = typeof(CompAbilityEffect_LongDistanceTeleport);
        }
    }
}
