using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;

namespace SuperHeroGenesBase
{
    public class IncidentWorker_CaravanArrivalBetterTributeCollector : IncidentWorker_TraderCaravanArrival
    {
        public string TraderKindCategory = "TributeCollector";
        public string label = "Royal tribute collector";
        public string acceptance = "any prisoners and gold";
        public string reason = "fulfilling the tradition and supporting the upper houses";
        public string frequency = "roughly twice per year";

        protected override bool TryResolveParmsGeneral(IncidentParms parms)
        {
            if (!base.TryResolveParmsGeneral(parms))
            {
                return false;
            }
            if (def.HasModExtension<SHGExtension>())
            {
                SHGExtension extension = def.GetModExtension<SHGExtension>();
                if (extension.faction == null)
                {
                    Log.Error("The faction for " + def + " was not set in the mod extension. Returning false.");
                    return false;
                }
                Faction faction = Find.FactionManager.FirstFactionOfDef(extension.faction);
                if (faction == null) return false;
                if (extension.category != null) TraderKindCategory = extension.category;

                Map map = (Map)parms.target;
                parms.faction = faction;
                parms.traderKind = DefDatabase<TraderKindDef>.AllDefsListForReading.Where((TraderKindDef t) => t.category == TraderKindCategory && t.faction == extension.faction).RandomElementByWeight((TraderKindDef t) => TraderKindCommonality(t, map, parms.faction));
                if (parms.traderKind.label != null) label = parms.traderKind.label;
                if (extension.descriptionOverrideA != null) acceptance = extension.descriptionOverrideA;
                if (extension.descriptionOverrideB != null) reason = extension.descriptionOverrideB;
                if (extension.descriptionOverrideC != null) frequency = extension.descriptionOverrideC;
            }
            else
            {
                Log.Error(def + " doesn't have the extension, so this is returning false by default.");
                return false;
            }
            return true;
        }

        protected override bool CanFireNowSub(IncidentParms parms)
        {
            if (!base.CanFireNowSub(parms) || !def.HasModExtension<SHGExtension>())
            {
                return false;
            }
            SHGExtension extension = def.GetModExtension<SHGExtension>();
            if (extension.faction == null) return false;

            Faction faction = Find.FactionManager.FirstFactionOfDef(extension.faction);
            if (faction == null) return false;
            return FactionCanBeGroupSource(faction, (Map)parms.target);
        }

        protected override void SendLetter(IncidentParms parms, List<Pawn> pawns, TraderKindDef traderKind)
        {
            TaggedString letterLabel = "LetterLabelTributeCaravanArrival".Translate(label);
            TaggedString letterText = "LetterTributeCaravanArrival".Translate(label, acceptance, reason, frequency, parms.faction.Named("FACTION")).CapitalizeFirst();
            letterText += "\n\n" + "LetterCaravanArrivalCommonWarning".Translate();
            PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(pawns, ref letterLabel, ref letterText, "LetterRelatedPawnsNeutralGroup".Translate(Faction.OfPlayer.def.pawnsPlural), true);
            SendStandardLetter(letterLabel, letterText, LetterDefOf.PositiveEvent, parms, pawns[0]);
        }
    }
}
