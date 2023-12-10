﻿using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace SuperHeroGenesBase
{
    public class JobGiver_GetResourcePack : ThinkNode_JobGiver
    {
        private static float? cachedResourcePackResourceGain;

        public static float ResourcePackResourceGain(Pawn pawn, List<ThingDef> resourcePacks)
        {
            if (!cachedResourcePackResourceGain.HasValue)
            {
                if (!ModsConfig.BiotechActive)
                {
                    cachedResourcePackResourceGain = 0f;
                }
                else
                {
                    Thing carriedThing = pawn.carryTracker.CarriedThing;
                    foreach (ThingDef thing in resourcePacks)
                    {

                        if (!(thing.ingestible?.outcomeDoers?.FirstOrDefault((IngestionOutcomeDoer x) => x is IngestionOutcomeDoer_OffsetResource) is IngestionOutcomeDoer_OffsetResource ingestionOutcomeDoer_OffsetResource))
                        {
                            cachedResourcePackResourceGain = 0f;
                        }
                        else
                        {
                            cachedResourcePackResourceGain = ingestionOutcomeDoer_OffsetResource.offset;
                        }

                            // Checks if inventory has the thing because if so it'll be the default anyway
                        if (carriedThing != null && carriedThing.def == thing)
                        {
                            return cachedResourcePackResourceGain.Value;
                        }
                        for (int i = 0; i < pawn.inventory.innerContainer.Count; i++)
                        {
                            if (pawn.inventory.innerContainer[i].def == thing)
                            {
                                return cachedResourcePackResourceGain.Value;
                            }
                        }
                    }
                }
            }
            return cachedResourcePackResourceGain.Value; // If never found an item in the inventory, return the last calculated value
        }

        public static void ResetStaticData()
        {
            cachedResourcePackResourceGain = null;
        }

        public override float GetPriority(Pawn pawn)
        {
            if (!ModsConfig.BiotechActive)
            {
                return 0f;
            }
            if (pawn.genes?.GetFirstGeneOfType<ResourceGene>() == null)
            {
                return 0f;
            }
            return 9.1f;
        }

        protected override Job TryGiveJob(Pawn pawn)
        {
            if (!ModsConfig.BiotechActive) return null;
            ResourceGene resourceGene = pawn.genes?.GetFirstGeneOfType<ResourceGene>(); // Verifies that any resource gene exists
            if (resourceGene == null)
            {
                return null;
            }

            List<ResourceGene> resourcesPresent = new List<ResourceGene>(); // Creates list of all resource genes
            foreach (Gene gene in pawn.genes?.GenesListForReading)
            {
                if (gene.def.HasModExtension<DRGExtension>() && gene.def.GetModExtension<DRGExtension>().isMainGene) resourcesPresent.Add((ResourceGene)gene);
            }
            foreach (ResourceGene resource in resourcesPresent) // Check each resource gene, and the moment a viable one appears, return job
            {
                if (!resource.ShouldConsumeResourceNow())
                {
                    continue;
                }
                DRGExtension extension = resource.def.GetModExtension<DRGExtension>();
                if (resource.resourcePacksAllowed && !extension.resourcePacks.NullOrEmpty())
                {
                    int num = Mathf.FloorToInt((resource.Max - resource.Value) / ResourcePackResourceGain(pawn, extension.resourcePacks));
                    if (num > 0)
                    {
                        Thing resourcePack = GetResourcePack(pawn, extension.resourcePacks);
                        if (resourcePack != null)
                        {
                            Job job = JobMaker.MakeJob(JobDefOf.Ingest, resourcePack);
                            job.count = Mathf.Min(resourcePack.stackCount, num);
                            job.ingestTotalCount = true;
                            return job;
                        }
                    }
                }
            }
            return null;
        }

        private Thing GetResourcePack(Pawn pawn, List<ThingDef> resourcePacks)
        {
            Thing carriedThing = pawn.carryTracker.CarriedThing;
            foreach (ThingDef thingDef in resourcePacks)
            {
                if (carriedThing != null && carriedThing.def == thingDef)
                {
                    return carriedThing;
                }
                for (int i = 0; i < pawn.inventory.innerContainer.Count; i++)
                {
                    if (pawn.inventory.innerContainer[i].def == thingDef)
                    {
                        return pawn.inventory.innerContainer[i];
                    }
                }
            }
            List<Thing> resourcepackThings = new List<Thing>();
            foreach (ThingDef thing in resourcePacks) resourcepackThings.Add(ThingMaker.MakeThing(thing));
            return GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, resourcepackThings.AsEnumerable(), PathEndMode.OnCell, TraverseParms.For(pawn), 9999f, (Thing t) => pawn.CanReserve(t) && !t.IsForbidden(pawn));
        }

        public static AcceptanceReport CanFeedOnPrisoner(Pawn bloodfeeder, Pawn prisoner)
        {
            if (prisoner.WouldDieFromAdditionalBloodLoss(0.4499f))
            {
                return "CannotFeedOnWouldKill".Translate(prisoner.Named("PAWN"));
            }
            if (!prisoner.IsPrisonerOfColony || !prisoner.guest.PrisonerIsSecure || prisoner.guest.interactionMode != PrisonerInteractionModeDefOf.Bloodfeed || prisoner.IsForbidden(bloodfeeder) || !bloodfeeder.CanReserveAndReach(prisoner, PathEndMode.OnCell, bloodfeeder.NormalMaxDanger()) || prisoner.InAggroMentalState)
            {
                return false;
            }
            return true;
        }

        private Pawn GetPrisoner(Pawn pawn)
        {
            return (Pawn)GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, pawn.Map.mapPawns.PrisonersOfColonySpawned, PathEndMode.OnCell, TraverseParms.For(pawn), 9999f, (Thing t) => t is Pawn prisoner && CanFeedOnPrisoner(pawn, prisoner).Accepted);
        }
    }
}
