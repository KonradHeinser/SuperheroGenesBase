using System.Collections.Generic;
using RimWorld.BaseGen;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using RimWorld;

namespace SuperHeroGenesBase
{
    public class GenStep_ComplexLayout : GenStep_ScattererBestFit
    {
        private LayoutStructureSketch structureSketch;

        private static readonly IntVec2 DefaultComplexSize = new IntVec2(80, 80);

        public RuleDef ruleDef;

        public override int SeedPart => 235635649;

        protected override IntVec2 Size => new IntVec2(structureSketch.structureLayout.container.Width + 10, structureSketch.structureLayout.container.Height + 10);

        private GenStepParams currentParams;

        public float externalPointsMultiplier = 1;

        public float internalPointsMultiplier = 1;

        public override bool CollisionAt(IntVec3 cell, Map map)
        {
            List<Thing> thingList = cell.GetThingList(map);
            for (int i = 0; i < thingList.Count; i++)
            {
                if (thingList[i].def.IsBuildingArtificial)
                {
                    return true;
                }
            }
            return false;
        }

        public override void Generate(Map map, GenStepParams parms)
        {
            count = 1;
            nearMapCenter = true;
            currentParams = parms;
            structureSketch = parms.sitePart.parms.ancientLayoutStructureSketch;
            if (structureSketch?.structureLayout == null)
            {
                TryRecoverEmptySketch(parms);
            }
            base.Generate(map, parms);
        }

        private void TryRecoverEmptySketch(GenStepParams parms)
        {
            bool flag = false;

            if (!ruleDef.resolvers.NullOrEmpty())
                foreach (SymbolResolver resolver in ruleDef.resolvers)
                    if (resolver is SymbolResolver_NewComplex newComplex && newComplex.defaultLayout != null)
                    {
                        StructureGenParams parms2 = new StructureGenParams
                        {
                            size = newComplex.minRectSize
                        };
                        structureSketch = newComplex.defaultLayout.Worker.GenerateStructureSketch(parms2);
                        flag = true;
                        break;
                    }

            if (!flag)
            {
                StructureGenParams parms2 = new StructureGenParams
                {
                    size = DefaultComplexSize
                };
                structureSketch = LayoutDefOf.AncientComplex.Worker.GenerateStructureSketch(parms2);
                Log.Warning("Failed to recover lost complex from any quest. Generating an ancient complex.");
            }
        }

        protected override void ScatterAt(IntVec3 c, Map map, GenStepParams parms, int stackCount = 1)
        {
            ResolveParams resolveParams = default;
            resolveParams.ancientLayoutStructureSketch = structureSketch;

            SitePartParams parms2 = parms.sitePart.parms;
            resolveParams.threatPoints = parms2.threatPoints;
            resolveParams.interiorThreatPoints = ((parms2.interiorThreatPoints > 0f) ? new float?(parms2.interiorThreatPoints) : null) * internalPointsMultiplier;
            resolveParams.exteriorThreatPoints = ((parms2.exteriorThreatPoints > 0f) ? new float?(parms2.exteriorThreatPoints) : null) * externalPointsMultiplier;
            resolveParams.rect = CellRect.CenteredOn(c, structureSketch.structureLayout.container.Width, structureSketch.structureLayout.container.Height);

            if (structureSketch.layoutDef is ComplexLayoutDef complex)
                resolveParams.thingSetMakerDef = complex.rewardThingSetMakerDef;
            else
                resolveParams.thingSetMakerDef = parms.sitePart.parms.ancientComplexRewardMaker;

            ResolveParams parms3 = resolveParams;
            FormCaravanComp component = parms.sitePart.site.GetComponent<FormCaravanComp>();
            if (component != null)
            {
                component.foggedRoomsCheckRect = parms3.rect;
            }

            BaseGen.globalSettings.map = map;
            SitePart sitePart = currentParams.sitePart;

            if (sitePart.conditionCauser != null)
            {
                CellRect cellRect = CellRect.CenteredOn(c, 10, 10).ClipInsideMap(map);
                sitePart.conditionCauserWasSpawned = true;
                ResolveParams ccSketchResolveParams = default(ResolveParams);
                ccSketchResolveParams.rect = cellRect;
                ccSketchResolveParams.faction = map.ParentFaction;
                ccSketchResolveParams.conditionCauser = sitePart.conditionCauser;
                BaseGen.symbolStack.Push("conditionCauserRoom", ccSketchResolveParams);
            }

            BaseGen.symbolStack.Push(ruleDef.symbol, parms3);
            BaseGen.Generate();
        }
    }
}
