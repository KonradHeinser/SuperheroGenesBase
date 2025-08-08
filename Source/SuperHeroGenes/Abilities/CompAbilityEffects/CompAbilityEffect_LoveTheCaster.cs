﻿using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class CompAbilityEffect_LoveTheCaster : CompAbilityEffect
    {
        public new CompProperties_LoveTheCaster Props => (CompProperties_LoveTheCaster)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            Pawn pawn = target.Pawn;
            if (pawn?.health != null && pawn != parent.pawn && pawn.ageTracker.AgeBiologicalYearsFloat > 16f && parent.pawn.ageTracker.AgeBiologicalYearsFloat > 16f)
            {
                Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(Props.hediffToApply);
                if (firstHediffOfDef != null)
                {
                    pawn.health.RemoveHediff(firstHediffOfDef);
                }
                Hediff_LoveTheCaster hediff_LoveTheCaster = (Hediff_LoveTheCaster)HediffMaker.MakeHediff(Props.hediffToApply, pawn, pawn.health.hediffSet.GetBrain());
                hediff_LoveTheCaster.target = parent.pawn;
                if (Props.psychic)
                {
                    HediffComp_Disappears hediffComp_Disappears = hediff_LoveTheCaster.TryGetComp<HediffComp_Disappears>();
                    if (hediffComp_Disappears != null)
                    {
                        float num = parent.def.EffectDuration(parent.pawn);
                        num *= pawn.GetStatValue(StatDefOf.PsychicSensitivity);
                        hediffComp_Disappears.ticksToDisappear = num.SecondsToTicks();
                    }
                }
                pawn.health.AddHediff(hediff_LoveTheCaster);
            }
        }

        public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
        {
            return Valid(target, true) && base.CanApplyOn(target, dest);
        }

        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            if (target.Pawn == null)
                return false;

            if (target.Pawn.ageTracker.AgeBiologicalYearsFloat < 16f)
            {
                if (throwMessages)
                    Messages.Message("CannotUseAbility".Translate(parent.def.label) + ": " + "AbilityCantApplyTooYoung".Translate(target.Pawn), target.Pawn, MessageTypeDefOf.RejectInput, historical: false);
                return false;
            }
            if (parent.pawn.ageTracker.AgeBiologicalYearsFloat < 16f)
            {
                if (throwMessages)
                    Messages.Message("CannotUseAbility".Translate(parent.def.label) + ": " + "AbilityCantApplyTooYoung".Translate(parent.pawn), parent.pawn, MessageTypeDefOf.RejectInput, historical: false);
                return false;
            }
            return base.Valid(target, throwMessages);
        }
    }
}
