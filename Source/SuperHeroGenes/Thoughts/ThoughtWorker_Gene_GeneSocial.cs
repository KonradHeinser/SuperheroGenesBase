﻿using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class ThoughtWorker_Gene_GeneSocial : ThoughtWorker
    {
        protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn otherPawn)
        {
            if (def.gender != 0 && otherPawn.gender != def.gender || otherPawn.genes == null || !ModsConfig.BiotechActive)
            {
                return ThoughtState.Inactive;
            }

            if (RelationsUtility.PawnsKnowEachOther(p, otherPawn))
            {
                SHGExtension extension = def.GetModExtension<SHGExtension>();

                if (!extension.nullifyingGenes.NullOrEmpty())
                {
                    foreach (Gene gene in p.genes.GenesListForReading)
                    {
                        if (extension.nullifyingGenes.Contains(gene.def)) return ThoughtState.Inactive;
                    }
                }
                if (!extension.requiredGenes.NullOrEmpty())
                {
                    bool flag = true;
                    foreach (Gene gene in p.genes.GenesListForReading)
                    {
                        if (extension.requiredGenes.Contains(gene.def))
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag) return ThoughtState.Inactive;
                }
                if (!extension.compoundingHatred)
                {
                    if (!extension.checkedGenes.NullOrEmpty())
                    {
                        foreach (Gene gene in otherPawn.genes.GenesListForReading)
                        {
                            if (extension.checkedGenes.Contains(gene.def)) return ThoughtState.ActiveAtStage(0);
                        }
                    }
                    else
                    {
                        Log.Error(def + " doesn't have any checked genes, meaning it will always be inactive");
                    }
                }
                else
                {
                    int num = 0;
                    if (!extension.checkedGenes.NullOrEmpty())
                    {
                        foreach (Gene gene in otherPawn.genes.GenesListForReading)
                        {
                            if (extension.checkedGenes.Contains(gene.def)) num++;
                            if (num >= def.stages.Count) return ThoughtState.ActiveAtStage(def.stages.Count - 1);
                        }
                        return ThoughtState.ActiveAtStage(num - 1);
                    }
                    else
                    {
                        Log.Error(def + " doesn't have any checked genes, meaning it will always be inactive");
                    }
                }
            }

            return ThoughtState.Inactive;
        }
    }
}
