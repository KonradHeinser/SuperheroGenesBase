using Verse;
using System.Collections.Generic;
using System;

namespace SuperHeroGenesBase
{
    public class HediffAdder : Gene
    {
        public override void PostAdd()
        {
            base.PostAdd();
            HediffAdding(pawn, this);
        }

        public override void PostRemove()
        {
            base.PostRemove();
            HediffRemoving(pawn, this);
        }

        public static void HediffAdding(Pawn pawn, Gene gene)
        {
            SHGExtension extension = gene.def.GetModExtension<SHGExtension>();
            if (extension != null && !extension.hediffsToApply.NullOrEmpty())
            {
                Dictionary<BodyPartDef, int> foundParts = new Dictionary<BodyPartDef, int>();
                foreach (HediffsToParts hediffToParts in extension.hediffsToApply)
                {
                    foundParts.Clear();
                    if (!hediffToParts.bodyParts.NullOrEmpty())
                    {
                        foreach (BodyPartDef bodyPartDef in hediffToParts.bodyParts)
                        {
                            if (pawn.RaceProps.body.GetPartsWithDef(bodyPartDef).NullOrEmpty()) continue;
                            if (foundParts.NullOrEmpty() || !foundParts.ContainsKey(bodyPartDef))
                            {
                                foundParts.Add(bodyPartDef, 0);
                            }
                            if (hediffToParts.onlyIfNew) SHGUtilities.AddHediffToPart(pawn, pawn.RaceProps.body.GetPartsWithDef(bodyPartDef).ToArray()[foundParts[bodyPartDef]], hediffToParts.hediff, hediffToParts.severity);
                            else SHGUtilities.AddHediffToPart(pawn, pawn.RaceProps.body.GetPartsWithDef(bodyPartDef).ToArray()[foundParts[bodyPartDef]], hediffToParts.hediff, hediffToParts.severity, hediffToParts.severity);
                            foundParts[bodyPartDef]++;
                        }
                    }
                    else
                    {
                        if (SHGUtilities.HasHediff(pawn, hediffToParts.hediff))
                        {
                            if (hediffToParts.onlyIfNew) continue;
                            Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(hediffToParts.hediff);
                            hediff.Severity += hediffToParts.severity;
                        }
                        else
                        {
                            SHGUtilities.AddOrAppendHediffs(pawn, hediffToParts.severity, 0, hediffToParts.hediff);
                        }
                    }
                }
                if (extension.vanishingGene) pawn.genes.RemoveGene(gene);
            }
        }

        public static void HediffRemoving(Pawn pawn, Gene gene)
        {
            SHGExtension extension = gene.def.GetModExtension<SHGExtension>();
            if (extension != null && !extension.vanishingGene && !extension.hediffsToApply.NullOrEmpty())
            {
                SHGUtilities.RemoveHediffsFromParts(pawn, extension.hediffsToApply);
            }
        }
    }
}