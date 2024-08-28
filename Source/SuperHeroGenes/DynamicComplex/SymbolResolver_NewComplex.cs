using Verse;
using Verse.AI;
using RimWorld.BaseGen;
using RimWorld;
using System.Collections.Generic;

namespace SuperHeroGenesBase
{
    public class SymbolResolver_NewComplex : SymbolResolver
    {
        private static List<IntVec3> doorCells = new List<IntVec3>();

        public List<ComplexSet> filthSet;

        public LayoutDef defaultLayout;

        public PawnKindDef dessicatedCorpsePawnKind; // Currently Ideology only

        public IntRange corpseRandomAgeRange;

        public TerrainDef outdoorPathTerrainDef;

        public TerrainDef defaultFlooring;

        // Replacement for the defenses symbol

        public List<ThingDef> firstLayerThingOptions;

        public float firstLayerChancePerTile = 0.8f;

        public ThingDef firstLayerFilth;

        public float firstLayerFilthChance;

        public List<ThingDef> secondLayerThingOptions;

        public float secondLayerChancePerTile = 0.6f;

        public ThingDef secondLayerFilth;

        public float secondLayerFilthChance;

        public List<ThingDef> thirdLayerThingOptions;

        public float thirdLayerChancePerTile = 0.05f;

        public ThingDef thirdLayerFilth;

        public float thirdLayerFilthChance;

        public List<ThingDef> fourthLayerThingOptions;

        public float fourthLayerChancePerTile = 0.015f;

        public ThingDef fourthLayerFilth;

        public float fourthLayerFilthChance;
        // 

        public override void Resolve(ResolveParams rp)
        {
            ResolveParams resolveParams = rp;

            if (!filthSet.NullOrEmpty())
                foreach (ComplexSet set in filthSet)
                {
                    resolveParams.ignoreDoorways = set.ignoreDoorways;
                    resolveParams.filthDensity = set.density;
                    resolveParams.filthDef = set.thing;
                    BaseGen.symbolStack.Push("filth", resolveParams);
                }

            if (dessicatedCorpsePawnKind != null)
            {
                ResolveParams resolveParams2 = rp;
                resolveParams2.desiccatedCorpsePawnKind = dessicatedCorpsePawnKind;
                resolveParams2.desiccatedCorpseRandomAgeRange = corpseRandomAgeRange;
                BaseGen.symbolStack.Push("desiccatedCorpses", resolveParams2);
            }

            if (outdoorPathTerrainDef != null)
            {
                ResolveParams resolveParams3 = rp;
                resolveParams3.floorDef = outdoorPathTerrainDef;
                BaseGen.symbolStack.Push("outdoorsPath", resolveParams3);
            }

            if (!firstLayerThingOptions.NullOrEmpty() || !secondLayerThingOptions.NullOrEmpty() || !thirdLayerThingOptions.NullOrEmpty() || !fourthLayerThingOptions.NullOrEmpty())
                CreateDefenses(rp);

            BaseGen.symbolStack.Push("ensureCanReachMapEdge", rp);
            ResolveComplex(rp);

            // Not sure what this is, but it's from the ancient complex stuff and removing it might break some stuff
            ResolveParams resolveParams4 = rp;
            resolveParams4.rect = rp.rect.ExpandedBy(5);
            resolveParams4.floorDef = TerrainDefOf.Gravel;
            resolveParams4.chanceToSkipFloor = 0.05f;
            resolveParams4.floorOnlyIfTerrainSupports = true;
            BaseGen.symbolStack.Push("floor", resolveParams4);

            foreach (IntVec3 item in resolveParams4.rect)
            {
                Building edifice = item.GetEdifice(BaseGen.globalSettings.map);
                if (edifice != null && edifice.def.destroyable && edifice.def.IsBuildingArtificial)
                {
                    edifice.Destroy();
                }
            }
        }

        private void CreateDefenses(ResolveParams rp)
        {
            doorCells.Clear();
            Map map = BaseGen.globalSettings.map;
            foreach (IntVec3 item in rp.rect)
            {
                Building_Door door = item.GetDoor(BaseGen.globalSettings.map);
                if (door != null && map.reachability.CanReachMapEdge(item, TraverseParms.For(TraverseMode.NoPassClosedDoors)))
                {
                    doorCells.Add(door.Position);
                }
            }
            Thing placedThing;

            if (!firstLayerThingOptions.NullOrEmpty())
                foreach (IntVec3 item2 in rp.rect.ExpandedBy(5).EdgeCells.InRandomOrder())
                    if (Rand.Chance(firstLayerChancePerTile))
                    {
                        TryPlaceThing(firstLayerThingOptions.RandomElement(), item2, out placedThing);

                        if (firstLayerFilth != null)
                            ScatterDebrisUtility.ScatterFilthAroundThing(placedThing, map, firstLayerFilth, firstLayerFilthChance, 0);
                    }

            if (!secondLayerThingOptions.NullOrEmpty())
                foreach (IntVec3 item2 in rp.rect.ExpandedBy(7).EdgeCells.InRandomOrder())
                    if (Rand.Chance(secondLayerChancePerTile))
                    {
                        TryPlaceThing(secondLayerThingOptions.RandomElement(), item2, out placedThing);

                        if (secondLayerFilth != null)
                            ScatterDebrisUtility.ScatterFilthAroundThing(placedThing, map, secondLayerFilth, secondLayerFilthChance, 0);
                    }

            if (!thirdLayerThingOptions.NullOrEmpty())
                foreach (IntVec3 item2 in rp.rect.ExpandedBy(10).EdgeCells.InRandomOrder())
                    if (Rand.Chance(thirdLayerChancePerTile))
                    {
                        TryPlaceThing(thirdLayerThingOptions.RandomElement(), item2, out placedThing);

                        if (thirdLayerFilth != null)
                            ScatterDebrisUtility.ScatterFilthAroundThing(placedThing, map, thirdLayerFilth, thirdLayerFilthChance, 0);
                    }

            if (!fourthLayerThingOptions.NullOrEmpty())
                foreach (IntVec3 item2 in rp.rect.ExpandedBy(14).EdgeCells.InRandomOrder())
                    if (Rand.Chance(fourthLayerChancePerTile))
                    {
                        TryPlaceThing(fourthLayerThingOptions.RandomElement(), item2, out placedThing);

                        if (fourthLayerFilth != null)
                            ScatterDebrisUtility.ScatterFilthAroundThing(placedThing, map, fourthLayerFilth, fourthLayerFilthChance, 0);
                    }

            doorCells.Clear();
        }

        private bool CanReachEntrance(IntVec3 cell, List<IntVec3> entrancePositions)
        {
            Map map = BaseGen.globalSettings.map;
            for (int i = 0; i < entrancePositions.Count; i++)
            {
                if (map.reachability.CanReach(cell, entrancePositions[i], PathEndMode.OnCell, TraverseParms.For(TraverseMode.PassDoors)))
                {
                    return true;
                }
            }
            return false;
        }

        private bool TryPlaceThing(ThingDef thingDef, IntVec3 position, out Thing placedThing, Rot4? rot = null)
        {
            Map map = BaseGen.globalSettings.map;
            CellRect rect = GenAdj.OccupiedRect(position, rot ?? thingDef.defaultPlacingRot, thingDef.size);
            if (!rect.InBounds(map))
            {
                placedThing = null;
                return false;
            }
            if (!GenConstruct.TerrainCanSupport(rect, map, thingDef))
            {
                placedThing = null;
                return false;
            }
            foreach (IntVec3 item in rect)
            {
                if (item.Roofed(map))
                {
                    placedThing = null;
                    return false;
                }
                List<Thing> thingList = item.GetThingList(map);
                for (int i = 0; i < thingList.Count; i++)
                {
                    if (thingList[i].def.category != ThingCategory.Plant)
                    {
                        placedThing = null;
                        return false;
                    }
                }
            }
            placedThing = GenSpawn.Spawn(ThingMaker.MakeThing(thingDef), position, map, rot ?? thingDef.defaultPlacingRot);
            return true;
        }

        public void ResolveComplex(ResolveParams rp)
        {
            if (rp.ancientLayoutStructureSketch == null)
            {
                StructureGenParams parms = new StructureGenParams
                {
                    size = new IntVec2(rp.rect.Width, rp.rect.Height)
                };
                if (defaultLayout != null)
                    rp.ancientLayoutStructureSketch = defaultLayout.Worker.GenerateStructureSketch(parms);
                else
                    rp.ancientLayoutStructureSketch = LayoutDefOf.AncientComplex.Worker.GenerateStructureSketch(parms);
            }
            ResolveParams resolveParams = rp;
            resolveParams.ancientLayoutStructureSketch = rp.ancientLayoutStructureSketch;
            BaseGen.symbolStack.Push("ancientComplexSketch", resolveParams);
            ResolveParams resolveParams2 = rp;
            if (defaultFlooring != null)
                resolveParams2.floorDef = defaultFlooring;
            else
                resolveParams2.floorDef = TerrainDefOf.Concrete;
            resolveParams2.allowBridgeOnAnyImpassableTerrain = true;
            resolveParams2.floorOnlyIfTerrainSupports = false;
            foreach (LayoutRoom room in rp.ancientLayoutStructureSketch.structureLayout.Rooms)
            {
                foreach (CellRect rect in room.rects)
                {
                    resolveParams2.rect = rect.MovedBy(rp.rect.Min);
                    BaseGen.symbolStack.Push("floor", resolveParams2);
                    BaseGen.symbolStack.Push("clear", resolveParams2);
                }
            }
        }
    }
}
