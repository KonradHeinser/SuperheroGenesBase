using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class CompAbilityEffect_AbsorbPower : CompAbilityEffect
    {
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            if (target.Thing != null && target.Thing is Corpse c)
            {
                Pawn caster = parent.pawn;
                float severity = c.InnerPawn.BodySize * 0.01f;
                if (c.InnerPawn.RaceProps.IsMechanoid)
                    caster.AddOrAppendHediffs(severity, severity, SHGDefOf.SHG_EverEvolving_Foodless);
                else if ((c.InnerPawn.RaceProps.Insect || c.InnerPawn.IsEntity) && ModsConfig.IsActive("EBSG.Framework"))
                    caster.AddOrAppendHediffs(severity, severity, SHGDefOf.SHG_EverEvolving_Lethality);
                else
                    caster.AddOrAppendHediffs(severity, severity, SHGDefOf.SHG_EverEvolving_Enlightenment);
            }
        }
    }
}
