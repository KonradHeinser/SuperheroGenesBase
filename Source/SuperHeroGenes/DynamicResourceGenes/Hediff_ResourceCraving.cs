using Verse;

namespace SuperHeroGenesBase
{
    public class Hediff_ResourceCraving : HediffWithComps
    {
        public override string SeverityLabel => Severity.ToStringPercent();

        public override bool Visible => true;
    }
}
