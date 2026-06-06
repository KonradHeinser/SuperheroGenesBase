using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class CompAbilityEffect_CreateLinkedHediff : CompAbilityEffect
    {
        public new CompProperties_CreateLinkedHediff Props => (CompProperties_CreateLinkedHediff)props;

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            var targetPawn = target.Pawn;
            var caster = parent.pawn;
            if (targetPawn?.health == null || targetPawn == caster) return;

            if (Props.hediffOnTarget != null)
            {
                var firstHediffOfDef = targetPawn.health.hediffSet.GetFirstHediffOfDef(Props.hediffOnTarget);
                if (firstHediffOfDef != null)
                {
                    targetPawn.health.RemoveHediff(firstHediffOfDef);
                }
                var targetHediff = HediffMaker.MakeHediff(Props.hediffOnTarget, targetPawn, Props.targetHediffOnBrain ? targetPawn.health.hediffSet.GetBrain() : null) as HediffWithTarget;
                if (targetHediff == null)
                {
                    Log.Error("Failed to make a hediff on the target: " + Props.hediffOnTarget.defName);
                    return;
                }
                targetHediff.target = caster;
                if (Props.psychic)
                {
                    var hediffComp_Disappears = targetHediff.TryGetComp<HediffComp_Disappears>();
                    if (hediffComp_Disappears != null)
                    {
                        var num = parent.def.EffectDuration(parent.pawn);
                        num *= targetPawn.GetStatValue(StatDefOf.PsychicSensitivity);
                        num *= caster.GetStatValue(StatDefOf.PsychicSensitivity);
                        hediffComp_Disappears.ticksToDisappear = num.SecondsToTicks();
                    }
                }
                targetPawn.health.AddHediff(targetHediff);
            }

            if (Props.hediffOnCaster != null)
            {

                var firstHediffOfDef = caster.health.hediffSet.GetFirstHediffOfDef(Props.hediffOnCaster);
                if (firstHediffOfDef != null)
                {
                    caster.health.RemoveHediff(firstHediffOfDef);
                }
                var casterHediff = HediffMaker.MakeHediff(Props.hediffOnCaster, caster, Props.casterHediffOnBrain ? caster.health.hediffSet.GetBrain() : null) as HediffWithTarget;
                if (casterHediff == null)
                {
                    Log.Error("Failed to make a hediff on the caster: " + Props.hediffOnCaster.defName);
                    return;
                }
                casterHediff.target = targetPawn;
                if (Props.psychic)
                {
                    var hediffComp_Disappears = casterHediff.TryGetComp<HediffComp_Disappears>();
                    if (hediffComp_Disappears != null)
                    {
                        var num = parent.def.EffectDuration(parent.pawn);
                        num *= caster.GetStatValue(StatDefOf.PsychicSensitivity);
                        num *= targetPawn.GetStatValue(StatDefOf.PsychicSensitivity);
                        hediffComp_Disappears.ticksToDisappear = num.SecondsToTicks();
                    }
                }
                caster.health.AddHediff(casterHediff);
            }
        }
    }
}