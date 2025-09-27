using RimWorld.QuestGen;
using Verse;

namespace SuperHeroGenesBase
{
    public class QuestNode_CopyIdeology : QuestNode
    {
        public SlateRef<Pawn> origin;

        public SlateRef<Pawn> target;

        protected override void RunInt()
        {
            Slate slate = QuestGen.slate;

            // Mainly used to ensure site generation doesn't generate a target without an ideo
            if (target.GetValue(slate)?.ideo == null)
                return;

            target.GetValue(slate).ideo.SetIdeo(origin.GetValue(slate)?.Ideo);
        }

        protected override bool TestRunInt(Slate slate)
        {
            if (origin.GetValue(slate)?.Ideo == null)
                return false;
            // Can only check the target for ideo if the target is obtained before run
            if (target.GetValue(slate) != null && target.GetValue(slate).ideo == null)
                return false;
            return true;
        }
    }
}
