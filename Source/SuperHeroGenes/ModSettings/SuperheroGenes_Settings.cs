using RimWorld;
using UnityEngine;
using Verse;

namespace SuperHeroGenesBase
{
    public class SuperheroGenes_Mod : Mod
    {


        public SuperheroGenes_Mod(ModContentPack content) : base(content)
        {
            GetSettings<SuperheroGenes_Settings>();
        }
        public override string SettingsCategory()
        {

            return "SHG_ModName".Translate();



        }



        public override void DoSettingsWindowContents(Rect inRect)
        {
            SuperheroGenes_Settings.DoWindowContents(inRect);
        }
    }

    public class SuperheroGenes_Settings : ModSettings
    {
        public static bool condensedMeteors = true;
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref condensedMeteors, "condensedMeteors", true, true);




        }

        public static void DoWindowContents(Rect inRect)
        {
            Listing_Standard optionsMenu = new Listing_Standard();


            optionsMenu.Begin(inRect);
            optionsMenu.CheckboxLabeled("SHG_CondensedMeteors".Translate(), ref condensedMeteors, "SHG_CondensedMeteorsDescription".Translate());
            optionsMenu.Gap(15f);
            if (ModsConfig.IsActive("SuperheroGenes.Villains")) { bool uselessFlag = true; }
            optionsMenu.End();
        }
    }
}
