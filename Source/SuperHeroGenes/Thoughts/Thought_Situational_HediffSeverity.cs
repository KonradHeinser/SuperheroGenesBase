using RimWorld;

namespace SuperHeroGenesBase
{
    public class Thought_Situational_HediffSeverity : Thought_Situational
    {
        public override float MoodOffset()
        {
            SHGExtension thoughtExtension = def.GetModExtension<SHGExtension>();

            if (thoughtExtension?.hediff != null && thoughtExtension.curve != null)
            {
                if (pawn.HasHediff(thoughtExtension.hediff, out var hediff))
                    return thoughtExtension.curve.Evaluate(hediff.Severity);
            }

            return base.MoodOffset();
        }
    }
}
