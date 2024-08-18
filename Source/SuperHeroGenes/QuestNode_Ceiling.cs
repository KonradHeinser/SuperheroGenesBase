using RimWorld.QuestGen;
using System;
using Verse;

namespace SuperHeroGenesBase
{
    public class QuestNode_Ceiling : QuestNode
    {
        public SlateRef<double> value;

        [NoTranslate]
        public SlateRef<string> storeAs;

        protected override bool TestRunInt(Slate slate)
        {
            return !storeAs.GetValue(slate).NullOrEmpty();
        }

        protected override void RunInt()
        {
            Slate slate = QuestGen.slate;
            slate.Set(storeAs.GetValue(slate), Math.Ceiling(value.GetValue(slate)));
        }
    }
}
