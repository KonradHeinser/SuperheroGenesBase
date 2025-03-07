using System;
using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class ThoughtWorker_HediffSeverity : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            SHGExtension thoughtExtension = def.GetModExtension<SHGExtension>();

            if (thoughtExtension?.hediff != null && thoughtExtension.curve != null &&
                p.HasHediff(thoughtExtension.hediff, out var hediff))
            {
                return Math.Abs(thoughtExtension.curve.Evaluate(hediff.Severity)) >= 1;
            }

            return ThoughtState.Inactive;
        }

        public override string PostProcessLabel(Pawn p, string label)
        {
            SHGExtension thoughtExtension = def.GetModExtension<SHGExtension>();
            if (p.health.hediffSet.TryGetHediff(thoughtExtension.hediff, out var hediff))
            {
                return label.Formatted(p.Named("PAWN"), hediff.Label.Named("HEDIFF"));
            }
            return base.PostProcessLabel(p, label);
        }

        public override string PostProcessDescription(Pawn p, string description)
        {
            SHGExtension thoughtExtension = def.GetModExtension<SHGExtension>();
            if (p.health.hediffSet.TryGetHediff(thoughtExtension.hediff, out var hediff))
            {
                return description.Formatted(p.Named("PAWN"), hediff.Label.Named("HEDIFF"));
            }
            return base.PostProcessDescription(p, description);
        }
    }
}
