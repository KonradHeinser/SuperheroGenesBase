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
        public List<ThingDef> villainousDevices;

        public FactionDef faction;

        public SitePartDef sitePartDef;

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
                interval = new IntRange(300000, 600000),
                useMapParentThreatPoints = map?.Parent,
                expiryInfoPartKey = "SHGHero_DestroyedDevices",
                maxActiveSubquests = 1,
                maxSuccessfulSubquests = 3
            };
            generator.subquestDefs.AddRange(GetAllSubquests(QuestGen.Root));
            generator.outSignalsCompleted.Add(text);
            quest.AddPart(generator);
            num = slate.Get("points", 0f);
            Thing thing = ThingMaker.MakeThing(villainousDevices.RandomElement());
            QuestGen_Signal.SignalPass(quest, null, QuestGenUtility.HardcodedSignalWithQuestID("device.TookDamage"), awakenSecurityThreatsSignal);

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
            Faction hostileFaction = FactionUtility.DefaultFactionFrom(faction);
            Site site = QuestGen_Sites.GenerateSite(Gen.YieldSingle(new SitePartDefWithParams(sitePartDef, sitePartParams)), tile, hostileFaction);
            quest.SpawnWorldObject(site, null, text);

            TaggedString taggedString = "SHGHero_DeviceFoundLocation".Translate() + "\n\n" + "SHGHero_DeviceFoundLocationFoundSecurityThreats".Translate(hostileFaction);
            quest.Letter(LetterDefOf.PositiveEvent, text, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, Gen.YieldSingle(site), false,
                taggedString.Resolve(), null, "SHGHero_DeviceFoundLocation".Translate());
            quest.DescriptionPart("[questDescriptionPartBeforeDiscovery]", quest.AddedSignal, text, QuestPart.SignalListenMode.OngoingOrNotYetAccepted);
            quest.DescriptionPart("SHGHero_DeviceFoundLocation".Translate(), text);

            QuestPart_Delay part = new QuestPart_Delay
            {
                delayTicks = 180000,
                alertLabel = "AncientAltarThreatsWaking".Translate(),
                alertExplanation = "AncientAltarThreatsWakingDesc".Translate(),
                ticksLeftAlertCritical = 2500,
                inSignalEnable = text,
                alertCulprits =
                {
                    (GlobalTargetInfo)thing,
                    (GlobalTargetInfo)site
                },
                isBad = true,
                outSignalsCompleted = { awakenSecurityThreatsSignal }
            };
            quest.AddPart(part);
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
                quest.Message("SHGHero_ThreatsAlerted".Translate(hostileFaction), MessageTypeDefOf.NegativeEvent);
            }, text4);
            quest.AnyHostileThreatToPlayer(site, true, delegate
            {
                quest.Message("SHGHero_ThreatsDistrubed".Translate(), MessageTypeDefOf.NegativeEvent);
            }, null, awakenSecurityThreatsSignal);

            quest.End(QuestEndOutcome.Success, 0, null, QuestGenUtility.HardcodedSignalWithQuestID("thing.Destroyed"), QuestPart.SignalListenMode.OngoingOnly, true);
            slate.Set("device", thing);
            slate.Set("site", site);
        }

        protected override bool TestRunInt(Slate slate)
        {
            return TryFindSiteTile(out var _, true) && GetAllSubquests(QuestGen.Root).Any() && villainousDevices.Any() &&
                FactionUtility.DefaultFactionFrom(faction) != null && Find.Storyteller.difficulty.allowViolentQuests;
        }

        private bool TryFindSiteTile(out int tile, bool exitOnFirstTileFound = false)
        {
            return TileFinder.TryFindNewSiteTile(out tile, 7, 27, false, TileFinderMode.Near, -1, exitOnFirstTileFound);
        }

        private IEnumerable<QuestScriptDef> GetAllSubquests(QuestScriptDef parent)
        {
            return DefDatabase<QuestScriptDef>.AllDefs.Where((QuestScriptDef q) => q.epicParent == parent);
        }
    }
}
