using Verse;
namespace SuperHeroGenesBase
{
    public class HediffComp_ExplodeWhenDamaged : BurstHediffCompBase
    {
        public new HediffCompProperties_ExplodeWhenDamaged Props => (HediffCompProperties_ExplodeWhenDamaged)props;

        public int cooldownTicks = 0; // Not saved because this is just to avoid performance issues

        public override void CompPostTickInterval(ref float severityAdjustment, int delta)
        {
            base.CompPostTickInterval(ref severityAdjustment, delta);

            if (cooldownTicks > 0)
                cooldownTicks -= delta;
        }

        public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            if (cooldownTicks <= 0 && parent.Severity >= Props.minSeverity && parent.Severity < Props.maxSeverity)
            {
                DoExplosion(parent.pawn.Position);
                cooldownTicks = Props.cooldownTicks;
            }
        }
    }
}
