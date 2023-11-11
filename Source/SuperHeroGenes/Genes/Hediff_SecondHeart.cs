using Verse;

namespace SuperHeroGenesBase
{
    public class Hediff_SecondHeart : HediffWithComps
    {
        [Unsaved(false)]
        private Gene_DoubleHearted cachedDependencyGene;

        public bool ShouldSatify => Severity >= def.stages[1].minSeverity - 0.1f;

        public override bool ShouldRemove => false;

        public override bool Visible
        {
            get
            {
                if (LinkedGene != null && !LinkedGene.Active)
                {
                    return false;
                }
                return base.Visible;
            }
        }

        public Gene_DoubleHearted LinkedGene
        {
            get
            {
                if (cachedDependencyGene == null)
                {
                    cachedDependencyGene = pawn.genes?.GetFirstGeneOfType<Gene_DoubleHearted>();
                }
                return cachedDependencyGene;
            }
        }
    }
}