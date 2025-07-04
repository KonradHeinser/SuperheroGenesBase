using System.Linq;
using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class CompAbilityEffect_ToggleHediffExistence : CompAbilityEffect
    {
        public new CompProperties_AbilityToggleHediffExistence Props => (CompProperties_AbilityToggleHediffExistence)props;

        public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
        {
            return target.Pawn?.health?.hediffSet != null;
        }

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            if (target.Pawn.GetHediffFromParts(Props.hediff.hediff, Props.hediff.bodyParts).Any())
                SHGUtilities.RemoveHediffs(target.Pawn, Props.hediff.hediff);
            else
                SHGUtilities.AddHediffsToParts(target.Pawn, null, Props.hediff);
        }
    }
}
