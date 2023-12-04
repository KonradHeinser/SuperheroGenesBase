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
        public static bool condensedMeteors = true;
        public static bool expensiveBase = false;
        public static bool supersEverywhere = false;
        public static bool activatableSuperGenes = false;
        public static bool interruptibleActivatables = false;
        public static bool middleGrounds = false;
        public static bool allGrounds = false;

        public static bool medievalVillains = false;
        public static bool vengefulOne = true;

        public SuperheroGenes_Settings() { }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref condensedMeteors, "condensedMeteors");
            Scribe_Values.Look(ref expensiveBase, "expensiveBase");
            Scribe_Values.Look(ref supersEverywhere, "supersEverywhere");
            Scribe_Values.Look(ref activatableSuperGenes, "activatableSuperGenes");
            Scribe_Values.Look(ref interruptibleActivatables, "interruptibleActivatables");
            Scribe_Values.Look(ref middleGrounds, "middleGrounds");
            Scribe_Values.Look(ref allGrounds, "allGrounds");

            Scribe_Values.Look(ref medievalVillains, "medievalVillains");
            Scribe_Values.Look(ref vengefulOne, "vengefulOne");
        }

        public void DoWindowContents(Rect inRect)
        {
            Listing_Standard optionsMenu = new Listing_Standard();

            optionsMenu.Begin(inRect);
            optionsMenu.Label("SHG_ModName".Translate());
            optionsMenu.Gap(7f);
            optionsMenu.CheckboxLabeled("SHG_CondensedMeteors".Translate(), ref condensedMeteors, "SHG_CondensedMeteorsDescription".Translate());
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
            if (ModsConfig.IsActive("SuperheroGenes.Villains"))
            {
                optionsMenu.Gap(10f);
                optionsMenu.Label("SHG_Villains_ModName".Translate());
                optionsMenu.Gap(7f);
                optionsMenu.CheckboxLabeled("SHG_MedievalVillains".Translate(), ref medievalVillains, "SHG_MedievalVillainsDescription".Translate());
                optionsMenu.Gap(10f);
                optionsMenu.CheckboxLabeled("SHG_VengefulOne".Translate(), ref vengefulOne, "SHG_VengefulOneDescription".Translate());
                optionsMenu.Gap(10f);
            }
            optionsMenu.End();
            base.Write();
        }
    }
}
