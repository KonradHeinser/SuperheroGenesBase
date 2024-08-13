using RimWorld;
using UnityEngine;
using Verse;
using System;
using System.Collections.Generic;
using System.Linq;


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
            return "SHG_ModName".Translate();
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
        public static bool multipleArchetypes = false;
        public static bool multipleMutations = false;
        public static bool expensiveBase = false;
        public static bool supersEverywhere = false;
        public static bool activatableSuperGenes = false;
        public static bool interruptibleActivatables = false;
        public static bool middleGrounds = false;
        public static bool allGrounds = false;
        public static bool radiomancerOvercharge = true;
        public static bool superDrugNoTrader = false;
        public static bool superDrugNoReward = false;

        // AI stuff
        public static bool poolUsage = false;
        public static bool automaticHealer = false;
        public static bool automaticDefense = false;
        public static bool automaticDefenseDrafted = false;
        public static bool automaticBuffs = false;
        public static bool automaticDebuffs = false;
        public static bool automaticDebuffsDrafted = false;
        public static bool automaticOffense = false;
        public static bool automaticOffenseDrafted = false;
        public static bool automaticFleeing = false;

        // Villains and Stereotypes stuff
        public static bool medievalVillains = false;
        public static bool vengefulOne = true;

        // Hero Organization stuff
        public static bool medievalHeroes = false;

        public SuperheroGenes_Settings() { }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref condensedMeteors, "condensedMeteors", true);
            Scribe_Values.Look(ref multipleMutations, "multipleMutations");
            Scribe_Values.Look(ref multipleArchetypes, "multipleArchetypes");
            Scribe_Values.Look(ref expensiveBase, "expensiveBase");
            Scribe_Values.Look(ref supersEverywhere, "supersEverywhere");
            Scribe_Values.Look(ref activatableSuperGenes, "activatableSuperGenes");
            Scribe_Values.Look(ref interruptibleActivatables, "interruptibleActivatables");
            Scribe_Values.Look(ref middleGrounds, "middleGrounds");
            Scribe_Values.Look(ref allGrounds, "allGrounds");
            Scribe_Values.Look(ref radiomancerOvercharge, "radiomancerOvercharge", true);
            Scribe_Values.Look(ref superDrugNoTrader, "superDrugNoTrader");
            Scribe_Values.Look(ref superDrugNoReward, "superDrugNoReward");

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

            // Villains and Stereotypes stuff
            Scribe_Values.Look(ref medievalVillains, "medievalVillains");
            Scribe_Values.Look(ref vengefulOne, "vengefulOne", true);

            // Villains and Stereotypes stuff
            Scribe_Values.Look(ref medievalHeroes, "medievalHeroes");
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
            if (showMainOptions) numberOfOptions += 12;
            if (ModsConfig.IsActive("SuperheroGenes.Villains") && showVillainOptions) numberOfOptions += 2;
            if (ModsConfig.IsActive("SuperheroGenes.Heroes") && showHeroOptions) numberOfOptions += 1;
            if (showAutocastOptions) numberOfOptions += 10;
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
            }

            optionsMenu.End();
            Widgets.EndScrollView();
            base.Write();
        }
    }
}
