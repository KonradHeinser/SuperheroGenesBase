using Verse;
using RimWorld;

namespace SuperHeroGenesBase
{
    public class ThoughtWorker_NearWater : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (def.stages.Count < 2 || p.MapHeld == null) return ThoughtState.Inactive;

            SHGExtension extension = def.GetModExtension<SHGExtension>();

            if (extension == null)
            {
                if (SHGUtilities.CheckNearbyWater(p, 1, out int waterCount)) return GetThoughtState(1);
                return GetThoughtState(0);
            }
            if (!SHGUtilities.CheckNearbyWater(p, 1, out int count, extension.maxWaterDistance)) return GetThoughtState(0);
            return GetThoughtState(0);
        }

        public ThoughtState GetThoughtState(int stage)
        {
            if (def.stages[stage].baseMoodEffect != 0) return ThoughtState.ActiveAtStage(stage);
            return ThoughtState.Inactive;
        }
    }
}
