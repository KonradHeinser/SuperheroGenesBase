using Verse;
using System.Collections.Generic;

namespace SuperHeroGenesBase
{
    public class HediffAdder : Gene
    {
        public override void PostAdd()
        {
            base.PostAdd();
            SHGExtension extension = def.GetModExtension<SHGExtension>();
            if (extension != null && !extension.hediffsToApply.NullOrEmpty())
            {
                foreach (HediffsToParts hediffToParts in extension.hediffsToApply)
                {
                    if (!hediffToParts.bodyParts.NullOrEmpty())
                    {
                        List<BodyPartRecord> alreadyTargettedParts = new List<BodyPartRecord>();
                        foreach (BodyPartDef bodyPartDef in hediffToParts.bodyParts)
                        {
                            Hediff firstHediffOfDef = null;
                            BodyPartRecord bodyPart = null;
                            foreach (BodyPartRecord notMissingPart in pawn.health.hediffSet.GetNotMissingParts())
                            {
                                if (notMissingPart.def == bodyPartDef && (alreadyTargettedParts.NullOrEmpty() || !alreadyTargettedParts.Contains(notMissingPart)))
                                {
                                    alreadyTargettedParts.Add(notMissingPart);
                                    bodyPart = notMissingPart;
                                    break;
                                }
                            }
                            if (bodyPart == null) continue; // If no part is found, just "continue" down the list
                            foreach (Hediff hediff in pawn.health.hediffSet.hediffs) // Go through all the hediffs to try to find the hediff on the specified part
                            {
                                if (hediff.Part == bodyPart && hediff.def == hediffToParts.hediff) firstHediffOfDef = hediff;
                                break;
                            }

                            if (firstHediffOfDef != null)
                            {
                                if (hediffToParts.onlyIfNew) continue;
                                firstHediffOfDef.Severity += hediffToParts.severity;
                            }
                            else
                            {
                                pawn.health.AddHediff(hediffToParts.hediff, bodyPart);
                                foreach (Hediff hediff in pawn.health.hediffSet.hediffs) // Go through all the hediffs to try to find the hediff on the specified part
                                {
                                    if (hediff.Part == bodyPart && hediff.def == hediffToParts.hediff) firstHediffOfDef = hediff;
                                    break;
                                }
                                firstHediffOfDef.Severity = hediffToParts.severity;
                            }
                        }
                    }
                    else
                    {
                        if (pawn.health.hediffSet?.HasHediff(hediffToParts.hediff) == true)
                        {
                            if (hediffToParts.onlyIfNew) continue;
                            Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(hediffToParts.hediff);
                            hediff.Severity += hediffToParts.severity;
                        }
                        else
                        {
                            pawn.health.AddHediff(hediffToParts.hediff);
                            Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(hediffToParts.hediff);
                            hediff.Severity = hediffToParts.severity;
                        }
                    }
                }
                if (extension.vanishingGene) pawn.genes.RemoveGene(this);
            }
            else
            {
                Log.Error(def + " could not find the hediffs to add list.");
            }
        }
    }
}