﻿using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class HediffComp_SeverityByLightLevel : HediffComp
    {
        public HediffCompProperties_SeverityByLightLevel Props => (HediffCompProperties_SeverityByLightLevel)props;

        public override void CompPostTick(ref float severityAdjustment)
        {
            if (!parent.pawn.IsHashIntervalTick(200)) return;

            if (Pawn.Map != null)
                parent.Severity = Props.lightToSeverityCurve.Evaluate(Pawn.Map.glowGrid.GroundGlowAt(Pawn.Position));
            else if (Props.timeToSeverityCurve != null)
                parent.Severity = Props.timeToSeverityCurve.Evaluate(GenLocalDate.DayPercent(Pawn));
        }
    }
}
