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

            // Mainly used to ensure site generation doesn't generate pawns without an ideo
            if (origin.GetValue(slate)?.Ideo == null)
                return;
            if (target.GetValue(slate)?.ideo == null)
                return;

            target.GetValue(slate).ideo.SetIdeo(origin.GetValue(slate)?.Ideo);
        }

        protected override bool TestRunInt(Slate slate)
        {
            return true;
        }
    }
}
