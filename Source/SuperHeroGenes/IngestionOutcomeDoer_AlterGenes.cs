using System.Collections.Generic;
using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class IngestionOutcomeDoer_AlterGenes : IngestionOutcomeDoer
    {
        public List<GeneDef> alwaysAddedGenes;

        public bool inheritable = true;

        public List<GeneDef> alwaysRemovedGenes;

        public List<RandomXenoGenes> geneSets;

        public bool removeGenesFromOtherLists = true;

        public bool showMessage = true;

        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
        {
            pawn.GainRandomGeneSet(inheritable, removeGenesFromOtherLists, geneSets,
                alwaysAddedGenes, alwaysRemovedGenes, showMessage);
        }
    }
}
