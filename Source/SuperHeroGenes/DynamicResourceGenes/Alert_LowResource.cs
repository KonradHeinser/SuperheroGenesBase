using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using RimWorld;

namespace SuperHeroGenesBase
{
    public class Alert_LowResource : Alert
    {
        private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

        private List<string> targetLabels = new List<string>();

        private List<GlobalTargetInfo> Targets
        {
            get
            {
                CalculateTargets();
                return targets;
            }
        }

        public override string GetLabel() // Make the pawn adding use loops to add to lists so the alerts can be iterated through
        {
            if (Targets.Count == 1)
            {
                return "AlertLowResource".Translate() + ": " + targetLabels[0];
            }
            return "AlertLowResource".Translate();
        }

        private void CalculateTargets()
        {
            targets.Clear();
            targetLabels.Clear();
            if (!ModsConfig.BiotechActive)
            {
                return;
            }
            foreach (Pawn item in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive)
            {
                if (item.RaceProps.Humanlike && item.Faction == Faction.OfPlayer)
                {
                    ResourceGene resourceGene = item.genes?.GetFirstGeneOfType<ResourceGene>();
                    if (resourceGene != null && resourceGene.Value < resourceGene.MinLevelForAlert)
                    {
                        targets.Add(item);
                        targetLabels.Add(item.NameShortColored.Resolve());
                    }
                }
            }
        }

        public override TaggedString GetExplanation()
        {
            return "AlertLowResourceDesc".Translate() + ":\n" + targetLabels.ToLineList("  - ");
        }

        public override AlertReport GetReport()
        {
            return AlertReport.CulpritsAre(Targets);
        }
    }
}
