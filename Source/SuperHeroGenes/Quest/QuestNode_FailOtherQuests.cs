using Verse;
using System.Collections.Generic;
using RimWorld;
using RimWorld.QuestGen;
using System.Linq;

namespace SuperHeroGenesBase
{
    public class QuestNode_FailOtherQuests : QuestNode
    {
        [NoTranslate]
        public SlateRef<string> inSignal;

        public SlateRef<List<QuestScriptDef>> quests;

        public SlateRef<QuestEndOutcome> outcome = QuestEndOutcome.Fail;

        protected override void RunInt()
        {
            Slate slate = QuestGen.slate;
            if (quests.GetValue(slate).NullOrEmpty()) return;

            QuestGen.quest.AddPart(new QuestPart_FailOtherQuests
            {
                quests = quests.GetValue(slate),
                outcome = outcome.GetValue(slate),
                inSignal = QuestGenUtility.HardcodedSignalWithQuestID(inSignal.GetValue(slate))
            });
        }

        protected override bool TestRunInt(Slate slate)
        {
            return quests.GetValue(slate) != null;
        }
    }
}
