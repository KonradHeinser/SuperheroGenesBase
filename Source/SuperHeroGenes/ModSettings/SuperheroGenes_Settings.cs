﻿using RimWorld;
using UnityEngine;
using Verse;
using System.Collections.Generic;

namespace SuperHeroGenesBase
{
    public class SuperheroGenes_Mod : Mod
    {
        internal static SuperheroGenes_Settings settings;

        public SuperheroGenes_Mod(ModContentPack content) : base(content)
        {
            settings = GetSettings<SuperheroGenes_Settings>();
        }

        public override string SettingsCategory()
        {
            return "SHG_ModMenu".Translate();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            settings.DoWindowContents(inRect);
        }

        public override void WriteSettings()
        {
            base.WriteSettings();
        }
    }

    public class SuperheroGenes_Settings : ModSettings
    {
        private static Vector2 scrollPosition = Vector2.zero;

        private static bool showMainOptions = true;
        private static bool showVillainOptions = true;
        private static bool showHeroOptions = true;
        private static bool showAutocastOptions = true;

        public static bool condensedMeteors = true;
        public static bool multipleArchetypes;
        public static bool multipleMutations;
        public static bool expensiveBase;
        public static bool supersEverywhere;
        public static bool archetypesEverywhere;
        public static bool activatableSuperGenes;
        public static bool interruptibleActivatables;
        public static bool middleGrounds;
        public static bool allGrounds;
        public static bool radiomancerOvercharge = true;
        public static bool superDrugNoTrader;
        public static bool superDrugNoReward;
        public static int baseAbilityCooldown;
        public static bool noPsionicNeurotrainers;
        public static bool antiSupeDisease;
        public static bool disableEvolvingHemomancers;

        // AI stuff
        public static bool poolUsage;
        public static bool automaticHealer;
        public static bool automaticDefense;
        public static bool automaticDefenseDrafted;
        public static bool automaticBuffs;
        public static bool automaticDebuffs;
        public static bool automaticDebuffsDrafted;
        public static bool automaticOffense;
        public static bool automaticOffenseDrafted;
        public static bool automaticFleeing;
        public static bool automaticRadioPurge;

        // Villains and Stereotypes stuff
        public static bool medievalVillains;
        public static bool vengefulOne = true;
        public static bool mutationsAnywhere;

        // Hero Organization stuff
        public static bool medievalHeroes;
        public static bool leagueGathering;
        public static bool radiantQuests = true;

        public static Dictionary<int, string> baseAbilityCooldownOptions = new Dictionary<int, string>()
        {
            { 0, "SHG_Disabled" },
            { 1, "SHG_OneHour" },
            { 2, "SHG_ThreeHours" },
            { 3, "SHG_SixHours" },
            { 4, "SHG_TwelveHours" },
            { 5, "SHG_OneDay" }
        };

        public SuperheroGenes_Settings() { }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref condensedMeteors, "condensedMeteors", true);
            Scribe_Values.Look(ref multipleMutations, "multipleMutations");
            Scribe_Values.Look(ref multipleArchetypes, "multipleArchetypes");
            Scribe_Values.Look(ref expensiveBase, "expensiveBase");
            Scribe_Values.Look(ref supersEverywhere, "supersEverywhere");
            Scribe_Values.Look(ref archetypesEverywhere, "archetypesEverywhere");
            Scribe_Values.Look(ref activatableSuperGenes, "activatableSuperGenes");
            Scribe_Values.Look(ref interruptibleActivatables, "interruptibleActivatables");
            Scribe_Values.Look(ref middleGrounds, "middleGrounds");
            Scribe_Values.Look(ref allGrounds, "allGrounds");
            Scribe_Values.Look(ref radiomancerOvercharge, "radiomancerOvercharge", true);
            Scribe_Values.Look(ref superDrugNoTrader, "superDrugNoTrader");
            Scribe_Values.Look(ref superDrugNoReward, "superDrugNoReward");
            Scribe_Values.Look(ref baseAbilityCooldown, "baseAbilityCooldown", 0);
            Scribe_Values.Look(ref noPsionicNeurotrainers, "noPsionicNeurotrainers", false);
            Scribe_Values.Look(ref antiSupeDisease, "antiSupeDisease", false);
            Scribe_Values.Look(ref disableEvolvingHemomancers, "disableEvolvingHemomancers", false);

            // AI stuff
            Scribe_Values.Look(ref poolUsage, "poolUsage");
            Scribe_Values.Look(ref automaticHealer, "automaticHealer");
            Scribe_Values.Look(ref automaticDefense, "automaticDefense");
            Scribe_Values.Look(ref automaticDefenseDrafted, "automaticDefenseDrafted");
            Scribe_Values.Look(ref automaticBuffs, "automaticBuffs");
            Scribe_Values.Look(ref automaticDebuffs, "automaticDebuffs");
            Scribe_Values.Look(ref automaticDebuffsDrafted, "automaticDebuffsDrafted");
            Scribe_Values.Look(ref automaticOffense, "automaticOffense");
            Scribe_Values.Look(ref automaticOffenseDrafted, "automaticOffenseDrafted");
            Scribe_Values.Look(ref automaticFleeing, "automaticFleeing");
            Scribe_Values.Look(ref automaticRadioPurge, "automaticRadioPurge");

            // Villains and Stereotypes stuff
            Scribe_Values.Look(ref medievalVillains, "medievalVillains");
            Scribe_Values.Look(ref vengefulOne, "vengefulOne", true);
            Scribe_Values.Look(ref mutationsAnywhere, "mutationsAnywhere");

            // Villains and Stereotypes stuff
            Scribe_Values.Look(ref medievalHeroes, "medievalHeroes");
            Scribe_Values.Look(ref leagueGathering, "leagueGathering");
            Scribe_Values.Look(ref radiantQuests, "radiantQuests", true);
        }

        public void DoWindowContents(Rect inRect)
        {
            Listing_Standard optionsMenu = new Listing_Standard();

            var scrollContainer = inRect.ContractedBy(10);
            scrollContainer.height -= optionsMenu.CurHeight;
            scrollContainer.y += optionsMenu.CurHeight;
            Widgets.DrawBoxSolid(scrollContainer, Color.grey);
            var innerContainer = scrollContainer.ContractedBy(1);
            Widgets.DrawBoxSolid(scrollContainer, new ColorInt(37, 37, 37).ToColor);
            var frameRect = innerContainer.ContractedBy(5);
            frameRect.y += 15;
            frameRect.height -= 15;
            var contentRect = frameRect;
            contentRect.x = 0;
            contentRect.y = 0;
            contentRect.width -= 20;

            int numberOfOptions = 4; // One for each section
            if (showMainOptions) numberOfOptions += 16;
            if (ModsConfig.IsActive("SuperheroGenes.Villains") && showVillainOptions) numberOfOptions += 3;
            if (ModsConfig.IsActive("SuperheroGenes.Heroes") && showHeroOptions) numberOfOptions += 3;
            if (showAutocastOptions) numberOfOptions += 11;
            contentRect.height = numberOfOptions * 35; // To avoid weird white space, height is based off of option count of present mods

            Widgets.BeginScrollView(frameRect, ref scrollPosition, contentRect, true);

            optionsMenu.Begin(contentRect.AtZero());

            optionsMenu.CheckboxLabeled("SHG_ModName".Translate(), ref showMainOptions, "SHG_ModDescription".Translate());
            optionsMenu.Gap(7f);
            if (showMainOptions)
            {
                optionsMenu.CheckboxLabeled("SHG_CondensedMeteors".Translate(), ref condensedMeteors, "SHG_CondensedMeteorsDescription".Translate());
                optionsMenu.Gap(10f);
                optionsMenu.CheckboxLabeled("SHG_MultipleMutations".Translate(), ref multipleMutations, "SHG_MultipleMutationsDescription".Translate());
                optionsMenu.Gap(10f);
                optionsMenu.CheckboxLabeled("SHG_MultipleArchetypes".Translate(), ref multipleArchetypes, "SHG_MultipleArchetypesDescription".Translate());
                optionsMenu.Gap(10f);
                optionsMenu.CheckboxLabeled("SHG_ExpensiveBase".Translate(), ref expensiveBase, "SHG_ExpensiveBaseDescription".Translate());
                optionsMenu.Gap(10f);
                optionsMenu.CheckboxLabeled("SHG_SupersEverywhere".Translate(), ref supersEverywhere, "SHG_SupersEverywhereDescription".Translate());
                optionsMenu.Gap(10f);
                if (supersEverywhere)
                {
                    optionsMenu.CheckboxLabeled("SHG_ArchetypesEverywhere".Translate(), ref archetypesEverywhere, "SHG_ArchetypesEverywhereDescription".Translate());
                    optionsMenu.Gap(10f);
                }
                optionsMenu.CheckboxLabeled("SHG_ActivatableSuperGenes".Translate(), ref activatableSuperGenes, "SHG_ActivatableSuperGenesDescription".Translate());
                optionsMenu.Gap(10f);
                if (activatableSuperGenes)
                {
                    optionsMenu.CheckboxLabeled("SHG_InterruptibleActivatables".Translate(), ref interruptibleActivatables, "SHG_InterruptibleActivatablesDescription".Translate());
                    optionsMenu.Gap(10f);
                }
                optionsMenu.CheckboxLabeled("SHG_MiddleGrounds".Translate(), ref middleGrounds, "SHG_MiddleGroundsDescription".Translate());
                optionsMenu.Gap(10f);
                if (middleGrounds)
                {
                    optionsMenu.CheckboxLabeled("SHG_AllGrounds".Translate(), ref allGrounds, "SHG_AllGroundsDescription".Translate());
                    optionsMenu.Gap(10f);
                }
                optionsMenu.CheckboxLabeled("SHG_RadiomancerOvercharge".Translate(), ref radiomancerOvercharge, "SHG_RadiomancerOverchargeDescription".Translate());
                optionsMenu.Gap(10f);
                optionsMenu.CheckboxLabeled("SHG_SuperDrugs_NoTrader".Translate(), ref superDrugNoTrader, "SHG_SuperDrugs_NoTraderDescription".Translate());
                optionsMenu.Gap(10f);
                optionsMenu.CheckboxLabeled("SHG_SuperDrugs_NoReward".Translate(), ref superDrugNoReward, "SHG_SuperDrugs_NoRewardDescription".Translate());
                optionsMenu.Gap(10f);
                optionsMenu.CheckboxLabeled("SHG_AntiSupeDisease".Translate(), ref antiSupeDisease, "SHG_AntiSupeDiseaseDescription".Translate());
                optionsMenu.Gap(10f);
                optionsMenu.CheckboxLabeled("SHG_DisableEvolvingHemomancers".Translate(), ref disableEvolvingHemomancers, "SHG_DisableEvolvingHemomancersDescription".Translate());
                optionsMenu.Gap(10f);
                if (optionsMenu.ButtonTextLabeledPct("SHG_BaseAbilityCooldown".Translate(), baseAbilityCooldownOptions[baseAbilityCooldown].Translate(), 0.75f,
                    tooltip: "SHG_BaseAbilityCooldownDesc".Translate()))
                {
                    List<FloatMenuOption> options = new List<FloatMenuOption>();
                    foreach (var option in baseAbilityCooldownOptions)
                    {
                        options.Add(new FloatMenuOption(option.Value.Translate(), delegate
                        {
                            baseAbilityCooldown = option.Key;
                        }));
                    }
                    Find.WindowStack.Add(new FloatMenu(options));
                }
                /* Harmony patch can't run, so this is shelved for now
                if (ModsConfig.RoyaltyActive)
                {
                    optionsMenu.CheckboxLabeled("SHG_NoPsionicNeurotrainers".Translate(), ref noPsionicNeurotrainers, "SHG_NoPsionicNeurotrainersDescription".Translate());
                    optionsMenu.Gap(10f);
                }
                */
            }

            if (ModsConfig.IsActive("SuperheroGenes.Villains"))
            {
                optionsMenu.Gap(10f);
                optionsMenu.CheckboxLabeled("SHG_Villains_ModName".Translate(), ref showVillainOptions, "SHG_Villains_ModDescription".Translate());
                optionsMenu.Gap(7f);
                if (showVillainOptions)
                {
                    optionsMenu.CheckboxLabeled("SHG_MedievalVillains".Translate(), ref medievalVillains, "SHG_MedievalVillainsDescription".Translate());
                    optionsMenu.Gap(10f);
                    optionsMenu.CheckboxLabeled("SHG_VengefulOne".Translate(), ref vengefulOne, "SHG_VengefulOneDescription".Translate());
                    optionsMenu.Gap(10f);
                    if (supersEverywhere)
                    {
                        optionsMenu.CheckboxLabeled("SHG_MutationsAnywhere".Translate(), ref mutationsAnywhere, "SHG_MutationsAnywhereDescription".Translate());
                        optionsMenu.Gap(10f);
                    }
                }
            }

            if (ModsConfig.IsActive("SuperheroGenes.Heroes"))
            {
                optionsMenu.Gap(10f);
                optionsMenu.CheckboxLabeled("SHG_Heroes_ModName".Translate(), ref showHeroOptions, "SHG_Heroes_ModDescription".Translate());
                optionsMenu.Gap(7f);
                if (showHeroOptions)
                {
                    optionsMenu.CheckboxLabeled("SHG_MedievalHeroes".Translate(), ref medievalHeroes, "SHG_MedievalHeroesDescription".Translate());
                    optionsMenu.Gap(10f);
                    optionsMenu.CheckboxLabeled("SHG_LeagueGathering".Translate(), ref leagueGathering, "SHG_LeagueGatheringDescription".Translate());
                    optionsMenu.Gap(10f);
                    optionsMenu.CheckboxLabeled("SHG_RadiantQuests".Translate(), ref radiantQuests, "SHG_RadiantQuestsDescription".Translate());
                    optionsMenu.Gap(10f);
                }
            }

            optionsMenu.Gap(10f);
            optionsMenu.CheckboxLabeled("SHG_SuperAI".Translate(), ref showAutocastOptions, "SHG_SuperAIDescription".Translate());
            optionsMenu.Gap(7f);
            if (showAutocastOptions)
            {
                optionsMenu.CheckboxLabeled("SHG_AllowPoolUsage".Translate(), ref poolUsage, "SHG_AllowPoolUsageDescription".Translate());
                optionsMenu.Gap(10f);
                optionsMenu.CheckboxLabeled("SHG_AutomaticHealer".Translate(), ref automaticHealer, "SHG_AutomaticHealerDescription".Translate());
                optionsMenu.Gap(10f);
                optionsMenu.CheckboxLabeled("SHG_AutomaticDefense".Translate(), ref automaticDefense, "SHG_AutomaticDefenseDescription".Translate());
                optionsMenu.Gap(10f);
                if (automaticDefense)
                {
                    optionsMenu.CheckboxLabeled("SHG_AutomaticDefenseDrafted".Translate(), ref automaticDefenseDrafted, "SHG_AutomaticDefenseDraftedDescription".Translate());
                    optionsMenu.Gap(10f);
                }
                optionsMenu.CheckboxLabeled("SHG_AutomaticBuffs".Translate(), ref automaticBuffs, "SHG_AutomaticBuffsDescription".Translate());
                optionsMenu.Gap(10f);
                optionsMenu.CheckboxLabeled("SHG_AutomaticDebuff".Translate(), ref automaticDebuffs, "SHG_AutomaticDebuffDescription".Translate());
                optionsMenu.Gap(10f);
                if (automaticDebuffs)
                {
                    optionsMenu.CheckboxLabeled("SHG_AutomaticDebuffDrafted".Translate(), ref automaticDebuffsDrafted, "SHG_AutomaticDebuffDraftedDescription".Translate());
                    optionsMenu.Gap(10f);
                }
                optionsMenu.CheckboxLabeled("SHG_AutomaticOffense".Translate(), ref automaticOffense, "SHG_AutomaticOffenseDescription".Translate());
                optionsMenu.Gap(10f);
                if (automaticOffense)
                {
                    optionsMenu.CheckboxLabeled("SHG_AutomaticOffenseDrafted".Translate(), ref automaticOffenseDrafted, "SHG_AutomaticOffenseDraftedDescription".Translate());
                    optionsMenu.Gap(10f);
                }
                optionsMenu.CheckboxLabeled("SHG_AutomaticFleeing".Translate(), ref automaticFleeing, "SHG_AutomaticFleeingDescription".Translate());
                optionsMenu.Gap(10f);
                optionsMenu.CheckboxLabeled("SHG_AutoRadioPurge".Translate(), ref automaticRadioPurge, "SHG_AutoRadioPurgeDescription".Translate());
                optionsMenu.Gap(10f);
            }

            optionsMenu.End();
            Widgets.EndScrollView();
            base.Write();
        }
    }
}
