using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;

namespace SuperHeroGenesBase
{
    public class QuestNode_Root_StopTheLeague : QuestNode
    {
        public FactionDef faction;

        public FactionDef enemyFaction;

        public SitePartDef sitePartDef;

        public IntRange ticksBetweenSubquests;

        public int totalSubquests;

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

        protected override void RunInt()
        {
            Quest quest = QuestGen.quest;
            Slate slate = QuestGen.slate;
            Map map = QuestGen_Get.GetMap();
            float num = slate.Get("points", 0f);
            TryFindSiteTile(out var tile);
            string text = QuestGen.GenerateNewSignal("SubquestsCompleted");
            string awakenSecurityThreatsSignal = QuestGen.GenerateNewSignal("AwakenSecurityThreats");
            QuestGen.GenerateNewSignal("PostMapAdded");
            bool allowViolentQuests = Find.Storyteller.difficulty.allowViolentQuests;

            QuestPart_SubquestGenerator_StopTheLeague generator = new QuestPart_SubquestGenerator_StopTheLeague
            {
                inSignalEnable = QuestGen.slate.Get<string>("inSignal"),
                interval = ticksBetweenSubquests,
                useMapParentThreatPoints = map?.Parent,
                expiryInfoPartKey = "SHGHero_DestroyedDevices",
                maxActiveSubquests = 1,
                maxSuccessfulSubquests = totalSubquests
            };
            generator.subquestDefs.AddRange(GetAllSubquests(QuestGen.Root));
            generator.outSignalsCompleted.Add(text);
            quest.AddPart(generator);
            quest.SignalPass(null, QuestGenUtility.HardcodedSignalWithQuestID("thing.TookDamage"), awakenSecurityThreatsSignal);

            QuestPart_Choice questPart_Choice = quest.RewardChoice();
            QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice
            {
                rewards = { new Reward_Unknown() }
            };
            questPart_Choice.choices.Add(choice);

            SitePartParams sitePartParams = new SitePartParams
            {
                points = num,
                triggerSecuritySignal = awakenSecurityThreatsSignal
            };
            if (num > 0f)
            {
                sitePartParams.exteriorThreatPoints = ExteriorThreatPointsOverPoints.Evaluate(num);
                sitePartParams.interiorThreatPoints = InteriorThreatPointsOverPoints.Evaluate(num);
            }

            Faction hostileFaction = Find.FactionManager.FirstFactionOfDef(enemyFaction);
            Site site = QuestGen_Sites.GenerateSite(Gen.YieldSingle(new SitePartDefWithParams(sitePartDef, sitePartParams)), tile, hostileFaction);
            quest.SpawnWorldObject(site, null, text);

            TaggedString taggedString = "SHGHero_DeviceFoundLocation".Translate() + "\n\n" + "SHGHero_DeviceFoundLocationFoundSecurityThreats".Translate(hostileFaction.Name);
            quest.Letter(LetterDefOf.PositiveEvent, text, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, Gen.YieldSingle(site), false,
                taggedString.Resolve(), null, "SHGHero_DeviceFoundLocation".Translate());
            quest.DescriptionPart("[questDescriptionPartBeforeDiscovery]", quest.AddedSignal, text, QuestPart.SignalListenMode.OngoingOrNotYetAccepted);
            quest.DescriptionPart("[questDescriptionPartAfterDiscovery]", text, "thing.Destroyed", QuestPart.SignalListenMode.OngoingOnly);
            quest.DescriptionPart("SHGHero_DeviceFoundLocation".Translate(), text);

            string text4 = QuestGen.GenerateNewSignal("ReTriggerSecurityThreats");
            QuestPart_PassWhileActive part2 = new QuestPart_PassWhileActive
            {
                inSignalEnable = awakenSecurityThreatsSignal,
                inSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.MapGenerated"),
                outSignal = text4
            };
            quest.AddPart(part2);
            quest.SignalPass(delegate
            {
                quest.SignalPass(null, null, awakenSecurityThreatsSignal);
                quest.Message("SHGHero_ThreatsAlerted".Translate(hostileFaction.def.pawnsPlural), MessageTypeDefOf.NegativeEvent);
            }, text4);
            quest.AnyHostileThreatToPlayer(site, true, delegate
            {
                quest.Message("SHGHero_ThreatsDistrubed".Translate(hostileFaction.def.pawnsPlural), MessageTypeDefOf.NegativeEvent);
            }, null, awakenSecurityThreatsSignal);

            slate.Set("site", site);

        }

        protected override bool TestRunInt(Slate slate)
        {
            Faction heroes = FactionUtility.DefaultFactionFrom(faction);
            Faction villains = FactionUtility.DefaultFactionFrom(enemyFaction);
            return heroes != null && !heroes.HostileTo(Faction.OfPlayer) && villains != null && TryFindSiteTile(out var _, true)
                && GetAllSubquests(QuestGen.Root).Any() && Find.Storyteller.difficulty.allowViolentQuests;
        }

        private bool TryFindSiteTile(out PlanetTile tile, bool exitOnFirstTileFound = false)
        {
            return TileFinder.TryFindNewSiteTile(out tile, exitOnFirstTileFound: exitOnFirstTileFound, validator: (arg => arg.Tile.hilliness == Hilliness.Flat));
        }

        private IEnumerable<QuestScriptDef> GetAllSubquests(QuestScriptDef parent)
        {
            return DefDatabase<QuestScriptDef>.AllDefs.Where((QuestScriptDef q) => q.epicParent == parent);
        }
    }
}
