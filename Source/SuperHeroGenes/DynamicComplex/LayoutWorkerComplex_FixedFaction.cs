using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class LayoutWorkerComplex_FixedFaction : LayoutWorkerComplex
    {
        public LayoutWorkerComplex_FixedFaction(LayoutDef def)
        : base(def)
        {
        }

        public override Faction GetFixedHostileFactionForThreats()
        {
            if (!Def.threats.NullOrEmpty() && Def.threats[0].def.faction != null)
            {
                return Find.FactionManager.FirstFactionOfDef(Def.threats[0].def.faction);
            }

            return null;
        }
    }
}
