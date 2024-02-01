﻿using Verse;
namespace SuperHeroGenesBase
{
    public class HediffComp_HediffOnKill : HediffComp
    {
        private HediffCompProperties_HediffOnKill Props => (HediffCompProperties_HediffOnKill)props;

        public override void Notify_KilledPawn(Pawn victim, DamageInfo? dinfo)
        {
            if (Props.hediff == null)
            {
                Log.Error(parent.def + " doesn't have a hediff set for the HediffOnKill comp.");
                return;
            }

            if (victim.RaceProps.Humanlike)
            {
                if (Props.allowHumanoids) SHGUtilities.AddOrAppendHediffs(parent.pawn, Props.severityForFirstKill, Props.severityPerExtraKill, Props.hediff);
            }
            else if (victim.RaceProps.Dryad)
            {
                if (Props.allowDryads) SHGUtilities.AddOrAppendHediffs(parent.pawn, Props.severityForFirstKill, Props.severityPerExtraKill, Props.hediff);
            }
            else if (victim.RaceProps.Insect)
            {
                if (Props.allowInsects) SHGUtilities.AddOrAppendHediffs(parent.pawn, Props.severityForFirstKill, Props.severityPerExtraKill, Props.hediff);
            }
            else if (victim.RaceProps.Animal)
            {
                if (Props.allowAnimals) SHGUtilities.AddOrAppendHediffs(parent.pawn, Props.severityForFirstKill, Props.severityPerExtraKill, Props.hediff);
            }
            else if (victim.RaceProps.IsMechanoid)
            {
                if (Props.allowMechanoids) SHGUtilities.AddOrAppendHediffs(parent.pawn, Props.severityForFirstKill, Props.severityPerExtraKill, Props.hediff);
            }
            else // As a default, just assume it's a viable target. This is for generic hopeful mod compatability
            {
                SHGUtilities.AddOrAppendHediffs(parent.pawn, Props.severityForFirstKill, Props.severityPerExtraKill, Props.hediff);
            }
        }

        public bool CheckGenderViability(Pawn victim)
        {
            if (!Props.onlyMaleVictims && !Props.onlyFemaleVictims) return true;
            if (Props.onlyMaleVictims && Props.onlyFemaleVictims) return false;
            Gender gender = victim.gender;
            if (gender == Gender.Male)
            {
                if (Props.onlyMaleVictims) return true;
            }
            else if (gender == Gender.Female)
            {
                if (Props.onlyFemaleVictims) return true;
            }
            return false;
        }
    }
}
