using System.Collections.Generic;
using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class HediffComp_TieredRegeneration : HediffComp
    {
        private HediffCompProperties_TieredRegeneration Props => (HediffCompProperties_TieredRegeneration)props;
        private int regrowTicksRemaining = -1;
        private int healTicksRemaining = -1;
        private bool healInProgress = false;
        private bool healWhileRegrowing = false;
        private bool prioritizeHeal = false;

        // Stats from the current set
        private bool regrowthAllowed = false;
        private bool healAllowed = true;
        private float tempMinSeverity;
        private float tempMaxSeverity;
        public int regrowthInterval;
        public int healInterval;
        private float healAmount;
        private int repeatCount;
        List<Hediff_Injury> wounds;

        // These are to allow the comp to check if it should be continuing to heal/regrow without having to go through the entire list every time


        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            healWhileRegrowing = Props.healWhileRegrowing;
            prioritizeHeal = Props.prioritizeHeal;
            GetSet();
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            Hediff missingPart = Pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.MissingBodyPart);

            if (parent.Severity < tempMinSeverity || parent.Severity > tempMaxSeverity) // This checks if the hediff is in a new set
            {
                GetSet();
            }
            if (healInProgress)
            {
                GetWounds();
                // Regrowth stuff
                if (regrowTicksRemaining >= 0)
                { // If regrowth is no longer possible or there are new wounds while prioritizing healing, quit trying to regrow
                    if (missingPart == null || !regrowthAllowed || (wounds.Count != 0 && Props.prioritizeHeal))
                    {
                        regrowTicksRemaining = -1;
                    }
                    else
                    {
                        if (regrowTicksRemaining == 0) // Otherwise, if the ticks have hit zero, give the part back
                        {
                            Pawn.health.RemoveHediff(missingPart);
                            regrowTicksRemaining = -1;
                        }
                        else { regrowTicksRemaining--; } // If not done with regrowing, then tick on
                    }
                }
                // Heal stuff
                if (healTicksRemaining >= 0)
                {
                    if (wounds.Count == 0 || !healAllowed) // If there are no wounds or healing has been disabled, reset heal stuff
                    {
                        healTicksRemaining = -1;
                    }
                    else
                    {
                        if (healTicksRemaining == 0) // If done healing, start grabbing random wounds and healing them
                        {
                            for (int i = 0; i < repeatCount; i++)
                            {
                                Hediff_Injury wound = wounds.RandomElement();
                                wound.Severity -= healAmount;
                                if (wound.Severity <= 0)
                                {
                                    wounds.Remove(wound);
                                    if (wounds.Count == 0) break;
                                }
                                if (wound.Severity >= wound.Part.def.hitPoints)
                                {
                                    // To avoid crazy damage and accidental killing from reverse healing, damage is limited to just before destruction

                                    wound.Severity = wound.Part.def.hitPoints - 0.1f;
                                    wounds.Remove(wound);
                                    if (wounds.Count == 0) break;
                                }
                            }
                            healTicksRemaining = -1;
                        }
                        else { healTicksRemaining--; } // If not done healing, then tick on
                    }
                }

                if (regrowTicksRemaining < 0 && healTicksRemaining < 0)
                {
                    healInProgress = false;
                }
                else if (healWhileRegrowing) // If both are supposed to be active at the same time, and one of them is still active, try to activate the other
                {
                    missingPart = Pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.MissingBodyPart);
                    GetWounds();

                    if (regrowTicksRemaining < 0 && missingPart != null && regrowthAllowed) // If the inactive one is regrowth and there is a missing part, start the timer
                    {
                        regrowTicksRemaining = regrowthInterval;
                    }
                    else if (healTicksRemaining < 0 && wounds.Count > 0 && healAllowed) // If healing is the inactive one, and there are wounds to heal, start a timer
                    {
                        healTicksRemaining = healInterval;
                    }
                }
            }
            else
            {
                GetWounds();
                if (healWhileRegrowing) // If both are supposed to be active at the same time try to activate both. It checks if they are permitted just to ensure no weird xml inputs are given
                {
                    if (regrowthAllowed && missingPart != null) // If regrowth is permitted and there is a missing part, start the timer
                    {
                        regrowTicksRemaining = regrowthInterval;
                    }
                    if (healAllowed && wounds.Count > 0) // If healing is permitted and there are wounds to heal, start a timer
                    {
                        healTicksRemaining = healInterval;
                    }
                }
                else if (prioritizeHeal) // If healing is prioritized, try to start that first
                {
                    if (healAllowed && wounds.Count > 0) // If healing is permitted and there are wounds to heal, start a timer
                    {
                        healTicksRemaining = healInterval;
                    }
                    else if (regrowthAllowed && missingPart != null) // If regrowth is permitted and there is a missing part, start the timer
                    {
                        regrowTicksRemaining = regrowthInterval;
                    }
                }
                else // Catch tries to regrow first
                {
                    if (regrowthAllowed && missingPart != null) // If regrowth is permitted and there is a missing part, start the timer
                    {
                        regrowTicksRemaining = regrowthInterval;
                    }
                    if (healAllowed && wounds.Count > 0) // If healing is permitted and there are wounds to heal, start a timer
                    {
                        healTicksRemaining = healInterval;
                    }
                }
                healInProgress |= (healTicksRemaining > 0 || regrowTicksRemaining > 0);
            }
        }

        // Get wounds is the method that checks all of a pawn's hediffs for wounds. This is only used when the pawn is about to be checked to minimize performance impact
        private void GetWounds()
        {
            wounds = new List<Hediff_Injury>();
            for (int i = 0; i < Pawn.health.hediffSet.hediffs.Count; i++)
            {
                Hediff_Injury hediff_Injury = Pawn.health.hediffSet.hediffs[i] as Hediff_Injury;

                // To avoid weird issues caused by reverse regen, a hard limit is placed on how much this can increase severity on a part's wound
                if (hediff_Injury != null && hediff_Injury.Severity < hediff_Injury.Part.def.hitPoints - 0.1)
                {
                    wounds.Add(hediff_Injury);
                }
            }
        }
        public void GetSet() // This checks to see if the new regen set allows for regrowing parts. If not, it will stop the current regrowth.
        {
            foreach (RegenSet regenSet in Props.regenSets)
            {
                if (parent.Severity >= regenSet.minSeverity && parent.Severity <= regenSet.maxSeverity)
                {
                    if (regenSet.ticksToRegrowPart > 0)
                    { 
                        regrowthAllowed = true;
                        regrowthInterval = regenSet.ticksToRegrowPart;
                    }
                    else { regrowthAllowed = false; }


                    if (regenSet.ticksToHealInterval > 0)
                    {
                        healInterval = regenSet.ticksToHealInterval;
                        healAllowed = true; 
                    }
                    else { healAllowed = false; }
                    healAmount = regenSet.healAmount;
                    repeatCount = regenSet.repeatHealCount;

                    tempMinSeverity = regenSet.minSeverity;
                    tempMaxSeverity = regenSet.maxSeverity;
                    break;
                }
            }
        }
    }
}
