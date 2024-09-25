﻿using Verse;
using RimWorld;
using System.Collections.Generic;

namespace SuperHeroGenesBase
{
    public class HediffComp_GiveAbilityAtSeverity : HediffComp
    {
        private HediffCompProperties_GiveAbilityAtSeverity Props => (HediffCompProperties_GiveAbilityAtSeverity)props;

        public List<AbilityDef> validAbilities = new List<AbilityDef>();

        public float previousSeverity = 0f;

        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            CheckAbilities();
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            if (parent.Severity == previousSeverity || !Pawn.IsHashIntervalTick(60))
                return;

            CheckAbilities();
        }

        private void CheckAbilities()
        {
            previousSeverity = parent.Severity;
            validAbilities = new List<AbilityDef>();

            foreach (AbilitiesAtSeverities severitySet in Props.abilitiesAtSeverities)
            {
                if (parent.Severity >= severitySet.minSeverity && parent.Severity <= severitySet.maxSeverity)
                {
                    if (severitySet.abilityDef != null)
                    {
                        parent.pawn.abilities.GainAbility(severitySet.abilityDef);
                        validAbilities.Add(severitySet.abilityDef);
                    }

                    if (!severitySet.abilityDefs.NullOrEmpty())
                        for (int i = 0; i < severitySet.abilityDefs.Count; i++)
                        {
                            parent.pawn.abilities.GainAbility(severitySet.abilityDefs[i]);
                            validAbilities.Add(severitySet.abilityDefs[i]);
                        }
                }
                else if (!parent.pawn.abilities.AllAbilitiesForReading.NullOrEmpty())
                    foreach (Ability ability in parent.pawn.abilities.AllAbilitiesForReading)
                    {
                        if (severitySet.abilityDef != null && (validAbilities.NullOrEmpty() || !validAbilities.Contains(severitySet.abilityDef)))
                            if (ability.def == severitySet.abilityDef)
                                parent.pawn.abilities.RemoveAbility(severitySet.abilityDef);

                        if (!severitySet.abilityDefs.NullOrEmpty())
                            if (severitySet.abilityDefs.Contains(ability.def) && (validAbilities.NullOrEmpty() || !validAbilities.Contains(ability.def)))
                                parent.pawn.abilities.RemoveAbility(ability.def);
                    }
            }
        }

        public override void CompPostPostRemoved()
        {
            foreach (AbilitiesAtSeverities severitySet in Props.abilitiesAtSeverities)
            {
                if (severitySet.abilityDef != null)
                    parent.pawn.abilities.RemoveAbility(severitySet.abilityDef);

                if (!severitySet.abilityDefs.NullOrEmpty())
                    for (int i = 0; i < severitySet.abilityDefs.Count; i++)
                        parent.pawn.abilities.RemoveAbility(severitySet.abilityDefs[i]);
            }
        }
    }
}
