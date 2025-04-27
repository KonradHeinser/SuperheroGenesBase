using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

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

        public static bool condensedMeteors = true;
        public static bool multipleArchetypes;
        public static bool multipleMutations;
        public static bool expensiveBase;
        public static bool supersEverywhere;
        public static bool archetypesEverywhere;
        public static bool mutationsAnywhere;
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
        public static bool disableColonistAI;
        public static bool automaticHealer;
        public static bool automaticDefense;
        public static bool automaticDefenseDrafted;
        public static bool automaticBuffs;
        public static bool automaticDebuffs;
        public static bool automaticDebuffsDrafted;
        public static bool automaticOffense;
        public static bool automaticOffenseDrafted;
        public static bool automaticFleeing;
        public static bool automaticRadioPurge = true;

        // Villains and Stereotypes stuff
        public static bool medievalVillains;
        public static bool vengefulOne = true;

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

        private List<TabRecord> tabsList;

        private int tabInt = 0;

        public List<TabRecord> TabsList
        {
            get
            {
                if (tabsList.NullOrEmpty())
                {
                    tabsList = new List<TabRecord>()
                    {
                        new TabRecord("SHG_ModName".Translate(), delegate ()
                        {
                            tabInt = 0;
                            tabsList.Clear();
                        }, tabInt == 0),
                        new TabRecord("SHG_SuperAI".Translate(), delegate ()
                        {
                            tabInt = 3;
                            tabsList.Clear();
                        }, tabInt == 3),
                    };

                    if (ModsConfig.IsActive("SuperheroGenes.Villains"))
                        tabsList.Insert(1, new TabRecord("SHG_Villains_ModName".Translate(),
                                delegate ()
                                {
                                    tabInt = 1;
                                    tabsList.Clear();
                                }, tabInt == 1)
                            );
                    if (ModsConfig.IsActive("SuperheroGenes.Heroes"))
                        tabsList.Insert(2, new TabRecord("SHG_Heroes_ModName".Translate(),
                                delegate ()
                                {
                                    tabInt = 2;
                                    tabsList.Clear();
                                }, tabInt == 2)
                            );
                }
                return tabsList;
            }
        }

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
            Scribe_Values.Look(ref mutationsAnywhere, "mutationsAnywhere");
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
            Scribe_Values.Look(ref disableColonistAI, "disableColonistAI");
            Scribe_Values.Look(ref automaticHealer, "automaticHealer");
            Scribe_Values.Look(ref automaticDefense, "automaticDefense");
            Scribe_Values.Look(ref automaticDefenseDrafted, "automaticDefenseDrafted");
            Scribe_Values.Look(ref automaticBuffs, "automaticBuffs");
            Scribe_Values.Look(ref automaticDebuffs, "automaticDebuffs");
            Scribe_Values.Look(ref automaticDebuffsDrafted, "automaticDebuffsDrafted");
            Scribe_Values.Look(ref automaticOffense, "automaticOffense");
            Scribe_Values.Look(ref automaticOffenseDrafted, "automaticOffenseDrafted");
            Scribe_Values.Look(ref automaticFleeing, "automaticFleeing");
            Scribe_Values.Look(ref automaticRadioPurge, "automaticRadioPurge", true);

            // Villains and Stereotypes stuff
            Scribe_Values.Look(ref medievalVillains, "medievalVillains");
            Scribe_Values.Look(ref vengefulOne, "vengefulOne", true);

            // Villains and Stereotypes stuff
            Scribe_Values.Look(ref medievalHeroes, "medievalHeroes");
            Scribe_Values.Look(ref leagueGathering, "leagueGathering");
            Scribe_Values.Look(ref radiantQuests, "radiantQuests", true);
        }

        public void DoWindowContents(Rect inRect)
        {
            Listing_Standard optionsMenu = new Listing_Standard();

            Rect tabs = new Rect(inRect)
            {
                yMin = 80,
                height = 40
            };
            TabDrawer.DrawTabs<TabRecord>(tabs, TabsList, Mathf.CeilToInt(TabsList.Count / 5), Mathf.FloorToInt(tabs.width / 5));

            var scrollContainer = new Rect(inRect);
            scrollContainer.height -= optionsMenu.CurHeight + tabs.height;
            scrollContainer.y += tabs.height;
            Widgets.DrawBoxSolid(scrollContainer, Color.grey);
            var innerContainer = scrollContainer.ContractedBy(1);
            Widgets.DrawBoxSolid(scrollContainer, new ColorInt(37, 37, 37).ToColor);
            var frameRect = innerContainer.ContractedBy(5);
            frameRect.y += 15;
            frameRect.height -= 15;
            var contentRect = frameRect.ContractedBy(10);

            if (tabInt == 0)
                contentRect.height = 550;
            Widgets.BeginScrollView(frameRect, ref scrollPosition, contentRect);
            optionsMenu.Begin(contentRect);

            switch (tabInt)
            {
                case 0:
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
                        optionsMenu.CheckboxLabeled("SHG_MutationsAnywhere".Translate(), ref mutationsAnywhere, "SHG_MutationsAnywhereDescription".Translate());
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
                    break;
                case 1:
                    optionsMenu.CheckboxLabeled("SHG_MedievalVillains".Translate(), ref medievalVillains, "SHG_MedievalVillainsDescription".Translate());
                    optionsMenu.Gap(10f);
                    optionsMenu.CheckboxLabeled("SHG_VengefulOne".Translate(), ref vengefulOne, "SHG_VengefulOneDescription".Translate());
                    optionsMenu.Gap(10f);
                    break;
                case 2:
                    optionsMenu.CheckboxLabeled("SHG_MedievalHeroes".Translate(), ref medievalHeroes, "SHG_MedievalHeroesDescription".Translate());
                    optionsMenu.Gap(10f);
                    optionsMenu.CheckboxLabeled("SHG_LeagueGathering".Translate(), ref leagueGathering, "SHG_LeagueGatheringDescription".Translate());
                    optionsMenu.Gap(10f);
                    optionsMenu.CheckboxLabeled("SHG_RadiantQuests".Translate(), ref radiantQuests, "SHG_RadiantQuestsDescription".Translate());
                    optionsMenu.Gap(10f);
                    break;
                case 3:
                    optionsMenu.CheckboxLabeled("SHG_DisableColonistAI".Translate(), ref disableColonistAI, "SHG_DisableColonistAIDescription".Translate());
                    optionsMenu.Gap(10f);
                    if (!disableColonistAI)
                    {
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
                    }
                    break;
            }
            
            optionsMenu.End();
            Widgets.EndScrollView();
            Write();
        }
    }
}
