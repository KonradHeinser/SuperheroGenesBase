﻿using RimWorld;
using Verse;
using Verse.AI;

namespace SuperHeroGenesBase
{
    public class MentalState_PanicFleeWater : MentalState
    {
        private int lastWaterSeenTick = -1;

        protected override bool CanEndBeforeMaxDurationNow => false;

        public override RandomSocialMode SocialModeMax()
        {
            return RandomSocialMode.Off;
        }

        public override void MentalStateTick(int delta)
        {
            base.MentalStateTick(delta);

            if (pawn.IsHashIntervalTick(30))
                if (lastWaterSeenTick < 0 || NearbyWater())
                    lastWaterSeenTick = Find.TickManager.TicksGame;
                else if (lastWaterSeenTick >= 0 && Find.TickManager.TicksGame >= lastWaterSeenTick + def.minTicksBeforeRecovery)
                    RecoverFromState();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref lastWaterSeenTick, "lastWaterSeenTick", -1);
        }

        private bool NearbyWater()
        {
            return SHGUtilities.CheckNearbyWater(pawn, 1, out _, def.GetModExtension<SHGExtension>().radius);
        }
    }
}
