using Verse;
using RimWorld;

namespace SuperHeroGenesBase
{
    public class ThoughtWorker_NearFire : ThoughtWorker
    {
        public SHGExtension cachedExtension = null;
        public bool alreadyCheckedExtension = false;

        public SHGExtension Extension
        {
            get
            {
                if (!alreadyCheckedExtension)
                {
                    alreadyCheckedExtension = true;
                    if (def.HasModExtension<SHGExtension>())
                        cachedExtension = def.GetModExtension<SHGExtension>();
                }

                return cachedExtension;
            }
        }

        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (!ModsConfig.BiotechActive || p.genes == null || !NearFire(p))
                return ThoughtState.Inactive;

            return ThoughtState.ActiveAtStage(0);
        }

        public bool NearFire(Pawn pawn)
        {
            float radius = Extension != null ? Extension.radius : 19.9f;

            Map mapHeld = pawn.MapHeld;
            if (mapHeld == null)
                return false;

            IntVec3 positionHeld = pawn.PositionHeld;
            int num = GenRadial.NumCellsInRadius(radius);
            for (int i = 0; i < num; i++)
            {
                IntVec3 intVec = pawn.Position + GenRadial.RadialPattern[i];
                if (intVec.InBounds(mapHeld) && !intVec.Fogged(mapHeld) && GenSight.LineOfSight(positionHeld, intVec, mapHeld, true) && intVec.ContainsStaticFire(mapHeld))
                    return true;
            }
            return false;
        }
    }
}
