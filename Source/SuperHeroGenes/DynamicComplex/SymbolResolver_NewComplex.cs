﻿using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.BaseGen;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace SuperHeroGenesBase
{
    public class SymbolResolver_NewComplex : SymbolResolver_AncientComplex_Base
    {
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

        public float defendRadius = 50;

        protected override LayoutDef DefaultLayoutDef => defaultLayout;

        public override void Resolve(ResolveParams rp)
        {
            ResolveParams resolveParams = rp;

            Map map = BaseGen.globalSettings.map;
            foreach (IntVec3 item in resolveParams.rect.ExpandedBy(5))
            {
                Building edifice = item.GetEdificeSafe(map);
                if (edifice?.def.destroyable == true)
                    edifice.Destroy(DestroyMode.QuestLogic);
                List<Thing> things = new List<Thing>(item.GetThingList(map));
                if (!things.NullOrEmpty())
                    foreach (Thing thing in things)
                        thing.Destroy(DestroyMode.QuestLogic);
            }

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
                resolveParams3.pathwayFloorDef = outdoorPathTerrainDef;
                BaseGen.symbolStack.Push("outdoorsPath", resolveParams3);
            }

            BaseGen.symbolStack.Push("ensureCanReachMapEdge", rp);
            if (rp.faction == null && defaultLayout is ComplexLayoutDef cLayout && cLayout.Worker is LayoutWorkerComplex cWorker && cWorker.GetFixedHostileFactionForThreats() != null)
            {
                rp.faction = cWorker.GetFixedHostileFactionForThreats();
                if (cLayout.rewardThingSetMakerDef != null && cLayout.roomRewardCrateFactor <= 0)
                {
                    ResolveParams resolveParams5 = rp;
                    resolveParams5.thingSetMakerDef = resolveParams5.thingSetMakerDef ?? ThingSetMakerDefOf.MapGen_DefaultStockpile;
                    resolveParams5.lootMarketValue = resolveParams5.lootMarketValue ?? 1800f;
                    BaseGen.symbolStack.Push("lootScatter", resolveParams5);
                }
            }

            ResolveComplex(rp);

            ResolveParams resolveParams4 = rp;
            resolveParams4.rect = rp.rect.ExpandedBy(5);
            resolveParams4.floorDef = defaultFlooring ?? TerrainDefOf.Gravel;
            resolveParams4.chanceToSkipFloor = 0;
            resolveParams4.floorOnlyIfTerrainSupports = true;
            BaseGen.symbolStack.Push("floor", resolveParams4);

            if (defaultLayout is ComplexLayoutDef complexLayout && complexLayout.Worker is LayoutWorkerComplex complexWorker && complexWorker.GetFixedHostileFactionForThreats() != null)
            {
                Faction faction = complexWorker.GetFixedHostileFactionForThreats();

                if (faction == Faction.OfMechanoids)
                {
                    Lord lord = LordMaker.MakeNewLord(Faction.OfMechanoids, new LordJob_SleepThenMechanoidsDefend(), BaseGen.globalSettings.map);

                    if (rp.exteriorThreatPoints.HasValue)
                    {
                        ResolveParams resolveParams2 = rp;
                        resolveParams2.sleepingMechanoidsWakeupSignalTag = rp.triggerSecuritySignal;
                        resolveParams2.sendWokenUpMessage = false;
                        resolveParams2.rect = rp.rect.ExpandedBy(10);
                        resolveParams2.threatPoints = rp.exteriorThreatPoints.Value;
                        BaseGen.symbolStack.Push("sleepingMechanoids", resolveParams2);
                    }

                    if (rp.interiorThreatPoints.HasValue)
                    {
                        Mathf.Min(rp.rect.Width, rp.rect.Height);

                        foreach (PawnKindDef combatPawnKindsForPoint in PawnUtility.GetCombatPawnKindsForPoints(MechClusterGenerator.MechKindSuitableForCluster, rp.interiorThreatPoints.Value, (PawnKindDef pk) => 1f / pk.combatPower))
                        {
                            ResolveParams resolveParams3 = rp;
                            resolveParams3.singlePawnKindDef = combatPawnKindsForPoint;
                            resolveParams3.singlePawnLord = lord;
                            BaseGen.symbolStack.Push("pawn", resolveParams3);
                        }
                    }
                }
                else
                {
                    // These ones will always be defenders
                    if (rp.exteriorThreatPoints.HasValue)
                    {
                        Lord lord = (rp.settlementLord = rp.singlePawnLord ?? LordMaker.MakeNewLord(faction, new LordJob_DefendPoint(rp.rect.CenterCell, defendRadius), map));
                        ResolveParams resolveParams2 = rp;
                        TraverseParms traverseParms = TraverseParms.For(TraverseMode.PassDoors);
                        resolveParams2.pawnGroupKindDef = rp.pawnGroupKindDef ?? PawnGroupKindDefOf.Combat;
                        resolveParams2.singlePawnLord = lord;
                        resolveParams2.singlePawnSpawnCellExtraPredicate = rp.singlePawnSpawnCellExtraPredicate ?? ((IntVec3 x) => map.reachability.CanReachMapEdge(x, traverseParms));
                        resolveParams2.rect = rp.rect.ExpandedBy(10);
                        resolveParams2.spawnNear = rp.rect.CenterCell;
                        resolveParams2.pawnGroupMakerParams = new PawnGroupMakerParms
                        {
                            tile = map.Tile,
                            faction = faction,
                            points = (float)rp.exteriorThreatPoints,
                            inhabitants = true,
                        };

                        BaseGen.symbolStack.Push("pawnGroup", resolveParams2);
                    }

                    // These ones will attack the player after a while. Will usually spawn outside the complex then run to the center to prepare. The spawning outside is a bug I haven't figured out yet
                    if (rp.interiorThreatPoints.HasValue)
                    {
                        Lord lord = (rp.settlementLord = rp.singlePawnLord ?? LordMaker.MakeNewLord(faction, new LordJob_DefendBase(faction, rp.rect.CenterCell, 2500, rp.attackWhenPlayerBecameEnemy ?? false), map));
                        List<PawnKindDef> validPawnKinds = new List<PawnKindDef>();
                        foreach (PawnGroupMaker item in faction.def.pawnGroupMakers.Where((PawnGroupMaker x) => x.kindDef == PawnGroupKindDefOf.Combat))
                            foreach (PawnGenOption option in item.options)
                                if (!validPawnKinds.Contains(option.kind))
                                    validPawnKinds.Add(option.kind);

                        foreach (PawnKindDef combatPawnKindsForPoint in PawnUtility.GetCombatPawnKindsForPoints(validPawnKinds.Contains, rp.interiorThreatPoints.Value, (PawnKindDef pk) => 1f / pk.combatPower))
                        {
                            ResolveParams resolveParams3 = rp;
                            resolveParams3.rect = rp.rect.ContractedBy(3);
                            resolveParams3.singlePawnKindDef = combatPawnKindsForPoint;
                            resolveParams3.singlePawnLord = lord;
                            BaseGen.symbolStack.Push("pawn", resolveParams3);
                        }
                    }
                }
            }

            // Done last because it is the most flexible about things being in the way
            if (!firstLayerThingOptions.NullOrEmpty() || !secondLayerThingOptions.NullOrEmpty() || !thirdLayerThingOptions.NullOrEmpty() || !fourthLayerThingOptions.NullOrEmpty())
                CreateDefenses(rp);
        }

        private void CreateDefenses(ResolveParams rp)
        {
            Map map = BaseGen.globalSettings.map;
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
    }
}
