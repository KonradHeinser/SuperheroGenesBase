using RimWorld;
using Verse;
using System.Collections.Generic;

namespace SuperHeroGenesBase
{
    public class CompAbilityEffect_AbilityValidator : CompAbilityEffect
    {
        public new CompProperties_AbilityAbilityValidator Props => (CompProperties_AbilityAbilityValidator)props;

        public CompAbilityEffect_AbilityValidator()
        {

        }

        public override bool GizmoDisabled(out string reason)
        {
            if (Props.disableGizmo)
            {
                if (!CheckHour(out string explanation))
                {
                    reason = explanation;
                    return true;
                }
            }
            reason = null;
            return false;
        }

        public bool CheckHour(out string explanation)
        {
            if (Props.minPartOfDay <= 0 && Props.maxPartOfDay >= 1)
            {
                explanation = null;
                return true;
            }
            float time = GenLocalDate.DayPercent(parent.pawn);

            if (time < Props.minPartOfDay || time > Props.maxPartOfDay)
            {
                int minHour = GenDate.HourOfDay((long)(Props.minPartOfDay * 60000), Find.WorldGrid.LongLatOf(parent.pawn.Tile).x);
                int maxHour = GenDate.HourOfDay((long)(Props.maxPartOfDay * 60000), Find.WorldGrid.LongLatOf(parent.pawn.Tile).x);
                explanation = "AbilityTime".Translate(minHour.ToString(), maxHour.ToString(), parent.pawn);
                return false;
            }
            explanation = null;
            return true;
        }
    }
}
