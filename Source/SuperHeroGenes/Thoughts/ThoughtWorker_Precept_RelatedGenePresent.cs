﻿using System;
using Verse;
using RimWorld;

namespace SuperHeroGenesBase
{
    public class ThoughtWorker_Precept_RelatedGenePresent : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {
            if (!ModsConfig.BiotechActive || !ModsConfig.IdeologyActive || p.IsBloodfeeder())
            {
                return ThoughtState.Inactive;
            }
            SHGExtension extension = def.GetModExtension<SHGExtension>();
            if (!extension.checkNotPresent)
            {
                foreach (Pawn item in p.MapHeld.mapPawns.AllPawnsSpawned)
                {
                    if (HasRelatedGene(item, extension.relatedGene) && (item.IsPrisonerOfColony || item.IsSlaveOfColony || item.IsColonist))
                    {
                        return ThoughtState.ActiveDefault;
                    }
                }
                return ThoughtState.Inactive;
            }
            else
            {
                foreach (Pawn item in p.MapHeld.mapPawns.AllPawnsSpawned)
                {
                    if (!HasRelatedGene(item, extension.relatedGene) && (item.IsPrisonerOfColony || item.IsSlaveOfColony || item.IsColonist))
                    {
                        return ThoughtState.ActiveDefault;
                    }
                }
                return ThoughtState.Inactive;
            }
        }

        public static bool HasRelatedGene(Pawn pawn, GeneDef relatedGene)
        {
            if (!ModsConfig.BiotechActive || pawn.genes == null)
            {
                return false;
            }
            return SHGUtilities.HasRelatedGene(pawn, relatedGene);
        }
    }
}
