﻿using System.Linq;
using System.Collections.Generic;
using RimWorld;
using Verse;
using UnityEngine;

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
            if (Props.statRadius != null)
            {
                if (caster.GetStatValue(Props.statRadius) > 0) radius = caster.GetStatValue(Props.statRadius);
                else radius = 0;
            }
            if (!Props.injureSelf)
            {
                ignoreList.Add(caster);
            }

            int damageAmount = Props.damageStat != null ? Mathf.FloorToInt(caster.StatOrOne(Props.damageStat)) : Props.damageAmount;

            if ((int)Props.extraGasType != 1)
            {
                GenExplosion.DoExplosion(caster.Position, caster.Map, radius, Props.damageDef, caster, damageAmount,
                    Props.armorPenetration, Props.explosionSound, null, null, null, Props.postExplosionThing, Props.postExplosionThingChance,
                    Props.postExplosionSpawnThingCount, (GasType)(int)Props.extraGasType, null, 255, Props.applyDamageToExplosionCellsNeighbors,
                    Props.preExplosionThing, Props.preExplosionThingChance, Props.preExplosionSpawnThingCount, Props.chanceToStartFire,
                    Props.damageFalloff, null, ignoreList, null, true, 1f, Props.excludeRadius, true,
                    Props.postExplosionThingWater, Props.screenShakeFactor);
            }
            else
            {
                GenExplosion.DoExplosion(caster.Position, caster.Map, radius, Props.damageDef, caster, damageAmount,
                    Props.armorPenetration, Props.explosionSound, null, null, null, Props.postExplosionThing, Props.postExplosionThingChance,
                    Props.postExplosionSpawnThingCount, null, null, 255, Props.applyDamageToExplosionCellsNeighbors, Props.preExplosionThing,
                    Props.preExplosionThingChance, Props.preExplosionSpawnThingCount, Props.chanceToStartFire, Props.damageFalloff, null, ignoreList,
                    null, true, 1f, Props.excludeRadius, true, Props.postExplosionThingWater, Props.screenShakeFactor);
            }
        }

        public override void DrawEffectPreview(LocalTargetInfo target)
        {
            float radius = Props.radius;
            if (Props.statRadius != null && parent.pawn.GetStatValue(Props.statRadius) >= 0) radius = parent.pawn.GetStatValue(Props.statRadius);

            GenDraw.DrawFieldEdges(SHGUtilities.AffectedCells(parent.pawn, parent.pawn.Map, parent.pawn, radius).ToList(), Valid(target) ? Color.white : Color.red);
        }
    }
}
