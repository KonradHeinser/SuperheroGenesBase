using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using RimWorld;
using System;

namespace SuperHeroGenesBase
{
    public class CompAbilityEffect_CreateItems : CompAbilityEffect
    {
        public new CompProperties_AbilityCreateItems Props => (CompProperties_AbilityCreateItems)props;

        public Dictionary<IntVec3, ThingPatternPart> partList = new Dictionary<IntVec3, ThingPatternPart>();

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            Map map = parent.pawn.Map;
            List<Thing> list = new List<Thing>();
            list.AddRange(AffectedCells(target, map).SelectMany((IntVec3 c) => from t in c.GetThingList(map)
                                                                               where t.def.category == ThingCategory.Item && (t.def != partList[c].thing || t.def.stackLimit == 1)
                                                                               select t));
            foreach (Thing item in list)
            {
                item.DeSpawn();
            }
            foreach (IntVec3 item2 in AffectedCells(target, map))
            {
                Thing thing = ThingMaker.MakeThing(partList[item2].thing);
                thing.stackCount = Math.Max(partList[item2].count, partList[item2].thing.stackLimit);
                GenSpawn.Spawn(thing, item2, map);
                if (Props.sendSkipSignal) CompAbilityEffect_Teleport.SendSkipUsedSignal(item2, parent.pawn);
            }
            foreach (Thing item3 in list)
            {
                IntVec3 intVec = IntVec3.Invalid;
                for (int i = 0; i < 9; i++)
                {
                    IntVec3 intVec2 = item3.Position + GenRadial.RadialPattern[i];
                    if (intVec2.InBounds(map) && intVec2.Walkable(map) && map.thingGrid.ThingsListAtFast(intVec2).Count <= 0)
                    {
                        intVec = intVec2;
                        break;
                    }
                }
                if (intVec != IntVec3.Invalid)
                {
                    GenSpawn.Spawn(item3, intVec, map);
                }
                else
                {
                    GenPlace.TryPlaceThing(item3, item3.Position, map, ThingPlaceMode.Near);
                }
            }
            Log.Message("Done respawning despawns");
        }

        public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
        {
            return Valid(target, true);
        }

        public override void DrawEffectPreview(LocalTargetInfo target)
        {
            GenDraw.DrawFieldEdges(AffectedCells(target, parent.pawn.Map).ToList(), Valid(target) ? Color.white : Color.red);
        }

        private IEnumerable<IntVec3> AffectedCells(LocalTargetInfo target, Map map)
        {
            if (!Props.thingPattern.NullOrEmpty())
            {
                partList.Clear();
                foreach (ThingPatternPart patternPart in Props.thingPattern)
                {
                    if (patternPart.thing != null && patternPart.count > 0)
                    {
                        IntVec3 intVec = target.Cell + new IntVec3(patternPart.relativeLocation.x, 0, patternPart.relativeLocation.z);
                        if ((!patternPart.skipIfBlocked || (!intVec.Filled(parent.pawn.Map) && intVec.Standable(parent.pawn.Map))) && !partList.ContainsKey(intVec))
                        {
                            partList.Add(intVec, patternPart);
                            yield return intVec;
                        }
                    }
                }
            }
        }

        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            if (AffectedCells(target, parent.pawn.Map).Any((IntVec3 c) => c.Filled(parent.pawn.Map)))
            {
                if (throwMessages)
                {
                    Messages.Message("CannotUseAbility".Translate(parent.def.label) + ": " + "AbilityOccupiedCells".Translate(), target.ToTargetInfo(parent.pawn.Map), MessageTypeDefOf.RejectInput, historical: false);
                }
                return false;
            }
            if (AffectedCells(target, parent.pawn.Map).Any((IntVec3 c) => !c.Standable(parent.pawn.Map)))
            {
                if (throwMessages)
                {
                    Messages.Message("CannotUseAbility".Translate(parent.def.label) + ": " + "AbilityUnwalkable".Translate(), target.ToTargetInfo(parent.pawn.Map), MessageTypeDefOf.RejectInput, historical: false);
                }
                return false;
            }
            return true;
        }
    }
}
