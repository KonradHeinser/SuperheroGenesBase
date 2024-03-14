using Verse;

namespace SuperHeroGenesBase
{
    public class HediffComp_ExplodeOnDeath : BurstHediffCompBase
    {
        public new HediffCompProperties_ExplodeOnDeath Props => (HediffCompProperties_ExplodeOnDeath)props;

        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {
            DoExplosion(parent.pawn.Corpse.Position);
        }
    }
}
