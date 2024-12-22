using Verse;
using RimWorld.QuestGen;

namespace SuperHeroGenesBase
{
    public class QuestNode_LogMessage : QuestNode
    {
        public string message = "Quest run";

        protected override void RunInt()
        {
            Log.Message(message + " : RunInt");
        }

        protected override bool TestRunInt(Slate slate)
        {
            Log.Message(message + " : TestRunInt");
            return true;
        }
    }
}
