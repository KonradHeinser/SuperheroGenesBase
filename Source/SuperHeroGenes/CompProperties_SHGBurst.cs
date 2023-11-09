using System;
using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class CompProperties_SHGBurst : CompProperties_AbilityEffect
    {
        public enum Gases
        {
            Smoke = 0,
            None = 1,
            Tox = 8,
            Rot = 24
        }
        public float radius;
        public DamageDef damageDef;
        public int damageAmount = -1;
        public float armorPenetration = -1f;
        public SoundDef explosionSound = null;
        public ThingDef postExplosionThing = null; // This is usually what you want
        public float postExplosionThingChance = 0f;
        public int postExplosionSpawnThingCount = 1;
        public Gases extraGasType = Gases.Smoke; // Converted to gas type in comp
        public bool applyDamageToExplosionCellsNeighbors = false; // Should probably stay this way
        public ThingDef preExplosionThing = null; // This is usually what you want
        public float preExplosionThingChance = 0f;
        public bool damageFalloff = false;
        public int preExplosionSpawnThingCount = 1;
        public float chanceToStartFire = 0f;
        public float excludeRadius = 0f; // Usability is questionable
        public ThingDef postExplosionThingWater = null;
        public float screenShakeFactor = 0;
        public bool injureSelf = true;


        public CompProperties_SHGBurst() 
        {
            compClass = typeof(CompAbilityEffect_SHGBurst);
        }
    }
}
