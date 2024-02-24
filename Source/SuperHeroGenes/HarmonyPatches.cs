﻿using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;
using System.Text;
using UnityEngine;

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
            harmony.Patch(AccessTools.Method(typeof(Pawn_RelationsTracker), nameof(Pawn_RelationsTracker.SecondaryLovinChanceFactor)),
                          postfix: new HarmonyMethod(patchType, nameof(SecondaryLovinChanceFactorPostFix)));
            harmony.Patch(AccessTools.Method(typeof(InteractionWorker_RomanceAttempt), nameof(InteractionWorker_RomanceAttempt.RomanceFactors)),
                          postfix: new HarmonyMethod(patchType, nameof(RomanceFactorsPostFix)));
            harmony.Patch(AccessTools.PropertyGetter(typeof(Gene_Hemogen), nameof(Gene_Hemogen.InitialResourceMax)),
                          postfix: new HarmonyMethod(patchType, nameof(HemomancerHemogenMaxPostFix)));
            harmony.Patch(AccessTools.Method(typeof(Thing), nameof(Thing.TakeDamage)),
                prefix: new HarmonyMethod(patchType, nameof(TakeDamagePrefix)));
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

        public static void SecondaryLovinChanceFactorPostFix(ref float __result, Pawn otherPawn, ref Pawn ___pawn)
        {
            if (ModsConfig.BiotechActive && otherPawn.genes != null)
            {
                List<Gene> genesListForReading = otherPawn.genes.GenesListForReading;
                foreach (Gene gene in genesListForReading)
                {
                    if (gene.def.HasModExtension<GRCExtension>())
                    {
                        float num = 1f;
                        GRCExtension extension = gene.def.GetModExtension<GRCExtension>();
                        if (extension.carrierStat != null)
                        {
                            float statValue = otherPawn.GetStatValue(extension.carrierStat);
                            if (extension.onlyWhileLoweredCarrier && statValue < 1) num *= statValue;
                            else if (extension.onlyWhileRaisedCarrier && statValue > 1) num *= statValue;
                            else if (!extension.onlyWhileLoweredCarrier && !extension.onlyWhileRaisedCarrier) num *= statValue;
                        }
                        if (extension.otherStat != null)
                        {
                            float statValue = ___pawn.GetStatValue(extension.otherStat);
                            if (extension.onlyWhileLoweredOther && statValue < 1) num *= statValue;
                            else if (extension.onlyWhileRaisedOther && statValue > 1) num *= statValue;
                            else if (!extension.onlyWhileLoweredOther && !extension.onlyWhileRaisedOther) num *= statValue;
                        }
                        __result *= num;
                    }
                }
            }
        }

        public static void RomanceFactorsPostFix(ref string __result, Pawn romancer, Pawn romanceTarget)
        {
            if (ModsConfig.BiotechActive && romancer.genes != null)
            {
                List<Gene> genesListForReading = romancer.genes.GenesListForReading;
                float num = 1f;
                bool flag = false;
                foreach (Gene gene in genesListForReading)
                {
                    if (gene.def.HasModExtension<GRCExtension>())
                    {
                        GRCExtension extension = gene.def.GetModExtension<GRCExtension>();
                        if (extension.carrierStat != null)
                        {
                            float statValue = romancer.GetStatValue(extension.carrierStat);
                            if (extension.onlyWhileLoweredCarrier && statValue < 1) num *= statValue;
                            else if (extension.onlyWhileRaisedCarrier && statValue > 1) num *= statValue;
                            else if (!extension.onlyWhileLoweredCarrier && !extension.onlyWhileRaisedCarrier) num *= statValue;
                        }
                        if (extension.otherStat != null)
                        {
                            float statValue = romanceTarget.GetStatValue(extension.otherStat);
                            if (extension.onlyWhileLoweredOther && statValue < 1) num *= statValue;
                            else if (extension.onlyWhileRaisedOther && statValue > 1) num *= statValue;
                            else if (!extension.onlyWhileLoweredOther && !extension.onlyWhileRaisedOther) num *= statValue;
                        }
                        flag = true;
                    }
                }
                if (flag)
                {
                    StringBuilder stringBuilder = new StringBuilder(__result);
                    stringBuilder.AppendLine(" - " + "SHG_GeneticRomanceChance".Translate() + ": x" + num.ToStringPercent());
                    __result = stringBuilder.ToString();
                }
            }
        }

        public static void HemomancerHemogenMaxPostFix(ref float __result, ref Pawn ___pawn)
        {
            float hemomancyProficiency = ___pawn.GetStatValue(SHGDefOf.SHG_HemomancyProficiency);
            __result += hemomancyProficiency;
        }

        public static bool HasSpecialExplosion(Pawn pawn)
        {
            if (!pawn.Dead && pawn.health != null && !pawn.health.hediffSet.hediffs.NullOrEmpty())
            {
                foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
                {
                    HediffComp_ExplodingMeleeAttacks meleeExplodingComp = hediff.TryGetComp<HediffComp_ExplodingMeleeAttacks>();
                    if (meleeExplodingComp != null) return true;
                }
            }

            return false;
        }

        public static bool DoingSpecialExplosion(Pawn pawn, DamageInfo dinfo, Thing mainTarget)
        {
            if (pawn.health.hediffSet != null)
            {
                foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
                {
                    HediffComp_ExplodingMeleeAttacks meleeExplodingComp = hediff.TryGetComp<HediffComp_ExplodingMeleeAttacks>();
                    if (meleeExplodingComp != null)
                    {
                        if (dinfo.Def == meleeExplodingComp.Props.damageDef && meleeExplodingComp.currentlyExploding) return true;
                    }
                }
            }
            return false;
        }

        public static bool TakeDamagePrefix(DamageInfo dinfo, Thing __instance, DamageWorker.DamageResult __result)
        {
            if (dinfo.Instigator != null && dinfo.Instigator is Pawn pawn)
            {
                if (HasSpecialExplosion(pawn) && !DoingSpecialExplosion(pawn, dinfo, __instance))
                {
                    if (SHGUtilities.GetCurrentTarget(pawn, false) == __instance)
                    {
                        foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
                        {
                            if (!dinfo.Def.isExplosive && !dinfo.Def.isRanged)
                            {
                                Log.Message("Exploding");
                                HediffComp_ExplodingMeleeAttacks meleeExplodingComp = hediff.TryGetComp<HediffComp_ExplodingMeleeAttacks>();
                                if (meleeExplodingComp != null)
                                {
                                    meleeExplodingComp.currentlyExploding = true;
                                    meleeExplodingComp.DoExplosion(__instance.Position);
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}
