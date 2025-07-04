using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace SuperHeroGenesBase
{
    public class LayoutWorkerComplex_FixedFaction : LayoutWorkerComplex
    {
        public LayoutWorkerComplex_FixedFaction(LayoutDef def)
        : base(def)
        {
        }

        public override Faction GetFixedHostileFactionForThreats()
        {
            if (!Def.threats.NullOrEmpty() && Def.threats[0].def.faction != null)
            {
                return Find.FactionManager.FirstFactionOfDef(Def.threats[0].def.faction);
            }

            return null;
        }

        protected override void PreSpawnThreats(List<LayoutRoom> rooms, Map map, List<Thing> allSpawnedThings)
        {
            if (Def.rewardThingSetMakerDef != null)
                if (Def.roomRewardCrateFactor > 0f)
                {
                    int num = 0;
                    for (int i = 0; i < allSpawnedThings.Count; i++)
                    {
                        if (allSpawnedThings[i] is Building_Crate)
                        {
                            num++;
                        }
                    }
                    int num2 = Mathf.RoundToInt((float)rooms.Count * Def.roomRewardCrateFactor) - num;
                    if (num2 <= 0)
                    {
                        return;
                    }
                    ThingSetMakerDef thingSetMakerDef = Def.rewardThingSetMakerDef ?? ThingSetMakerDefOf.Reward_ItemsStandard;
                    foreach (LayoutRoom item in rooms.InRandomOrder())
                    {
                        bool flag = true;
                        
                        List<IntVec3> list = new List<IntVec3>(item.Cells);
                        foreach (IntVec3 item2 in list)
                        {
                            Building edifice = item2.GetEdifice(map);
                            if (edifice != null && edifice is Building_Crate)
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (!flag)
                        {
                            continue;
                        }
                        if (ComplexUtility.TryFindRandomSpawnCell(ThingDefOf.AncientHermeticCrate, item, map, out var spawnPosition, 1, Rot4.South))
                        {
                            Building_Crate building_Crate = (Building_Crate)GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.AncientHermeticCrate), spawnPosition, map, Rot4.South);
                            List<Thing> list2 = thingSetMakerDef.root.Generate(default(ThingSetMakerParams));
                            for (int num3 = list2.Count - 1; num3 >= 0; num3--)
                            {
                                Thing thing = list2[num3];
                                if (!building_Crate.TryAcceptThing(thing, false))
                                {
                                    thing.Destroy();
                                }
                            }
                            num2--;
                        }
                        if (num2 <= 0)
                        {
                            break;
                        }
                    }
                }
        }
    }
}
