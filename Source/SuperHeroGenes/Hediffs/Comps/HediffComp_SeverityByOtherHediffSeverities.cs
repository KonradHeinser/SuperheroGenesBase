using RimWorld;
using Verse;
using UnityEngine;
using System;

namespace SuperHeroGenesBase
{
    public class HediffComp_SeverityByOtherHediffSeverities : HediffComp_SetterBase
    {
        public HediffCompProperties_SeverityByOtherHediffSeverities Props => (HediffCompProperties_SeverityByOtherHediffSeverities)props;

        protected override void SetSeverity()
        {
            base.SetSeverity();
            float newSeverity = Props.baseSeverity;

            if (!Props.hediffSets.NullOrEmpty())
            {
                foreach (HediffSeverityFactor hediffSet in Props.hediffSets)
                {
                    Hediff hediff = Pawn.health.hediffSet.GetFirstHediffOfDef(hediffSet.hediff);
                    if (hediff != null)
                    {
                        float add = hediff.Severity * hediffSet.factor;
                        if (hediffSet.factor < 0) newSeverity += Math.Min(add, hediffSet.minResult);
                        else newSeverity += Math.Max(add, hediffSet.minResult);
                    }
                    else newSeverity += hediffSet.minResult;
                }
            }

            parent.Severity = newSeverity;
            ticksToNextCheck = 120;
        }
    }
}
