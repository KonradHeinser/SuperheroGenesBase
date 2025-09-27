using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using RimWorld;
using RimWorld.QuestGen;
using Verse;
using Verse.AI;

namespace SuperHeroGenesBase
{
    public class QuestNode_CopyIdeology : QuestNode
    {
        public SlateRef<Pawn> origin;

        public SlateRef<Pawn> target;

        protected override void RunInt()
        {
            Slate slate = QuestGen.slate;
            target.GetValue(slate).ideo.SetIdeo(origin.GetValue(slate)?.Ideo);
        }

        protected override bool TestRunInt(Slate slate)
        {
            if (origin.GetValue(slate)?.Ideo == null)
                return false;

            if (target.GetValue(slate)?.ideo == null)
                return false;

            return true;
        }
    }
}
