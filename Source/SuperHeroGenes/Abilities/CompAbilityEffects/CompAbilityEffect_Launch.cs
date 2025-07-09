using RimWorld.Planet;
using Verse.AI.Group;
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
            Map map = parent.pawn.MapHeld;
            parent.pawn.GetLord()?.RemovePawn(parent.pawn);
            if (map != null)
            {
                IntVec3 position = parent.pawn.PositionHeld;

                Thing transporter = ThingMaker.MakeThing(ThingDefOf.TransportPod);
                GenSpawn.Spawn(transporter, position, map);

                CompTransporter transportComp = transporter.TryGetComp<CompTransporter>();
                transportComp.groupID = parent.pawn.thingIDNumber;
                transportComp.TryRemoveLord(map);

                transportComp.GetDirectlyHeldThings().TryAddOrTransfer(parent.pawn.SplitOff(1));
                ThingOwner directlyHeldThings = transportComp.GetDirectlyHeldThings();

                ActiveTransporter activeTransporter = (ActiveTransporter)ThingMaker.MakeThing(ThingDefOf.ActiveDropPod);
                activeTransporter.Contents = new ActiveTransporterInfo();
                activeTransporter.Contents.innerContainer.TryAddRangeOrTransfer(directlyHeldThings, true, true);

                FlyShipLeaving obj = (FlyShipLeaving)SkyfallerMaker.MakeSkyfaller(Props.skyfallerLeaving ?? ThingDefOf.ActiveDropPod, activeTransporter);
                obj.groupID = transportComp.groupID;
                obj.destinationTile = target.Tile;
                obj.arrivalAction = new TransportersArrivalAction_FormCaravan("MessagePawnArrived");
                obj.worldObjectDef = Props.worldObject ?? WorldObjectDefOf.TravellingTransporters;

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
                    transport.destinationTile = target.Tile;
                    transport.arrivalAction = new TransportersArrivalAction_FormCaravan("MessagePawnArrived");
                    ActiveTransporterInfo podInfo = new ActiveTransporterInfo();
                    podInfo.innerContainer.TryAddRangeOrTransfer(caravan.AllThings);
                    podInfo.innerContainer.TryAddRangeOrTransfer(caravan.pawns);
                    transport.AddTransporter(podInfo, false);
                    Find.WorldObjects.Add(transport);
                    caravan.Destroy();
                }
            }
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
