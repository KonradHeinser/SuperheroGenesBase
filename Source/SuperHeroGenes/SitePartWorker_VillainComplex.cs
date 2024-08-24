using RimWorld.Planet;
using RimWorld;
using UnityEngine;
using RimWorld.QuestGen;
using Verse;
using System.Collections.Generic;
using Verse.Grammar;

namespace SuperHeroGenesBase
{
    public class SitePartWorker_FactionComplex : SitePartWorker
    {
        private List<string> threatsTmp = new List<string>();

        public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
        {
            if (site.MainSitePartDef == def)
            {
                return null;
            }
            return base.GetPostProcessedThreatLabel(site, sitePart) + ": " + GetThreatsInfo(sitePart.parms, site.Faction);
        }

        public override SitePartParams GenerateDefaultParams(float myThreatPoints, int tile, Faction faction)
        {
            SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
            sitePartParams.mortarsCount = Rand.RangeInclusive(0, 1);
            sitePartParams.turretsCount = Mathf.Clamp(Mathf.RoundToInt(sitePartParams.threatPoints / ThingDefOf.Turret_MiniTurret.building.combatPower), 2, 11);
            return sitePartParams;
        }

        public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
        {
            base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
            string threatsInfo = GetThreatsInfo(part.parms, part.site.Faction);
            outExtraDescriptionRules.Add(new Rule_String("threatsInfo", threatsInfo));
            if (!slate.TryGet("thing", out Thing thing))
            {
                thing = ThingMaker.MakeThing(part.def.conditionCauserDef);
                slate.Set("thing", thing);
            }

            part.conditionCauser = thing;
        }

        private string GetThreatsInfo(SitePartParams parms, Faction faction)
        {
            threatsTmp.Clear();
            int num = parms.mortarsCount + 1;
            string text = null;
            if (parms.turretsCount != 0)
            {
                text = ((parms.turretsCount != 1) ? ((string)"Turrets".Translate()) : ((string)"Turret".Translate()));
                threatsTmp.Add("KnownSiteThreatEnemyCountAppend".Translate(parms.turretsCount.ToString(), text));
            }
            if (parms.mortarsCount != 0)
            {
                text = ((parms.mortarsCount != 1) ? ((string)"Mortars".Translate()) : ((string)"Mortar".Translate()));
                threatsTmp.Add("KnownSiteThreatEnemyCountAppend".Translate(parms.mortarsCount.ToString(), text));
            }
            if (num != 0)
            {
                text = ((faction != null) ? ((num == 1) ? faction.def.pawnSingular : faction.def.pawnsPlural) : ((string)((num == 1) ? "Enemy".Translate() : "Enemies".Translate())));
                threatsTmp.Add("KnownSiteThreatEnemyCountAppend".Translate(num.ToString(), text));
            }
            return threatsTmp.ToCommaList(true);
        }

        public override void Notify_SiteMapAboutToBeRemoved(SitePart sitePart)
        {
            base.Notify_SiteMapAboutToBeRemoved(sitePart);
            if (!sitePart.conditionCauser.DestroyedOrNull() && sitePart.conditionCauser.Spawned && sitePart.conditionCauser.Map == sitePart.site.Map)
            {
                sitePart.conditionCauser.DeSpawn();
                sitePart.conditionCauserWasSpawned = false;
            }
        }
    }
}
