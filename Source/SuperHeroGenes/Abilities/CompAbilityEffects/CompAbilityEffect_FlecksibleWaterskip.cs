using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace SuperHeroGenesBase
{
    public class CompAbilityEffect_FlecksibleWaterskip : CompAbilityEffect
    {
        private new CompProperties_AbilityFlecksibleWaterskip Props => (CompProperties_AbilityFlecksibleWaterskip)props;
        
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            Map map = parent.pawn.Map;
            foreach (IntVec3 item in AffectedCells(target, map))
            {
                List<Thing> thingList = item.GetThingList(map);
                foreach (Thing thing in thingList)
                {
                    if (thing is Fire fire)
                        fire.Destroy();
                    else if (thing is Pawn pawn)
                        pawn.GetInvisibilityComp()?.DisruptInvisibility();
                }
                
                if (!item.Filled(map))
                    FilthMaker.TryMakeFilth(item, map, ThingDefOf.Filth_Water);

                if (Props.fleck != null)
                {
                    FleckCreationData dataStatic = FleckMaker.GetDataStatic(item.ToVector3Shifted(), map, Props.fleck);
                    dataStatic.rotationRate = Rand.Range(-30, 30);
                    dataStatic.rotation = 90 * Rand.RangeInclusive(0, 3);
                    map.flecks.CreateFleck(dataStatic);
                }
                CompAbilityEffect_Teleport.SendSkipUsedSignal(item, parent.pawn);
            }
        }

        private IEnumerable<IntVec3> AffectedCells(LocalTargetInfo target, Map map)
        {
            if (!target.Cell.InBounds(map) || target.Cell.Filled(parent.pawn.Map))
                yield break;
            
            foreach (IntVec3 item in GenRadial.RadialCellsAround(target.Cell, parent.def.EffectRadius, true).Where(c => c.InBounds(map) && GenSight.LineOfSightToEdges(target.Cell, c, map, true)))
                yield return item;
        }

        public override void DrawEffectPreview(LocalTargetInfo target)
        {
            GenDraw.DrawFieldEdges(AffectedCells(target, parent.pawn.Map).ToList(), Valid(target) ? Color.white : Color.red);
        }

        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            if (target.Cell.Filled(parent.pawn.Map))
            {
                if (throwMessages)
                    Messages.Message("AbilityOccupiedCells".Translate(parent.def.LabelCap), target.ToTargetInfo(parent.pawn.Map), MessageTypeDefOf.RejectInput, historical: false);
                return false;
            }
            return true;
        }
    }
}