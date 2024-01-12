using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class CompAbilityEffect_SHGBurst : CompAbilityEffect
    {
        public new CompProperties_SHGBurst Props => (CompProperties_SHGBurst)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            List<Thing> ignoreList = new List<Thing>();
            Pawn caster = parent.pawn;
            float radius = Props.radius;
            if (Props.statRadius != null && caster.GetStatValue(Props.statRadius) > 0) radius = caster.GetStatValue(Props.statRadius);
            if (!Props.injureSelf)
            {
                ignoreList.Add(caster);
            }
            if ((int)Props.extraGasType != 1)
            {
                GenExplosion.DoExplosion(caster.Position, caster.Map, radius, Props.damageDef, caster, Props.damageAmount,
                    Props.armorPenetration, Props.explosionSound, null, null, null, Props.postExplosionThing, Props.postExplosionThingChance, 
                    Props.postExplosionSpawnThingCount, (GasType)(int)Props.extraGasType, Props.applyDamageToExplosionCellsNeighbors, 
                    Props.preExplosionThing, Props.preExplosionThingChance, Props.preExplosionSpawnThingCount, Props.chanceToStartFire,
                    Props.damageFalloff, null, ignoreList, null, true, 1f, Props.excludeRadius, true, 
                    Props.postExplosionThingWater, Props.screenShakeFactor);
            }
            else
            {
                GenExplosion.DoExplosion(caster.Position, caster.Map, radius, Props.damageDef, caster, Props.damageAmount,
                    Props.armorPenetration, Props.explosionSound, null, null, null, Props.postExplosionThing, Props.postExplosionThingChance,
                    Props.postExplosionSpawnThingCount, null, Props.applyDamageToExplosionCellsNeighbors, Props.preExplosionThing, 
                    Props.preExplosionThingChance, Props.preExplosionSpawnThingCount, Props.chanceToStartFire,Props.damageFalloff, null, ignoreList,
                    null, true, 1f, Props.excludeRadius, true, Props.postExplosionThingWater,Props.screenShakeFactor);
            }
        }
    }
}
