using System;
using Verse;
using System.Collections.Generic;

namespace SuperHeroGenesBase
{
    public class Gene_CustomizableClotting : Gene
    {
        public override void Tick()
        {
            base.Tick();
            SHGExtension extension = def.GetModExtension<SHGExtension>();
            if (!pawn.IsHashIntervalTick(extension.ClotCheckInterval))
            {
                return;
            }
            List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
            if (extension.minTendQuality < 0) extension.minTendQuality = 0;
            if (extension.maxTendQuality > 1) extension.maxTendQuality = 1;
            FloatRange TendingQualityRange = new FloatRange(extension.minTendQuality, extension.maxTendQuality);
            for (int num = hediffs.Count - 1; num >= 0; num--)
            {
                if (hediffs[num].Bleeding)
                {
                    hediffs[num].Tended(TendingQualityRange.RandomInRange, TendingQualityRange.TrueMax, 1);
                }
            }
        }
    }
}
