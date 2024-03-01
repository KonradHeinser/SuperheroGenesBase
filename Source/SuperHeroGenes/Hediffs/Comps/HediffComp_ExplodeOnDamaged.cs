using Verse;
namespace SuperHeroGenesBase
{
    public class HediffComp_ExplodeOnDamaged : BurstHediffCompBase
    {
        public new HediffCompProperties_ExplodeWhenDamaged Props => (HediffCompProperties_ExplodeWhenDamaged)props;

        public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            if (parent.Severity >= Props.minSeverity)
                DoExplosion(parent.pawn.Position);
        }
    }
}
