using Verse;
using Verse.AI;

namespace SuperHeroGenesBase
{
    public class MentalBreakWorker_Hydrophobia : MentalBreakWorker
    {
        public override bool BreakCanOccur(Pawn pawn)
        {
            if (!pawn.Spawned || !base.BreakCanOccur(pawn)) return false;

            SHGExtension extension = def.GetModExtension<SHGExtension>();

            if (extension == null)
                return SHGUtilities.CheckNearbyWater(pawn, 1, out _);

            return SHGUtilities.CheckNearbyWater(pawn, 1, out _, extension.radius);
        }
    }
}
