using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace SuperHeroGenesBase
{
    public class IngestionOutcomeDoer_OffsetResource : IngestionOutcomeDoer
    {
        public float offset;

        public GeneDef mainResourceGene;

        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested)
        {
            ResourceGene resourceGene = (ResourceGene)pawn.genes.GetGene(mainResourceGene);
            ResourceGene.OffsetResource(pawn, offset * (float)ingested.stackCount, resourceGene, resourceGene.def.GetModExtension<DRGExtension>());
        }

        public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
        {
            if (ModsConfig.BiotechActive)
            {
                string text = ((offset >= 0f) ? "+" : string.Empty);
                yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawnImportant, "Resource".Translate().CapitalizeFirst(), text + Mathf.RoundToInt(offset * 100f), "ResourceDesc".Translate(), 1000);
            }
        }
    }
}
