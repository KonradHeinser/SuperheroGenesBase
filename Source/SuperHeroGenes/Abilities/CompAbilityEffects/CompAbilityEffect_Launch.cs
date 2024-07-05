using RimWorld.Planet;
using Verse;
using RimWorld;
using System;

namespace SuperHeroGenesBase
{
    public class CompAbilityEffect_Launch : CompAbilityEffect
    {
        public new CompProperties_Launch Props => (CompProperties_Launch)props;

        public override void Apply(GlobalTargetInfo target)
        {
            Map map = parent.pawn.Map;
            if (map != null)
            {
                IntVec3 position = parent.pawn.Position;

                Thing transporter = ThingMaker.MakeThing(ThingDefOf.TransportPod);
                GenSpawn.Spawn(transporter, position, map);

                CompTransporter transportComp = transporter.TryGetComp<CompTransporter>();
                transportComp.groupID = parent.pawn.thingIDNumber;
                transportComp.TryRemoveLord(map);

                transportComp.GetDirectlyHeldThings().TryAddOrTransfer(parent.pawn.SplitOff(1));
                ThingOwner directlyHeldThings = transportComp.GetDirectlyHeldThings();

                ActiveDropPod activeDropPod = (ActiveDropPod)ThingMaker.MakeThing(ThingDefOf.ActiveDropPod);
                activeDropPod.Contents = new ActiveDropPodInfo();
                activeDropPod.Contents.innerContainer.TryAddRangeOrTransfer(directlyHeldThings, true, true);

                FlyShipLeaving obj = (FlyShipLeaving)SkyfallerMaker.MakeSkyfaller(Props.skyfallerLeaving ?? ThingDefOf.DropPodLeaving, activeDropPod);
                obj.groupID = transportComp.groupID;
                obj.destinationTile = target.Tile;
                obj.arrivalAction = new TransportPodsArrivalAction_FormCaravan("MessagePawnArrived");
                obj.worldObjectDef = Props.worldObject ?? WorldObjectDefOf.TravelingTransportPods;

                transportComp.CleanUpLoadingVars(map);
                transportComp.parent.Destroy();

                GenSpawn.Spawn(obj, position, map);
            }
            else
            {
                Caravan caravan = parent.pawn.GetCaravan();
                if (caravan == null) return;
                WorldObject newCaravan = WorldObjectMaker.MakeWorldObject(Props.worldObject ?? WorldObjectDefOf.TravelingTransportPods);
                if (newCaravan is TravelingTransportPods transport)
                {
                    transport.Tile = caravan.Tile;
                    transport.destinationTile = target.Tile;
                    transport.arrivalAction = new TransportPodsArrivalAction_FormCaravan("MessagePawnArrived");
                    ActiveDropPodInfo podInfo = new ActiveDropPodInfo();
                    podInfo.innerContainer.TryAddRangeOrTransfer(caravan.AllThings);
                    podInfo.innerContainer.TryAddRangeOrTransfer(caravan.pawns);
                    transport.AddPod(podInfo, false);
                    Find.WorldObjects.Add(transport);
                    caravan.Destroy();
                }
            }
        }

        public override bool GizmoDisabled(out string reason)
        {
            if (!Props.disablingBiomes.NullOrEmpty())
            {
                if (parent.pawn.Map != null)
                {
                    if (Props.disablingBiomes.Contains(parent.pawn.Map.Biome))
                    {
                        reason = "SHG_Biome".Translate(parent.pawn.Map.Biome.LabelCap);
                        return false;
                    }
                }
                else
                {
                    Caravan caravan = parent.pawn.GetCaravan();
                    if (caravan != null && Props.disablingBiomes.Contains(caravan.Biome))
                    {
                        reason = "SHG_Biome".Translate(parent.pawn.Map.Biome.LabelCap);
                        return false;
                    }
                }
            }
            return base.GizmoDisabled(out reason);
        }

        public override bool CanApplyOn(GlobalTargetInfo target)
        {
            return Valid(target, true);
        }

        public override bool Valid(GlobalTargetInfo target, bool throwMessages = false)
        {
            Caravan caravan = parent.pawn.GetCaravan();

            // If the pawn isn't in a map or a caravan, get the fuck out of here
            if (parent.pawn.Map == null && caravan == null) return false;
            if (parent.pawn.Map != null && !parent.pawn.Spawned) return false;

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

            int tile = parent.pawn.Tile;

            int distance = Props.maxDistance;
            if (Props.distanceFactorStat != null) distance = (int)Math.Floor(distance * parent.pawn.GetStatValue(Props.distanceFactorStat));

            GenDraw.DrawWorldRadiusRing(tile, distance);

            if (Find.WorldGrid.TraversalDistanceBetween(tile, target.Tile) > distance) return false;
            if (Find.World.Impassable(target.Tile)) return false;

            return base.Valid(target, throwMessages);
        }
    }
}
