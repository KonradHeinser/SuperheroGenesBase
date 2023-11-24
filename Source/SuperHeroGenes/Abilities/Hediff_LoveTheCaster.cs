using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class Hediff_LoveTheCaster : HediffWithTarget
    {
        public override string LabelBase => base.LabelBase + " " + def.targetPrefix + " " + target?.LabelShortCap;
    }
}
