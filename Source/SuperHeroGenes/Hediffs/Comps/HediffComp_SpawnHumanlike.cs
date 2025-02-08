using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace SuperHeroGenesBase
{
    public class HediffComp_SpawnHumanlike : HediffComp
    {
        HediffCompProperties_SpawnHumanlike Props => (HediffCompProperties_SpawnHumanlike)props;

        public List<GeneDef> genes;

        public List<GeneDef> Genes
        {
            get
            {
                if (genes.NullOrEmpty())
                    genes = PregnancyUtility.GetInheritedGenes(null, Pawn);
                return genes;
            }
        }

        public override void CompPostPostRemoved()
        {
            if (!Pawn.Dead)
                SpawnPawns();
        }

        public void SpawnPawns()
        {
            Map map = Pawn.MapHeld;
            Caravan caravan = Pawn.GetCaravan();
            if (map == null && caravan == null) return;

            int numberToSpawn = Props.spawnCount.RandomInRange;
            List<IntVec3> alreadyUsedSpots = new List<IntVec3>();

            float fixedAge = 0f;

            switch (Props.developmentalStage)
            {
                case DevelopmentalStage.Adult:
                    fixedAge = 18f;
                    break;
                case DevelopmentalStage.Child:
                    fixedAge = 8f;
                    break;
            }

            for (int i = 0; i < numberToSpawn; i++)
            {
                // If the faction is somehow null, the child will default to joining the player
                PawnGenerationRequest request = new PawnGenerationRequest(Pawn.kindDef ?? PawnKindDefOf.Colonist, Pawn.Faction,
                    fixedLastName: PawnNamingUtility.GetLastName(Pawn), allowDowned: true, forceNoIdeo: true, fixedBiologicalAge: fixedAge,
                    fixedChronologicalAge: fixedAge, forcedXenotype: XenotypeDefOf.Baseliner, developmentalStages: Props.developmentalStage)
                {
                    DontGivePreArrivalPathway = true
                };

                request.ForcedEndogenes = Genes;

                Pawn pawn = PawnGenerator.GeneratePawn(request);

                pawn.genes.xenotypeName = Pawn.genes.xenotypeName;
                pawn.genes.iconDef = Pawn.genes.iconDef;

                if (map != null)
                {
                    IntVec3? intVec = null;

                    if (Pawn.Position.Walkable(map) && (alreadyUsedSpots.NullOrEmpty() || !alreadyUsedSpots.Contains(Pawn.Position)))
                    {
                        intVec = Pawn.Position;
                        alreadyUsedSpots.Add(Pawn.Position);
                    }
                    else intVec = CellFinder.RandomClosewalkCellNear(Pawn.Position, map, 1, delegate (IntVec3 cell)
                    {
                        if (!alreadyUsedSpots.NullOrEmpty() && alreadyUsedSpots.Contains(cell)) return false;
                        if (cell != Pawn.Position)
                        {
                            Building building = map.edificeGrid[cell];
                            if (building == null)
                            {
                                alreadyUsedSpots.Add(cell);
                                return true;
                            }

                            if (building.def?.IsBed != true) alreadyUsedSpots.Add(cell);
                            return building.def?.IsBed != true;
                        }
                        return false;
                    });
                    if (Props.filthOnCompletion != null) FilthMaker.TryMakeFilth(intVec.Value, map, ThingDefOf.Filth_AmnioticFluid, Props.filthPerSpawn.RandomInRange);

                    if (pawn.RaceProps.IsFlesh)
                        pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, Pawn);

                    if (pawn.playerSettings != null && Pawn.playerSettings != null)
                        pawn.playerSettings.AreaRestrictionInPawnCurrentMap = Pawn.playerSettings.AreaRestrictionInPawnCurrentMap;

                    if (!PawnUtility.TrySpawnHatchedOrBornPawn(pawn, Pawn, intVec))
                        Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
                }
                else caravan.AddPawn(pawn, true);

                if (pawn.Faction == Faction.OfPlayer)
                {
                    pawn.babyNamingDeadline = Find.TickManager.TicksGame + 60000;
                    ChoiceLetter_BabyBirth birthLetter = (ChoiceLetter_BabyBirth)LetterMaker.MakeLetter("SHG_CompSpawnPawn".Translate(pawn.Label, Props.letterLabelNote.Translate()),
                        "SHG_CompSpawnPawnHediffText".Translate(Pawn.Label, Props.letterTextPawnDescription.Translate()), LetterDefOf.BabyBirth, pawn);
                    birthLetter.Start();
                    Find.LetterStack.ReceiveLetter(birthLetter);
                }

                pawn.caller?.DoCall();
            }
        }
    }
}
