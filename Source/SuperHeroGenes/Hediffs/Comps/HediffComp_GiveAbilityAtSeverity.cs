using Verse;
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
            if (Pawn?.abilities == null) return;
            previousSeverity = parent.Severity;
            validAbilities = new List<AbilityDef>();

            foreach (var severitySet in Props.abilitiesAtSeverities)
            {
                if (parent.Severity >= severitySet.minSeverity && parent.Severity <= severitySet.maxSeverity)
                {
                    if (severitySet.abilityDef != null)
                    {
                        Pawn.abilities.GainAbility(severitySet.abilityDef);
                        validAbilities.Add(severitySet.abilityDef);
                    }

                    if (!severitySet.abilityDefs.NullOrEmpty())
                        for (var i = 0; i < severitySet.abilityDefs.Count; i++)
                        {
                            Pawn.abilities.GainAbility(severitySet.abilityDefs[i]);
                            validAbilities.Add(severitySet.abilityDefs[i]);
                        }
                }
                else if (!Pawn.abilities.AllAbilitiesForReading.NullOrEmpty())
                    foreach (var ability in Pawn.abilities.AllAbilitiesForReading)
                    {
                        if (severitySet.abilityDef != null && (validAbilities.NullOrEmpty() || !validAbilities.Contains(severitySet.abilityDef)))
                            if (ability.def == severitySet.abilityDef)
                                Pawn.abilities.RemoveAbility(severitySet.abilityDef);

                        if (!severitySet.abilityDefs.NullOrEmpty())
                            if (severitySet.abilityDefs.Contains(ability.def) && (validAbilities.NullOrEmpty() || !validAbilities.Contains(ability.def)))
                                Pawn.abilities.RemoveAbility(ability.def);
                    }
            }
        }

        public override void CompPostPostRemoved()
        {
            if (Pawn?.abilities == null) return;
            foreach (var severitySet in Props.abilitiesAtSeverities)
            {
                if (severitySet.abilityDef != null)
                    Pawn.abilities.RemoveAbility(severitySet.abilityDef);

                if (!severitySet.abilityDefs.NullOrEmpty())
                    for (var i = 0; i < severitySet.abilityDefs.Count; i++)
                        Pawn.abilities.RemoveAbility(severitySet.abilityDefs[i]);
            }
        }
    }
}
