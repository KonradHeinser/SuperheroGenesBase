using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class CompAbilityEffect_AbsorbPower : CompAbilityEffect
    {
        public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
        {
            return target.Thing != null && target.Thing is Corpse;
        }

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            if (target.Thing != null && target.Thing is Corpse c)
            {
                Pawn caster = parent.pawn;
                float severity = c.InnerPawn.BodySize * 0.1f;
                if (c.InnerPawn.RaceProps.IsMechanoid)
                    caster.AddOrAppendHediffs(severity, severity, SHGDefOf.SHG_EverEvolving_Foodless);
                else if ((c.InnerPawn.RaceProps.Insect || c.InnerPawn.IsEntity) && ModsConfig.IsActive("EBSG.Framework"))
                    caster.AddOrAppendHediffs(severity, severity, SHGDefOf.SHG_EverEvolving_Lethality);
                else
                    caster.AddOrAppendHediffs(severity, severity, SHGDefOf.SHG_EverEvolving_Enlightenment);
                c.Destroy();
            }
        }
    }
}
