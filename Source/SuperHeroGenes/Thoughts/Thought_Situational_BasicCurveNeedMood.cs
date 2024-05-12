using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class Thought_Situational_Bloodthirsty : Thought_Situational
    {
        private static readonly SimpleCurve moodOffsetCurve = new SimpleCurve
        {
            new CurvePoint(0.301f, 0f),
            new CurvePoint(0.3f, -5f),
            new CurvePoint(0f, -30f)
        };

        public override float MoodOffset()
        {
            Need_BloodThirst need_BloodThirst = pawn.needs?.TryGetNeed<Need_BloodThirst>();
            return need_BloodThirst != null ? moodOffsetCurve.Evaluate(need_BloodThirst.CurLevel) : 0f;
        }
    }
}
