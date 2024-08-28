using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld.SketchGen;
using RimWorld;

namespace SuperHeroGenesBase
{
    public class SketchResolver_FlexibleRoom : SketchResolver
    {
        private static List<IntVec3> tmpCells = new List<IntVec3>();

        public ThingDef bedType;

        public ThingDef cornerThing;

        public ThingDef wallEdgeThing;

        public ThingDef centerThing;

        protected override bool CanResolveInt(ResolveParams parms)
        {
            if (parms.rect.HasValue)
            {
                return parms.sketch != null;
            }
            return false;
        }

        protected override void ResolveInt(ResolveParams parms)
        {
            CellRect rect = parms.rect.Value;

            if (cornerThing != null)
            {
                ResolveParams parms2 = parms;
                parms2.cornerThing = cornerThing;
                parms2.requireFloor = true;
                SketchResolverDefOf.AddCornerThings.Resolve(parms2);
            }

            CellRect cellRect = SketchGenUtility.FindBiggestRect(parms.sketch, (IntVec3 p) => rect.Contains(p) && !parms.sketch.ThingsAt(p).Any());
            if (cellRect == CellRect.Empty)
            {
                return;
            }

            if (wallEdgeThing != null)
            {
                ResolveParams parms3 = parms;
                parms3.chance = 1f;
                parms3.wallEdgeThing = wallEdgeThing;
                SketchResolverDefOf.AddWallEdgeThings.Resolve(parms3);
            }

            if (centerThing != null)
            {
                ResolveParams parms4 = parms;
                parms4.thingCentral = centerThing;
                parms4.requireFloor = true;
                SketchResolverDefOf.AddThingsCentral.Resolve(parms4);
            }

            tmpCells.Clear();

            if (bedType != null)
            {
                if (cellRect.Width > cellRect.Height)
                {
                    tmpCells.AddRange(cellRect.GetEdgeCells(Rot4.North));
                    tmpCells.AddRange(cellRect.GetEdgeCells(Rot4.South));
                    foreach (IntVec3 tmpCell in tmpCells)
                    {
                        if (CanPlaceBedAt(bedType, tmpCell, Rot4.North, parms.sketch))
                        {
                            parms.sketch.AddThing(bedType, tmpCell, Rot4.North, null, 1, null, null, false);
                        }
                        if (CanPlaceBedAt(bedType, tmpCell, Rot4.South, parms.sketch))
                        {
                            parms.sketch.AddThing(bedType, tmpCell, Rot4.South, null, 1, null, null, false);
                        }
                    }
                }
                else
                {
                    tmpCells.AddRange(cellRect.GetEdgeCells(Rot4.East));
                    tmpCells.AddRange(cellRect.GetEdgeCells(Rot4.West));
                    foreach (IntVec3 tmpCell2 in tmpCells)
                    {
                        if (CanPlaceBedAt(bedType, tmpCell2, Rot4.East, parms.sketch))
                        {
                            parms.sketch.AddThing(bedType, tmpCell2, Rot4.East, null, 1, null, null, false);
                        }
                        if (CanPlaceBedAt(bedType, tmpCell2, Rot4.West, parms.sketch))
                        {
                            parms.sketch.AddThing(bedType, tmpCell2, Rot4.West, null, 1, null, null, false);
                        }
                    }
                }
                tmpCells.Clear();
            }
        }

        private bool CanPlaceBedAt(ThingDef def, IntVec3 position, Rot4 rot, Sketch sketch)
        {
            CellRect cellRect = GenAdj.OccupiedRect(position, rot, def.size);
            foreach (IntVec3 cell in cellRect.Cells)
            {
                if (sketch.ThingsAt(cell).Any((SketchThing x) => x.def == ThingDefOf.Wall))
                {
                    return false;
                }
            }
            bool result = false;
            foreach (IntVec3 edgeCell in cellRect.ExpandedBy(1).EdgeCells)
            {
                foreach (SketchThing item in sketch.ThingsAt(edgeCell))
                {
                    if (item.def == ThingDefOf.Wall)
                    {
                        result = true;
                        continue;
                    }
                    return false;
                }
            }
            return result;
        }
    }
}
