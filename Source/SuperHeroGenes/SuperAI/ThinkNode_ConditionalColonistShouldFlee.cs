using Verse;
using Verse.AI;
using Verse.AI.Group;
using RimWorld;

namespace SuperHeroGenesBase
{
    public class ThinkNode_ConditionalColonistShouldFlee : ThinkNode_Conditional
    {
        protected override bool Satisfied(Pawn pawn)
        {
            Pawn meleeThreat = pawn.mindState.meleeThreat;

            if (meleeThreat == null)
            {
                return false;
            }
            if (meleeThreat.IsInvisible() || pawn.IsInvisible())
            {
                return false;
            }
            if (IsHunting(pawn, meleeThreat))
            {
                return false;
            }
            if (IsDueling(pawn, meleeThreat))
            {
                return false;
            }
            if (PawnUtility.PlayerForcedJobNowOrSoon(pawn))
            {
                return false;
            }
            if (!pawn.mindState.MeleeThreatStillThreat)
            {
                pawn.mindState.meleeThreat = null;
                return false;
            }
            if (pawn.playerSettings != null && pawn.playerSettings.UsesConfigurableHostilityResponse && pawn.playerSettings.hostilityResponse == HostilityResponseMode.Flee)
            {
                return true;
            }

            return false;
        }

        private bool IsDueling(Pawn pawn, Pawn other)
        {
            if (pawn.GetLord()?.LordJob is LordJob_Ritual_Duel lordJob_Ritual_Duel)
            {
                return lordJob_Ritual_Duel.Opponent(pawn) == other;
            }
            return false;
        }

        private bool IsHunting(Pawn pawn, Pawn prey)
        {
            if (pawn.CurJob == null)
            {
                return false;
            }
            if (pawn.jobs.curDriver is JobDriver_Hunt jobDriver_Hunt)
            {
                return jobDriver_Hunt.Victim == prey;
            }
            if (pawn.jobs.curDriver is JobDriver_PredatorHunt jobDriver_PredatorHunt)
            {
                return jobDriver_PredatorHunt.Prey == prey;
            }
            return false;
        }
    }
}
