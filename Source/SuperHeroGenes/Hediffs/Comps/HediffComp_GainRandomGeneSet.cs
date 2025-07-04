using Verse;

namespace SuperHeroGenesBase
{
    public class HediffComp_GainRandomGeneSet : HediffComp
    {
        public HediffCompProperties_GainRandomGeneSet Props => (HediffCompProperties_GainRandomGeneSet)props;
        public int delayTicks = 10;

        public override void CompPostTickInterval(ref float severityAdjustment, int delta)
        {
            base.CompPostTickInterval(ref severityAdjustment, delta);

            delayTicks -= delta;
            if (delayTicks <= 0)
            {
                SHGUtilities.GainRandomGeneSet(parent.pawn, Props.inheritable, Props.removeGenesFromOtherLists, Props.geneSets, Props.alwaysAddedGenes, Props.alwaysRemovedGenes, Props.showMessage);
                if (parent.pawn.health.hediffSet.GetFirstHediffOfDef(parent.def) != null)
                    parent.pawn.health.RemoveHediff(parent.pawn.health.hediffSet.GetFirstHediffOfDef(parent.def));
            }
        }

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Values.Look(ref delayTicks, "SHG_GeneSetDelayTicks", 10);
        }
    }
}
