using RimWorld;
using Verse;
using System.Collections.Generic;

namespace SuperHeroGenesBase
{
    public class CompProperties_Launch : CompProperties_AbilityEffect
    {
        public int maxDistance = 10;

        public StatDef distanceFactorStat;

        public List<BiomeDef> disablingBiomes; // Mostly for SoS2 compatability and similar

        public ThingDef pawnLanded;

        public ThingDef skyfallerLeaving;

        public WorldObjectDef worldObject;

        // This checks if the caravan is immobilized due to mass. Only matters if noMapTravelWhenTooMuchMass is false
        public bool noMapTravelWhileImmobilized = true;

        // This compares the total mass of everything, including animals, with the caster's max carry. Usually results in the pawn only being able to transport themselves and non-pawns
        public bool noMapTravelWhenTooMuchMass = true;

        public CompProperties_Launch()
        {
            compClass = typeof(CompAbilityEffect_Launch);
        }
    }
}
