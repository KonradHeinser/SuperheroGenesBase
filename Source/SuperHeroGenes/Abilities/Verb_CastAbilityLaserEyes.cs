using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace SuperHeroGenesBase
{
    /*
    * Many portions of this code were originally made by the devs of the mod Biotech Gene: Laser-Eyes (https://steamcommunity.com/sharedfiles/filedetails/?id=2897565210&searchtext=laser+eyes)
    * While some portions have been trimmed down and altered to create the SHG version of laser eyes, credit for the initial concept of the code belongs to them
    * And for those counting, this makes 4 C# devs to make this one ability because Rimworld fought us every step on the way
    *    
    * Portions of the materials used to create this content/mod are trademarks and/or copyrighted works of Ludeon Studios Inc. All rights reserved by Ludeon. This content/mod is not official and is not endorsed by Ludeon.
    *
    * This is almost a direct copy of a decompiled copy of the basegame `Verse.Verb_ShootBeam`. This version however inherits from `Verb_CastAbility` instead of just `Verb` and provides the `Pawn` in place of the `EquipmentSource`.
    * A good faith attempt was made to create this by composition using a wrapped `Verse.Verb_ShootBeam` but this proved complex due to certain methods not being public or virtual.
    */

    public class Verb_CastAbilityLaserEyes : Verb_CastAbility
    {
        private List<Vector3> path = new List<Vector3>();
        private int ticksToNextPathStep;
        private Vector3 initialTargetPosition;
        private MoteDualAttached mote;
        private Effecter endEffecter;
        private Sustainer sustainer;
        private const int NumSubdivisionsPerUnitLength = 1;

        protected override int ShotsPerBurst => verbProps.burstShotCount;

        public float ShotProgress => ticksToNextPathStep / (float)verbProps.ticksBetweenBurstShots;

        public Vector3 InterpolatedPosition
        {
            get
            {
                Vector3 vector3 = CurrentTarget.CenterVector3 - initialTargetPosition;
                return Vector3.Lerp(path[burstShotsLeft],
                    path[Mathf.Min(burstShotsLeft + 1, path.Count - 1)], ShotProgress) + vector3;
            }
        }

        public override float? AimAngleOverride => state != VerbState.Bursting ? new float?() : (InterpolatedPosition - caster.DrawPos).AngleFlat();

        protected override bool TryCastShot()
        {
            if (currentTarget.HasThing && currentTarget.Thing.Map != caster.Map) return false;

            var shootLineFromTo = TryFindShootLineFromTo(caster.Position, currentTarget, out ShootLine _);
            if (verbProps.stopBurstWithoutLos && !shootLineFromTo) return false;

            lastShotTick = Find.TickManager.TicksGame;
            ticksToNextPathStep = verbProps.ticksBetweenBurstShots;
            IntVec3 intVec3_1 = InterpolatedPosition.Yto0().ToIntVec3();
            IntVec3 intVec3_2 = GenSight.LastPointOnLineOfSight(caster.Position, intVec3_1,
                c => c.CanBeSeenOverFast(caster.Map), true);
            HitCell(intVec3_2.IsValid ? intVec3_2 : intVec3_1);
            return true;
        }

        public override bool TryStartCastOn(LocalTargetInfo castTarg, LocalTargetInfo destTarg,
            bool surpriseAttack = false, bool canHitNonTargetPawns = true, bool preventFriendlyFire = false, bool nonInterruptingSelfCast = false)
        {
            return base.TryStartCastOn(verbProps.beamTargetsGround ? castTarg.Cell : castTarg,
                destTarg, surpriseAttack, canHitNonTargetPawns, preventFriendlyFire, nonInterruptingSelfCast);
        }

        public override void BurstingTick()
        {
            --ticksToNextPathStep;
            Vector3 vector3_1 = InterpolatedPosition;
            IntVec3 intVec3_1 = vector3_1.ToIntVec3();
            Vector3 vector3_2 = InterpolatedPosition - caster.Position.ToVector3Shifted();
            float num1 = vector3_2.MagnitudeHorizontal();
            Vector3 normalized = vector3_2.Yto0().normalized;
            IntVec3 intVec3_2 = GenSight.LastPointOnLineOfSight(caster.Position, intVec3_1,
                c => c.CanBeSeenOverFast(caster.Map), true);
            IntVec3 intVec3_3;
            if (intVec3_2.IsValid)
            {
                double num2 = num1;
                intVec3_3 = intVec3_1 - intVec3_2;
                double lengthHorizontal = intVec3_3.LengthHorizontal;
                num1 = (float)(num2 - lengthHorizontal);
                intVec3_3 = caster.Position;
                vector3_1 = intVec3_3.ToVector3Shifted() + normalized * num1;
                intVec3_1 = vector3_1.ToIntVec3();
            }

            Vector3 offsetA = normalized * verbProps.beamStartOffset;
            Vector3 vector3_3 = vector3_1 - intVec3_1.ToVector3Shifted();
            if (mote != null)
            {
                mote.UpdateTargets(new TargetInfo(caster.Position, caster.Map),
                    new TargetInfo(intVec3_1, caster.Map), offsetA, vector3_3);
                mote.Maintain();
            }

            if (verbProps.beamGroundFleckDef != null && Rand.Chance(verbProps.beamFleckChancePerTick))
                FleckMaker.Static(vector3_1, caster.Map, verbProps.beamGroundFleckDef);
            if (endEffecter == null && verbProps.beamEndEffecterDef != null)
                endEffecter = verbProps.beamEndEffecterDef.Spawn(intVec3_1, caster.Map, vector3_3);
            if (endEffecter != null)
            {
                endEffecter.offset = vector3_3;
                endEffecter.EffectTick(new TargetInfo(intVec3_1, caster.Map), TargetInfo.Invalid);
                --endEffecter.ticksLeft;
            }

            if (verbProps.beamLineFleckDef != null)
            {
                float num3 = 1f * num1;
                for (int index = 0; index < (double)num3; ++index)
                {
                    if (Rand.Chance(verbProps.beamLineFleckChanceCurve.Evaluate(index / num3)))
                    {
                        Vector3 vector3_4 = index * normalized - normalized * Rand.Value + normalized / 2f;
                        intVec3_3 = caster.Position;
                        FleckMaker.Static(intVec3_3.ToVector3Shifted() + vector3_4, caster.Map,
                            verbProps.beamLineFleckDef);
                    }
                }
            }

            sustainer?.Maintain();
        }

        public override void WarmupComplete()
        {
            burstShotsLeft = ShotsPerBurst;
            state = VerbState.Bursting;
            initialTargetPosition = currentTarget.CenterVector3;
            path.Clear();
            Vector3 vector3_1 = (currentTarget.CenterVector3 - caster.Position.ToVector3Shifted()).Yto0();
            float magnitude = vector3_1.magnitude;
            Vector3 normalized = vector3_1.normalized;
            Vector3 vector3_2 = normalized.RotatedBy(-90f);
            float num1 = verbProps.beamFullWidthRange > 0.0 ? Mathf.Min(magnitude / verbProps.beamFullWidthRange, 1f) : 1f;
            float num2 = (verbProps.beamWidth + 1f) * num1 / ShotsPerBurst;
            Vector3 vector3_3 = currentTarget.CenterVector3.Yto0() - vector3_2 * verbProps.beamWidth / 2f * num1;
            path.Add(vector3_3);
            for (int index = 0; index < ShotsPerBurst; ++index)
            {
                Vector3 vector3_4 = normalized - normalized / 2f;
                Vector3 vector3_5 = Mathf.Sin((float)((index / (double)ShotsPerBurst + 0.5) * 180)) * verbProps.beamCurvature * -normalized - normalized / 2f;
                path.Add(vector3_3 + (vector3_4 - vector3_5) * num1);
                vector3_3 += vector3_2 * num2;
            }
            Vector3 normalized2 = vector3_3 + (normalized - normalized / 2f - Mathf.Sin((float)((ShotsPerBurst / (double)ShotsPerBurst + 0.5) * 180)) * verbProps.beamCurvature * -normalized - normalized / 2f) * num1 + vector3_2 * num2;
            for (int index = 0; index < ShotsPerBurst; ++index)
            {
                Vector3 vector3_4 = normalized2 + normalized2 / 2f;
                Vector3 vector3_5 = Mathf.Sin((float)((index / (double)ShotsPerBurst + 0.5) * 180)) * verbProps.beamCurvature * normalized2 + normalized2 / 2f;
                path.Add(vector3_3 - (vector3_4 + vector3_5) * num1);
                vector3_3 -= vector3_2 * num2;
            }

            if (verbProps.beamMoteDef != null)
                mote = MoteMaker.MakeInteractionOverlay(verbProps.beamMoteDef, caster,
                    new TargetInfo(path[0].ToIntVec3(), caster.Map));
            TryCastNextBurstShot();
            ticksToNextPathStep = verbProps.ticksBetweenBurstShots;
            endEffecter?.Cleanup();
            if (verbProps.soundCastBeam == null) return;
            sustainer = verbProps.soundCastBeam.TrySpawnSustainer(SoundInfo.InMap(caster, MaintenanceType.PerTick));
        }

        private bool CanHit(Thing thing) => thing.Spawned && !CoverUtility.ThingCovered(thing, caster.Map);

        private void HitCell(IntVec3 cell) => ApplyDamage(VerbUtility.ThingsToHit(cell, caster.Map, CanHit).RandomElementWithFallback());

        private void ApplyDamage(Thing thing)
        {
            IntVec3 intVec3_1 = InterpolatedPosition.Yto0().ToIntVec3();
            IntVec3 intVec3_2 = GenSight.LastPointOnLineOfSight(caster.Position, intVec3_1,
                c => c.CanBeSeenOverFast(caster.Map), true);
            if (intVec3_2.IsValid)
                intVec3_1 = intVec3_2;
            Map map = caster.Map;
            if (thing == null || verbProps.beamDamageDef == null)
                return;
            float angleFlat = (currentTarget.Cell - caster.Position).AngleFlat;
            BattleLogEntry_RangedImpact log = new BattleLogEntry_RangedImpact(caster, thing,
                currentTarget.Thing, caster.def, null, null);
            DamageInfo dinfo = new DamageInfo(verbProps.beamDamageDef,
                verbProps.beamDamageDef.defaultDamage, verbProps.beamDamageDef.defaultArmorPenetration,
                angleFlat, caster, weapon: caster.def, intendedTarget: currentTarget.Thing);
            thing.TakeDamage(dinfo).AssociateWithLog(log);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref path, "path", LookMode.Value);
            Scribe_Values.Look(ref ticksToNextPathStep, "ticksToNextPathStep");
            Scribe_Values.Look(ref initialTargetPosition, "initialTargetPosition");
            if (Scribe.mode != LoadSaveMode.PostLoadInit || path != null)
                return;
            path = new List<Vector3>();
        }

    }
}
