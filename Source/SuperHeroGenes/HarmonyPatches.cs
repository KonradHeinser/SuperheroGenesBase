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
            harmony.Patch(AccessTools.Method(typeof(EquipmentUtility), nameof(EquipmentUtility.CanEquip), new[] { typeof(Thing), typeof(Pawn), typeof(string).MakeByRefType(), typeof(bool) }), 
                postfix: new HarmonyMethod(patchType, nameof(CanEquipPostfix)));

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
        public static void CanEquipPostfix(ref bool __result, Thing thing, Pawn pawn, ref string cantReason)
        {
            SHGExtension extension = thing.def.GetModExtension<SHGExtension>();
            if (extension != null && __result)
            { // Attempt to get the various limiting lists
                List<GeneDef> requiredGenesToEquip = extension.requiredGenesToEquip;
                List<GeneDef> requireOneOfGenesToEquip = extension.requireOneOfGenesToEquip;
                List<GeneDef> forbiddenGenesToEquip = extension.forbiddenGenesToEquip;
                List<XenotypeDef> requireOneOfXenotypeToEquip = extension.requireOneOfXenotypeToEquip;
                List<XenotypeDef> forbiddenXenotypesToEquip = extension.forbiddenXenotypesToEquip;

                Pawn_GeneTracker currentGenes = pawn.genes;
                if (!requiredGenesToEquip.NullOrEmpty() || !requireOneOfGenesToEquip.NullOrEmpty() || !forbiddenGenesToEquip.NullOrEmpty() || !requireOneOfXenotypeToEquip.NullOrEmpty() || !forbiddenXenotypesToEquip.NullOrEmpty())
                {
                    bool flag = true;
                    if (!requireOneOfXenotypeToEquip.NullOrEmpty() && !requireOneOfXenotypeToEquip.Contains(pawn.genes.Xenotype) && flag)
                    {
                        cantReason = "SHG_XenoRestrictedEquipment_AnyOne".Translate();
                        flag = false;
                    }
                    if (!forbiddenXenotypesToEquip.NullOrEmpty() && forbiddenXenotypesToEquip.Contains(pawn.genes.Xenotype) && flag)
                    {
                        cantReason = "SHG_XenoRestrictedEquipment_None".Translate();
                        flag = false;
                    }
                    if (!forbiddenGenesToEquip.NullOrEmpty() && flag)
                    {
                        foreach (Gene gene in currentGenes.GenesListForReading)
                        {
                            if (forbiddenGenesToEquip.Contains(gene.def))
                            {
                                cantReason = "SHG_GeneRestrictedEquipment_None".Translate();
                                flag = false;
                                break;
                            }
                        }
                    }
                    if (!requiredGenesToEquip.NullOrEmpty() && flag)
                    {
                        foreach (Gene gene in currentGenes.GenesListForReading)
                        {
                            if (requiredGenesToEquip.Contains(gene.def))
                            {
                                requiredGenesToEquip.Remove(gene.def);
                            }
                        }
                        if (!requiredGenesToEquip.NullOrEmpty())
                        {
                            cantReason = "SHG_GeneRestrictedEquipment_All".Translate();
                            flag = false;
                        }
                    }
                    if (!requireOneOfGenesToEquip.NullOrEmpty() && flag)
                    {
                        flag = false;
                        cantReason = "SHG_GeneRestrictedEquipment_AnyOne".Translate();
                        foreach (Gene gene in currentGenes.GenesListForReading)
                        {
                            if (requiredGenesToEquip.Contains(gene.def))
                            {
                                flag = true;
                                cantReason = null;
                                break;
                            }
                        }
                    }
                    __result = flag;
                }
                else __result = true;
            }
        }
    }
}
