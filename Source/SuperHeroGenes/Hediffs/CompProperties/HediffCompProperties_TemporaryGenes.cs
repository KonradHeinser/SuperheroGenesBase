using Verse;
using System.Collections.Generic;

namespace SuperHeroGenesBase
{
    public class HediffCompProperties_TemporaryGenes : HediffCompProperties
    {
        public List<GenesAtSeverity> genesAtSeverities;

        public HediffCompProperties_TemporaryGenes()
        {
            compClass = typeof(HediffComp_TemporaryGenes);
        }
    }
}
