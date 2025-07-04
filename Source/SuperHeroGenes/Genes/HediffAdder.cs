using Verse;
using System.Collections.Generic;
using RimWorld;

namespace SuperHeroGenesBase
{
    public class HediffAdder : Gene
    {
        public List<AbilityDef> addedAbilities;
        public int cachedGeneCount = 0;

        public override void PostAdd()
        {
            base.PostAdd();
            HediffAdding(pawn, this);
            if (addedAbilities == null) addedAbilities = new List<AbilityDef>();
            if (!def.statOffsets.NullOrEmpty())
                foreach (var offset in def.statOffsets)
                    if (offset.stat == SHGDefOf.SHG_HemomancyProficiency)
                    {
                        pawn.genes?.GetFirstGeneOfType<Gene_Hemogen>()?.ResetMax();
                        break;
                    }
        }

        public override void Tick()
        {
            base.Tick();

            if (pawn.IsHashIntervalTick(200))
            {
                SHGExtension SHGextension = def.GetModExtension<SHGExtension>();
                if (SHGextension != null && !SHGextension.geneAbilities.NullOrEmpty() && pawn.genes.GenesListForReading.Count != cachedGeneCount)
                {
                    if (addedAbilities == null) addedAbilities = new List<AbilityDef>();
                    addedAbilities = SHGUtilities.AbilitiesWithCertainGenes(pawn, SHGextension.geneAbilities, addedAbilities);
                    cachedGeneCount = pawn.genes.GenesListForReading.Count;
                }
            }
        }

        public override void PostRemove()
        {
            base.PostRemove();
            SHGExtension SHGextension = def.GetModExtension<SHGExtension>();
            if (SHGextension?.noHediffRemoval != true)
                HediffRemoving(pawn, this);

            if (!addedAbilities.NullOrEmpty())
                SHGUtilities.RemovePawnAbilities(pawn, addedAbilities);
                
            if (!def.statOffsets.NullOrEmpty())
                foreach (var offset in def.statOffsets)
                    if (offset.stat == SHGDefOf.SHG_HemomancyProficiency)
                    {
                        pawn.genes?.GetFirstGeneOfType<Gene_Hemogen>()?.ResetMax();
                        break;
                    }
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

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref addedAbilities, "SHG_AddedAbilities");
        }
    }
}