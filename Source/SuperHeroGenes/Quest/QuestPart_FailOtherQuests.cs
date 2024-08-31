using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class QuestPart_FailOtherQuests : QuestPart
    {
        public string inSignal;

        public List<QuestScriptDef> quests;

        public QuestEndOutcome outcome;

        public override void Notify_QuestSignalReceived(Signal signal)
        {
            base.Notify_QuestSignalReceived(signal);
            if (signal.tag == inSignal)
            {
                List<QuestScriptDef> scriptDefs = quests;
                List<Quest> activeQuests = Find.QuestManager.QuestsListForReading.Where((Quest q) => scriptDefs.Contains(q.root)
                        && (q.State == QuestState.NotYetAccepted || q.State == QuestState.Ongoing)).ToList();
                if (activeQuests.NullOrEmpty()) return;
                foreach (Quest aQuest in activeQuests)
                    if (aQuest.State == QuestState.NotYetAccepted)
                        aQuest.End(QuestEndOutcome.InvalidPreAcceptance, false, false);
                    else
                        aQuest.End(outcome, false, false);
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref quests, "quests", LookMode.Def);
            Scribe_Values.Look(ref inSignal, "inSignal");
            Scribe_Values.Look(ref outcome, "outcome");
        }

    }
}
