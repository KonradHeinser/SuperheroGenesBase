﻿using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class HediffComp_CreateItems : HediffComp
    {
        public HediffCompProperties_CreateItems Props => (HediffCompProperties_CreateItems)props;

        public int ticksLeft;

        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            ticksLeft = Props.intervalTicks.RandomInRange;
        }

        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {
            MakeThingsFromList(Props.onDeathThings);
            MakeThingsFromList(Props.onDeathOrRemovalThings);
        }

        public override void CompPostPostRemoved()
        {
            MakeThingsFromList(Props.onRemovalThings);
            MakeThingsFromList(Props.onDeathOrRemovalThings);
        }

        public override void CompPostTickInterval(ref float severityAdjustment, int delta)
        {
            base.CompPostTickInterval(ref severityAdjustment, delta);

            if (!Props.intervalThings.NullOrEmpty())
            {
                ticksLeft -= delta;
                if (ticksLeft <= 0)
                    if (MakeThingsFromList(Props.intervalThings))
                        ticksLeft += Props.intervalTicks.RandomInRange;
            }
        }

        public bool MakeThingsFromList(List<List<ThingCreationItem>> list)
        {
            if (list.NullOrEmpty()) return true;

            Map map = parent.pawn.MapHeld;
            Caravan caravan = parent.pawn.GetCaravan();
            if (map == null && caravan == null) // Not sure what situations would cause this, but I'm not about to mess with them
            {
                ticksLeft = 200;
                return false;
            }

            float severity = parent.Severity;
            List<IntVec3> alreadyUsedSpots = new List<IntVec3>();

            foreach (List<ThingCreationItem> optionList in list)
                if (!optionList.Where((arg) => arg.severities.Includes(severity)).EnumerableNullOrEmpty())
                {
                    ThingCreationItem option = optionList.Where((arg) => arg.severities.Includes(severity)).RandomElementByWeight((arg) => arg.weight);
                    Thing thing = SHGUtilities.CreateThingCreationItem(option, Pawn);
                    if (thing == null) continue;
                    if (map != null)
                    {
                        IntVec3 intVec;
                        if (parent.pawn.Position.Walkable(map) && (alreadyUsedSpots.NullOrEmpty() || !alreadyUsedSpots.Contains(parent.pawn.Position)))
                        {
                            intVec = parent.pawn.Position;
                            alreadyUsedSpots.Add(parent.pawn.Position);
                        }
                        else intVec = CellFinder.RandomClosewalkCellNear(parent.pawn.Position, map, 1, delegate (IntVec3 cell)
                        {
                            if (!alreadyUsedSpots.NullOrEmpty() && alreadyUsedSpots.Contains(cell)) return false;
                            if (cell != parent.pawn.Position)
                            {
                                Building building = map.edificeGrid[cell];
                                if (building == null)
                                {
                                    alreadyUsedSpots.Add(cell);
                                    return true;
                                }

                                if (building.def?.IsBed != true) alreadyUsedSpots.Add(cell);
                                return building.def?.IsBed != true;
                            }
                            return false;
                        });
                        GenPlace.TryPlaceThing(thing, intVec, map, ThingPlaceMode.Near);
                    }
                    else if (thing.def.Minifiable || (!thing.def.IsPlant && !typeof(Building).IsAssignableFrom(thing.def.thingClass)))
                    {
                        if (thing.def.Minifiable) caravan.AddPawnOrItem(thing.MakeMinified(), false);
                        else caravan.AddPawnOrItem(thing, true);
                    }
                }
            return true;
        }

        public override void CompExposeData()
        {
            Scribe_Values.Look(ref ticksLeft, "ticksLeft", 2500);
        }
    }
}
