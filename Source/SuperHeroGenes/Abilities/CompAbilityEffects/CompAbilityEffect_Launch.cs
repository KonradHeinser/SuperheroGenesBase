using RimWorld.Planet;
using Verse.AI.Group;
using Verse;
using RimWorld;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

namespace SuperHeroGenesBase
{
    public class CompAbilityEffect_Launch : CompAbilityEffect
    {
        public new CompProperties_Launch Props => (CompProperties_Launch)props;

        private Thing transporter = null;

        private IEnumerable<IThingHolder> Pod
        {
            get
            {
                if (transporter == null)
                    transporter = ThingMaker.MakeThing(SHGDefOf.SHG_FlightPod);
                yield return transporter.TryGetComp<CompTransporter>();
            }
        }

        public override void Apply(GlobalTargetInfo target)
        {
            var options = DropOptions(target.Tile, TryLaunch).ToList();
            if (options.Count == 2) // The second action will be cancel
                options.First().action();
            else if (options.Count > 2)
                Find.WindowStack.Add(new FloatMenu(options));
        }

        private IEnumerable<FloatMenuOption> DropOptions(PlanetTile destination, Action<PlanetTile, TransportersArrivalAction> action)
        {
            if (TransportersArrivalAction_FormCaravan.CanFormCaravanAt(Pod, destination))
                yield return new FloatMenuOption("FormCaravanHere".Translate(), delegate
                {
                    action(destination, new TransportersArrivalAction_FormCaravan("MessagePawnArrived"));
                });

            var objects = Find.WorldObjects.ObjectsAt(destination);

            if (!objects.EnumerableNullOrEmpty())
                foreach (var obj in objects)
                {
                    if (ModsConfig.OdysseyActive && obj.RequiresSignalJammerToReach && !SuperheroGenes_Settings.noJammerReq)
                        continue;
                    if (obj is Settlement settlement)
                        foreach (FloatMenuOption option in TransportersArrivalAction_AttackSettlement.GetFloatMenuOptions(action, Pod, settlement))
                            yield return option;
                    else
                        foreach (FloatMenuOption option in obj.GetTransportersFloatMenuOptions(Pod, action))
                            yield return option;
                }

            yield return new FloatMenuOption("Cancel".Translate(), delegate
            {
                parent.ResetCooldown();
            });
        }

        private void TryLaunch(PlanetTile destination, TransportersArrivalAction action)
        {
            if (Pod == null)
                return;

            Map map = parent.pawn.MapHeld;
            parent.pawn.GetLord()?.RemovePawn(parent.pawn);
            if (map != null)
            {
                IntVec3 position = parent.pawn.PositionHeld;

                GenSpawn.Spawn(transporter, position, map);

                CompTransporter transportComp = transporter.TryGetComp<CompTransporter>();
                transportComp.groupID = parent.pawn.thingIDNumber;
                transportComp.TryRemoveLord(map);

                transportComp.GetDirectlyHeldThings().TryAddOrTransfer(parent.pawn.SplitOff(1));
                ThingOwner directlyHeldThings = transportComp.GetDirectlyHeldThings();

                ActiveTransporter activeTransporter = (ActiveTransporter)ThingMaker.MakeThing(ThingDefOf.ActiveDropPod);
                activeTransporter.Contents = new ActiveTransporterInfo();
                activeTransporter.Contents.innerContainer.TryAddRangeOrTransfer(directlyHeldThings, true, true);
                activeTransporter.Contents.sentTransporterDef = SHGDefOf.SHG_FlightPod;

                FlyShipLeaving obj = (FlyShipLeaving)SkyfallerMaker.MakeSkyfaller(Props.skyfallerLeaving ?? ThingDefOf.ActiveDropPod, activeTransporter);
                obj.groupID = transportComp.groupID;
                obj.destinationTile = destination;
                obj.arrivalAction = action;
                obj.worldObjectDef = Props.worldObject ?? WorldObjectDefOf.TravellingTransporters;
                if (obj is FlyPawnLeaving pawnLeaving)
                    pawnLeaving.pawn = parent.pawn;

                transportComp.CleanUpLoadingVars(map);
                transportComp.parent.Destroy();

                GenSpawn.Spawn(obj, position, map);
            }
            else
            {
                Caravan caravan = parent.pawn.GetCaravan();
                if (caravan == null) return;
                WorldObject newCaravan = WorldObjectMaker.MakeWorldObject(Props.worldObject ?? WorldObjectDefOf.TravellingTransporters);
                if (newCaravan is TravellingTransporters transport)
                {
                    transport.Tile = caravan.Tile;
                    transport.destinationTile = destination;
                    transport.arrivalAction = action;
                    if (transport is FlyingPawn flyingPawn)
                        flyingPawn.pawn = parent.pawn;
                    ActiveTransporterInfo podInfo = new ActiveTransporterInfo();
                    podInfo.innerContainer.TryAddRangeOrTransfer(caravan.AllThings);
                    podInfo.innerContainer.TryAddRangeOrTransfer(caravan.pawns);
                    podInfo.sentTransporterDef = SHGDefOf.SHG_FlightPod;
                    transport.AddTransporter(podInfo, false);
                    Find.WorldObjects.Add(transport);
                    caravan.Destroy();
                }
            }
            transporter = null;
        }

        public override bool GizmoDisabled(out string reason)
        {
            if (parent.pawn.InBed())
            {
                reason = "StatsReport_InBed".Translate();
                return true;
            }
            if (parent.pawn.MapHeld != null && !parent.pawn.PositionHeld.Standable(parent.pawn.MapHeld))
            {
                reason = "";
                return true;
            }
            if (!Props.disablingBiomes.NullOrEmpty())
            {
                if (parent.pawn.Map != null)
                {
                    if (Props.disablingBiomes.Contains(parent.pawn.Map.Biome))
                    {
                        reason = "SHG_Biome".Translate(parent.pawn.Map.Biome.LabelCap);
                        return true;
                    }
                }
                else
                {
                    Caravan caravan = parent.pawn.GetCaravan();
                    if (caravan != null && Props.disablingBiomes.Contains(caravan.Biome))
                    {
                        reason = "SHG_Biome".Translate(parent.pawn.Map.Biome.LabelCap);
                        return true;
                    }
                }
            }
            return base.GizmoDisabled(out reason);
        }

        public override bool CanApplyOn(GlobalTargetInfo target)
        {
            return Valid(target, false);
        }

        public override bool Valid(GlobalTargetInfo target, bool throwMessages = true)
        {
            Caravan caravan = parent.pawn.GetCaravan();

            // If the pawn isn't in a map or a caravan, get the fuck out of here
            if (!parent.pawn.Spawned && caravan == null) return false;
            if (parent.pawn.Spawned && parent.pawn.Position.Roofed(parent.pawn.Map))
            {
                if (!throwMessages)
                    Messages.Message("CannotUseAbility".Translate(parent.def.label) + ": " + "Roofed".Translate(), parent.pawn, MessageTypeDefOf.RejectInput, false);
                return false;
            }

            if (caravan != null && (Props.noMapTravelWhileImmobilized || Props.noMapTravelWhenTooMuchMass))
            {
                if (Props.noMapTravelWhenTooMuchMass)
                {
                    float maxMass = parent.pawn.GetStatValue(StatDefOf.CarryingCapacity);
                    foreach (Pawn pawn in caravan.PawnsListForReading)
                    {
                        if (pawn == parent.pawn) continue;

                        maxMass -= pawn.GetStatValue(StatDefOf.Mass);
                        if (maxMass < 0) return false;
                    }
                    foreach (Thing thing in caravan.AllThings)
                    {
                        if (thing is Pawn pawn) continue;

                        maxMass -= thing.GetStatValue(StatDefOf.Mass);
                        if (maxMass < 0) return false;
                    }
                }
                else
                {
                    if (caravan.ImmobilizedByMass) return false;
                }
            }
            PlanetTile tile = parent.pawn.Tile;
            PlanetLayer layer = target.Tile.Layer;
            PlanetTile layerTile = layer.GetClosestTile_NewTemp(tile);
            if (!Props.disablingBiomes.NullOrEmpty() && Props.disablingBiomes.Contains(target.Tile.Tile.PrimaryBiome))
                return false;

            int distance = Props.maxDistance;
            if (Props.distanceFactorStat != null)
                distance = (int)Math.Floor(distance * parent.pawn.GetStatValue(Props.distanceFactorStat));
            distance = Mathf.RoundToInt((float)distance / (float)layer.Def.rangeDistanceFactor);

            GenDraw.DrawWorldRadiusRing(layerTile, distance, CompPilotConsole.GetFuelRadiusMat(layerTile));

            if (Find.WorldGrid.TraversalDistanceBetween(layerTile, target.Tile, canTraverseLayers: true) > distance)
                return false;
            if (DropOptions(target.Tile, null).ToList().Count == 1)
                return false;

            return base.Valid(target, throwMessages);
        }
    }
}
