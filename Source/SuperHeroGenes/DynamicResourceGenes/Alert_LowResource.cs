﻿using System.Collections.Generic;
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

        public override string GetLabel()
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
            foreach (Pawn item in PawnsFinder.AllCaravansAndTravellingTransporters_Alive)
            {
                if (item.RaceProps.Humanlike && item.Faction == Faction.OfPlayer)
                {
                    ResourceGene resourceGene = item.genes?.GetFirstGeneOfType<ResourceGene>();
                    if (resourceGene == null) continue;
                    if (resourceGene.Value < resourceGene.MinLevelForAlert)
                    {
                        targets.Add(item);
                        targetLabels.Add(item.NameShortColored.Resolve());
                    }
                    else
                    {
                        foreach (Gene gene in item.genes.GenesListForReading)
                        {
                            if (gene.def.HasModExtension<DRGExtension>() && gene.def.GetModExtension<DRGExtension>().isMainGene)
                            {
                                resourceGene = (ResourceGene)gene;
                                if (resourceGene.Value < resourceGene.MinLevelForAlert)
                                {
                                    targets.Add(item);
                                    targetLabels.Add(item.NameShortColored.Resolve());
                                    break;
                                }
                            }
                        }
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
