using Verse;
using System.Collections.Generic;

namespace SuperHeroGenesBase
{
    public class HediffComp_SeverityFromResource : HediffComp
    {
        private ResourceGene cachedResourceGene;

        public HediffCompProperties_SeverityFromResource Props => (HediffCompProperties_SeverityFromResource)props;

        public override bool CompShouldRemove => CheckForResourceGene();

        public ResourceGene Resource
        {
            get
            {
                if (Props.mainResourceGene == null) Log.Error(parent.Label + "is missing the mainResource gene, meaning it can't increase the resource level.");
                 else if (cachedResourceGene == null || !cachedResourceGene.Active)
                {
                    cachedResourceGene = (ResourceGene)parent.pawn.genes.GetGene(Props.mainResourceGene);
                }
                return cachedResourceGene;
            }
        }

        public bool CheckForResourceGene()
        {
            if (Props.mainResourceGene != null)
            {
                if (parent.pawn.genes.GetGene(Props.mainResourceGene) != null) return true;
            }
            return false;
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            severityAdjustment += ((Resource.Value > 0f) ? Props.severityPerHourResource : Props.severityPerHourEmpty) / 2500f;
        }
    }
}
