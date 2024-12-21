﻿using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using RimWorld;

namespace SuperHeroGenesBase
{
    public class IncidentWorker_DiseaseGene : IncidentWorker_Disease
    {
        protected override IEnumerable<Pawn> PotentialVictimCandidates(IIncidentTarget target)
        {
            if (target is Map map)
            {
                return map.mapPawns.FreeColonistsAndPrisoners.Where((Pawn x) => (x.ParentHolder == null || !(x.ParentHolder is CompBiosculpterPod)) && SHGUtilities.PawnHasAnyOfGenes(x, def.GetModExtension<SHGExtension>().checkedGenes));
            }
            return ((Caravan)target).PawnsListForReading.Where((Pawn x) => x.IsFreeColonist || x.IsPrisonerOfColony);
        }

        protected override IEnumerable<Pawn> ActualVictims(IncidentParms parms)
        {
            int num = PotentialVictimCandidates(parms.target).Count();
            int randomInRange = new IntRange(Mathf.RoundToInt((float)num * def.diseaseVictimFractionRange.min), Mathf.RoundToInt((float)num * def.diseaseVictimFractionRange.max)).RandomInRange;
            randomInRange = Mathf.Clamp(randomInRange, 1, def.diseaseMaxVictims);
            return PotentialVictims(parms.target).InRandomOrder().Take(randomInRange);
        }
    }
}
