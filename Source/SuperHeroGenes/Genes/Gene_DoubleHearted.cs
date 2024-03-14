using System.Collections.Generic;
using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class Gene_DoubleHearted : Gene
    {
        public int lastDeathTicks;
        public Hediff_SecondHeart LinkedHediff
        {
            get
            {
                List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
                for (int i = 0; i < hediffs.Count; i++)
                {
                    Hediff_SecondHeart hediff_SecondHeart;
                    if ((hediff_SecondHeart = (hediffs[i] as Hediff_SecondHeart)) != null)
                    {
                        return hediff_SecondHeart;
                    }
                }
                return null;
            }
        }

        public override void PostAdd()
        {
            base.PostAdd();
            Hediff_SecondHeart hediff_SecondHeart = (Hediff_SecondHeart)HediffMaker.MakeHediff(SHGDefOf.SecondHeart, pawn, null);
            pawn.health.AddHediff(hediff_SecondHeart, null, null, null);
            lastDeathTicks = Find.TickManager.TicksGame;
        }

        public override void PostRemove()
        {
            Hediff_SecondHeart linkedHediff = LinkedHediff;
            if (linkedHediff != null)
            {
                pawn.health.RemoveHediff(linkedHediff);
            }
            base.PostRemove();
        }

        public override void Reset()
        {
            Hediff_SecondHeart linkedHediff = LinkedHediff;
            if (linkedHediff != null)
            {
                linkedHediff.Severity = 0.999f;
            }
            lastDeathTicks = Find.TickManager.TicksGame;
        }

        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {
            Hediff_SecondHeart linkedHediff = LinkedHediff;
            if (linkedHediff.Severity < 0.1f)
            {
                ResurrectionUtility.TryResurrect(pawn);
                Reset();
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref lastDeathTicks, "lastDeathTick", 0, false);
        }
    }
}
