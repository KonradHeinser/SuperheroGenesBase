using Verse;
using RimWorld;
using RimWorld.QuestGen;

namespace SuperHeroGenesBase
{
    public class QuestNode_GetSameQuestsSuccessOnly : QuestNode
    {
        [NoTranslate]
        public SlateRef<string> storeAs;

        protected override bool TestRunInt(Slate slate)
        {
            return true;
        }

        protected override void RunInt()
        {
            SetVars(QuestGen.slate);
        }

        private void SetVars(Slate slate)
        {
            int var = Find.QuestManager.QuestsListForReading.Count((Quest x) => x.root == QuestGen.Root && x.State == QuestState.EndedSuccess);
            slate.Set(storeAs.GetValue(slate), var);
        }
    }
}
