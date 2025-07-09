using RimWorld;
using RimWorld.BaseGen;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace SuperHeroGenesBase
{
    public class GenStep_ComplexLayout : GenStep_LargeRuins
    {
        private LayoutStructureSketch structureSketch;

        private static readonly IntVec2 DefaultComplexSize = new IntVec2(80, 80);

        public RuleDef ruleDef;

        public override int SeedPart => 235635649;

        protected override IntRange RuinsMinMaxRange => IntRange.One;

        protected override LayoutDef LayoutDef => null;

        private Faction faction = null;

        private bool checkedFaction = false;

        protected override IntVec2 MinSize => GetMinSize();

        private IntVec2 GetMinSize()
        {
            if (ruleDef.resolvers?.NullOrEmpty() == false)
                foreach (var resolver in ruleDef.resolvers)
                    if (resolver is SymbolResolver_NewComplex complex)
                        return complex.minRectSize;
            return DefaultComplexSize;
        }

        protected override IntVec2 MaxSize => GetMaxSize();

        private IntVec2 GetMaxSize()
        {
            if (ruleDef.resolvers?.NullOrEmpty() == false)
                foreach (var resolver in ruleDef.resolvers)
                    if (resolver is SymbolResolver_NewComplex complex)
                        return new IntVec2(Mathf.CeilToInt(complex.minRectSize.x + complex.defendRadius), Mathf.CeilToInt(complex.minRectSize.z + complex.defendRadius));
            return new IntVec2(100, 100);
        }

        protected override Faction Faction
        {
            get
            {
                if (faction == null && !checkedFaction)
                {
                    checkedFaction = true;
                    foreach (SymbolResolver resolver in ruleDef.resolvers)
                        if (resolver is SymbolResolver_NewComplex complexResolver 
                            && complexResolver.defaultLayout is ComplexLayoutDef complexLayout)
                        {
                            faction = Find.FactionManager.FirstFactionOfDef(complexLayout.threats[0].def.faction);
                            break;
                        }
                }
                return faction;
            }
        }

        private GenStepParams currentParams;

        public float externalPointsMultiplier = 1;

        public float internalPointsMultiplier = 1;

        public override void Generate(Map map, GenStepParams parms)
        {
            currentParams = parms;
            structureSketch = parms.sitePart.parms.ancientLayoutStructureSketch;
            if (structureSketch?.structureLayout == null)
                TryRecoverEmptySketch(parms);
            map.layoutStructureSketches.Add(structureSketch);
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
                            size = newComplex.minRectSize,
                        };
                        structureSketch = newComplex.defaultLayout.Worker.GenerateStructureSketch(parms2);
                        MapGenUtility.TryGetLargestClearRect(out var rect);
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
                Log.Warning("Failed to recover complex from anything. Generating an ancient complex.");
            }
        }

        protected override LayoutStructureSketch GenerateAndSpawn(CellRect rect, Map map, GenStepParams parms, LayoutDef layout)
        {
            ResolveParams resolveParams = default;
            resolveParams.ancientLayoutStructureSketch = structureSketch;

            SitePartParams parms2 = parms.sitePart.parms;
            resolveParams.threatPoints = parms2.threatPoints;
            resolveParams.interiorThreatPoints = ((parms2.interiorThreatPoints > 0f) ? new float?(parms2.interiorThreatPoints) : null) * internalPointsMultiplier;
            resolveParams.exteriorThreatPoints = ((parms2.exteriorThreatPoints > 0f) ? new float?(parms2.exteriorThreatPoints) : null) * externalPointsMultiplier;
            resolveParams.rect = structureSketch.structureLayout.container;

            if (structureSketch.layoutDef is ComplexLayoutDef complex)
                resolveParams.thingSetMakerDef = complex.rewardThingSetMakerDef;
            else
                resolveParams.thingSetMakerDef = parms.sitePart.parms.ancientComplexRewardMaker;

            ResolveParams parms3 = resolveParams;
            FormCaravanComp component = parms.sitePart.site.GetComponent<FormCaravanComp>();
            if (component != null)
                component.foggedRoomsCheckRect = parms3.rect;

            BaseGen.globalSettings.map = map;
            SitePart sitePart = currentParams.sitePart;

            if (sitePart.conditionCauser != null)
            {
                CellRect cellRect = CellRect.CenteredOn(resolveParams.rect.CenterCell, 10, 10).ClipInsideMap(map);
                sitePart.conditionCauserWasSpawned = true;
                ResolveParams ccSketchResolveParams = default;
                ccSketchResolveParams.rect = cellRect;
                ccSketchResolveParams.faction = map.ParentFaction;
                ccSketchResolveParams.conditionCauser = sitePart.conditionCauser;
                BaseGen.symbolStack.Push("conditionCauserRoom", ccSketchResolveParams);
            }

            BaseGen.symbolStack.Push(ruleDef.symbol, parms3);
            MapGenerator.UsedRects.Add(resolveParams.rect);
            BaseGen.Generate();

            return structureSketch;
        }
    }
}
