using Verse;
using System.Collections.Generic;

namespace SuperHeroGenesBase
{
    public class HediffComp_TemporaryGenes : HediffComp
    {
        public HediffCompProperties_TemporaryGenes Props => (HediffCompProperties_TemporaryGenes)props;

        public List<GeneDef> addedGenes;

        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            if (addedGenes == null)
            {
                addedGenes = new List<GeneDef>();
            }

            if (Props.genesAtSeverities.NullOrEmpty())
            {
                Log.Error(Def + " doesn't have any lists to use in HediffCompProperties_TemporaryGenes. Deleting hediff to avoid more errors.");
                parent.pawn.health.RemoveHediff(parent);
                return;
            }

            foreach (GenesAtSeverity geneSet in Props.genesAtSeverities)
            {
                if (geneSet.severities.Includes(parent.Severity))
                {
                    if (SHGUtilities.EquivalentGeneLists(new List<GeneDef>(addedGenes), new List<GeneDef>(geneSet.genes))) break;
                    SHGUtilities.RemoveGenesFromPawn(parent.pawn, addedGenes);
                    addedGenes.Clear();
                    addedGenes = SHGUtilities.AddGenesToPawn(parent.pawn, geneSet.xenogenes, geneSet.genes);
                    break;
                }
            }
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            if (!parent.pawn.IsHashIntervalTick(50)) return;

            foreach (GenesAtSeverity geneSet in Props.genesAtSeverities)
            {
                if (geneSet.severities.Includes(parent.Severity))
                {
                    if (SHGUtilities.EquivalentGeneLists(new List<GeneDef>(addedGenes), new List<GeneDef>(geneSet.genes))) break;
                    SHGUtilities.RemoveGenesFromPawn(parent.pawn, addedGenes);
                    addedGenes.Clear();
                    addedGenes = SHGUtilities.AddGenesToPawn(parent.pawn, geneSet.xenogenes, geneSet.genes);
                    break;
                }
            }
        }

        public override void CompPostPostRemoved()
        {
            SHGUtilities.RemoveGenesFromPawn(parent.pawn, addedGenes);
            addedGenes.Clear();
        }

        public override void CompExposeData()
        {
            Scribe_Collections.Look(ref addedGenes, "EBSG_AddedGenes");
        }
    }
}
