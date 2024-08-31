using Verse;
using System.Collections.Generic;
using RimWorld;
using RimWorld.QuestGen;
using System.Linq;

namespace SuperHeroGenesBase
{
    public class QuestNode_NotAllowedIfOthersPassed : QuestNode
    {
        public SlateRef<List<QuestScriptDef>> quests;

        protected override void RunInt()
        {
            return;
        }

        protected override bool TestRunInt(Slate slate)
        {
            if (quests.GetValue(slate) != null)
            {
                List<QuestScriptDef> avoidQuests = quests.GetValue(slate);
                if (avoidQuests.NullOrEmpty()) return true;

                foreach (Quest quest in Find.QuestManager.QuestsListForReading)
                    if (avoidQuests.Contains(quest.root) && quest.State == QuestState.EndedSuccess) return false;
            }
            return true;
        }
    }
}
