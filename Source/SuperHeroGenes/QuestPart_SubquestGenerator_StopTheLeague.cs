using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using RimWorld;

namespace SuperHeroGenesBase
{
    public class QuestPart_SubquestGenerator_StopTheLeague : QuestPart_SubquestGenerator
    {
        private List<QuestScriptDef> questQueue = new List<QuestScriptDef>();

        public MapParent useMapParentThreatPoints;

        protected override Slate InitSlate()
        {
            float var = 0f;
            if (useMapParentThreatPoints != null)
            {
                var = useMapParentThreatPoints.HasMap ? StorytellerUtility.DefaultThreatPointsNow(useMapParentThreatPoints.Map) : ((Find.AnyPlayerHomeMap == null) ? StorytellerUtility.DefaultThreatPointsNow(Find.World) : StorytellerUtility.DefaultThreatPointsNow(Find.AnyPlayerHomeMap));
            }
            Slate slate = new Slate();
            slate.Set("points", var);
            return slate;
        }

        protected override QuestScriptDef GetNextSubquestDef()
        {
            ShuffleQueue();
            QuestScriptDef questScriptDef = questQueue.First();
            if (!questScriptDef.CanRun(InitSlate()))
                return null;

            questQueue.RemoveAt(0);
            return questScriptDef;
        }

        private void ShuffleQueue()
        {
            questQueue.Clear();
            if (subquestDefs.Count == 1)
            {
                questQueue.AddRange(subquestDefs);
                return;
            }
            QuestScriptDef questScriptDef = (from q in quest.GetSubquests()
                                             orderby q.appearanceTick descending
                                             select q).FirstOrDefault()?.root;
            int num = 100;
            while (num > 0)
            {
                num--;
                questQueue.Clear();
                questQueue.AddRange(subquestDefs.InRandomOrder());
                if (questScriptDef == null || questQueue.First() != questScriptDef)
                {
                    break;
                }
            }
        }

        public override void DoDebugWindowContents(Rect innerRect, ref float curY)
        {
            base.DoDebugWindowContents(innerRect, ref curY);
            if (Widgets.ButtonText(new Rect(innerRect.x, curY, 500f, 25f), "Shuffle subquest queue"))
            {
                ShuffleQueue();
            }
            curY += 29f;
            if (Widgets.ButtonText(new Rect(innerRect.x, curY, 500f, 25f), "Log subquest queue"))
            {
                if (questQueue.Count == 0)
                {
                    ShuffleQueue();
                }
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < questQueue.Count; i++)
                {
                    stringBuilder.AppendLine($"{i} ->" + questQueue[i].defName);
                }
                Log.Message(stringBuilder.ToString());
            }
            curY += 29f;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref questQueue, "questQueue", LookMode.Def);
            Scribe_References.Look(ref useMapParentThreatPoints, "useMapParentThreatPoints");
            if (Scribe.mode == LoadSaveMode.PostLoadInit && questQueue == null)
            {
                questQueue = new List<QuestScriptDef>();
            }
        }
    }
}
