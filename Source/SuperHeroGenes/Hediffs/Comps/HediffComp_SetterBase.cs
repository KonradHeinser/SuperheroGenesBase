using Verse;

namespace SuperHeroGenesBase
{
    public class HediffComp_SetterBase : HediffComp
    {
        protected int ticksToNextCheck;

        protected virtual bool MustBeSpawned => false;

        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            base.CompPostPostAdd(dinfo);

            if (!MustBeSpawned || Pawn.Spawned)
                SetSeverity();
        }

        public override void CompPostTickInterval(ref float severityAdjustment, int delta)
        {
            base.CompPostTickInterval(ref severityAdjustment, delta);

            if (!MustBeSpawned || Pawn.Spawned)
            {
                if (ticksToNextCheck <= 0 || DoCheck())
                    SetSeverity();
                else
                    ticksToNextCheck -= delta;
            }
        }

        protected virtual bool DoCheck()
        {
            return true;
        }

        protected virtual void SetSeverity()
        {
        }
    }
}
