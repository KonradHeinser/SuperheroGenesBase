using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class Thought_Situational_PeopleInColony : Thought_Situational
    {
        public static SimpleCurve PeopleToMoodCurve;

        private static SimpleCurve GetCurve(ThoughtDef def)
        {
            SHGExtension extension = def.GetModExtension<SHGExtension>();
            return extension.peopleToMoodCurve;
        }

        public override float MoodOffset()
        {
            int freeColonistsAndPrisonersSpawnedCount = pawn.Map.mapPawns.FreeColonistsAndPrisonersSpawnedCount;
            PeopleToMoodCurve = GetCurve(def);
            return PeopleToMoodCurve.Evaluate(freeColonistsAndPrisonersSpawnedCount);
        }
    }
}
