using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace SuperHeroGenesBase
{
    public class MentalState_PanicFleeInsect : MentalState
    {
        private int lastSeenTick = -1;

        protected override bool CanEndBeforeMaxDurationNow => false;

        public override RandomSocialMode SocialModeMax()
        {
            return RandomSocialMode.Off;
        }

        public override void MentalStateTick()
        {
            base.MentalStateTick();

            if (pawn.IsHashIntervalTick(30))
                if (lastSeenTick < 0 || NearbyInsect())
                    lastSeenTick = Find.TickManager.TicksGame;
                else if (lastSeenTick >= 0 && Find.TickManager.TicksGame >= lastSeenTick + def.minTicksBeforeRecovery)
                    RecoverFromState();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref lastSeenTick, "lastSeenTick", -1);
        }

        private bool NearbyInsect()
        {
            if (!pawn.Spawned) return false;
            return pawn.Map.mapPawns.AllPawnsSpawned.Where((arg) => arg.Spawned &&
                arg.RaceProps.Insect && !arg.DeadOrDowned && arg.Awake() &&
                arg.Position.DistanceTo(pawn.Position) <= def.GetModExtension<SHGExtension>().radius).Any();
        }
    }
}
