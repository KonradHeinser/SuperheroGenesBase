using Verse;
namespace SuperHeroGenesBase
{
    public class HediffComp_ExplodingMeleeAttacks : BurstHediffCompBase
    {
        public new HediffCompProperties_ExplodingMeleeAttacks Props => (HediffCompProperties_ExplodingMeleeAttacks)props;

        public bool currentlyExploding = false;
        public int explosionCooldown = -1;

        public override void CompPostTickInterval(ref float severityAdjustment, int delta)
        {
            base.CompPostTickInterval(ref severityAdjustment, delta);

            if (currentlyExploding && explosionCooldown < 0)
                explosionCooldown = 10;
            else if (currentlyExploding)
                if (explosionCooldown <= 0)
                    currentlyExploding = false;
                else
                    explosionCooldown -= delta;
        }
    }
}