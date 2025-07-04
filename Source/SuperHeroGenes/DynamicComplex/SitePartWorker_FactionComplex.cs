using RimWorld.Planet;
using RimWorld;
using UnityEngine;
using RimWorld.QuestGen;
using Verse;
using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace SuperHeroGenesBase
{
    public class SitePartWorker_FactionComplex : SitePartWorker
    {
        private static readonly SimpleCurve ExteriorThreatPointsOverPoints = new SimpleCurve
        {
            new CurvePoint(0f, 500f),
            new CurvePoint(500f, 500f),
            new CurvePoint(10000f, 10000f)
        };

        private static readonly SimpleCurve InteriorThreatPointsOverPoints = new SimpleCurve
        {
            new CurvePoint(0f, 300f),
            new CurvePoint(300f, 300f),
            new CurvePoint(10000f, 5000f)
        };

        public override SitePartParams GenerateDefaultParams(float myThreatPoints, PlanetTile tile, Faction faction)
        {
            SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
            sitePartParams.mortarsCount = Rand.RangeInclusive(0, 1);
            sitePartParams.turretsCount = Mathf.Clamp(Mathf.RoundToInt(sitePartParams.threatPoints / ThingDefOf.Turret_MiniTurret.building.combatPower), 2, 11);

            if (myThreatPoints > 0)
            {
                sitePartParams.exteriorThreatPoints = ExteriorThreatPointsOverPoints.Evaluate(myThreatPoints);
                sitePartParams.interiorThreatPoints = InteriorThreatPointsOverPoints.Evaluate(myThreatPoints);
            }

            return sitePartParams;
        }

        public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
        {
            base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);

            if (part.def.conditionCauserDef != null && !slate.TryGet("thing", out Thing thing))
            {
                thing = ThingMaker.MakeThing(part.def.conditionCauserDef);
                slate.Set("thing", thing);
                part.conditionCauser = thing;
            }
            // Backup just in case something went really wrong earlier on
            if (slate.Get("points", 0) > 0 && (part.parms.interiorThreatPoints <= 0 || part.parms.exteriorThreatPoints <= 0) && Find.Storyteller.difficulty.allowViolentQuests)
            {
                part.parms.exteriorThreatPoints = ExteriorThreatPointsOverPoints.Evaluate(slate.Get("points", 0));
                part.parms.interiorThreatPoints = InteriorThreatPointsOverPoints.Evaluate(slate.Get("points", 0));
            }

            if (slate.Get("sitePoints", 0) > 0)
                part.parms.lootMarketValue = slate.Get("sitePoints", 0);
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
