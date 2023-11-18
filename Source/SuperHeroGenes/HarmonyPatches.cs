using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {
        private static readonly Type patchType = typeof(HarmonyPatches);

        static HarmonyPatches()
        {
            Harmony harmony = new Harmony("Rimworld.Alite.SHG.main");
            harmony.Patch(AccessTools.Method(typeof(CompAbilityEffect_ReimplantXenogerm), nameof(CompAbilityEffect_ReimplantXenogerm.PawnIdeoCanAcceptReimplant)),
                          postfix: new HarmonyMethod(patchType, nameof(PawnIdeoCanAcceptReimplantPostfix)));
            harmony.Patch(AccessTools.Method(typeof(Xenogerm), nameof(Xenogerm.PawnIdeoDisallowsImplanting)),
                          postfix: new HarmonyMethod(patchType, nameof(PawnIdeoDisallowsImplantingPostFix)));
        }

        public static void PawnIdeoCanAcceptReimplantPostfix(ref bool __result, Pawn implanter, Pawn implantee)
        {
            if (!__result) __result = false;

            else if (!IdeoUtility.DoerWillingToDo(SHGDefOf.SHG_PropagateSuperGene, implantee) && implanter.genes.GenesListForReading.Any((Gene x) => x.def == SHGDefOf.SuperHeroBase))
            {
                __result = false;
            }

            else
            {
                __result = true;
            }
        }

        public static void PawnIdeoDisallowsImplantingPostFix(ref bool __result, Pawn selPawn, ref Xenogerm __instance)
        {
            if (__result) __result = true;

            else if (!IdeoUtility.DoerWillingToDo(SHGDefOf.SHG_PropagateSuperGene, selPawn) && __instance.GeneSet.GenesListForReading.Any((GeneDef x) => x == SHGDefOf.SuperHeroBase))
            {
                __result = true;
            }
            else
            {
                __result = false;
            }
        }
    }
}
