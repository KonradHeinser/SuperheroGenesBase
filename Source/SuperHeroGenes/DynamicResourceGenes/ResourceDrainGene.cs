using System.Collections.Generic;
using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class ResourceDrainGene : Gene, IGeneResourceDrain
    {
        [Unsaved(false)]
        private ResourceGene cachedResourceGene;

        DRGExtension extension = null;

        public bool extensionAlreadyChecked = false;

        public List<AbilityDef> addedAbilities;

        public int cachedGeneCount = 0;

        public bool CanOffset
        {
            get
            {
                if (Active && !pawn.Dead)
                {
                    if (extension == null && !extensionAlreadyChecked)
                    {
                        extension = def.GetModExtension<DRGExtension>();
                        extensionAlreadyChecked = true;
                    }
                    if (extension != null)
                    {
                        if (pawn.Spawned)
                        {
                            float time = GenLocalDate.DayPercent(Pawn);
                            if (time < extension.startTime || time > extension.endTime) return false;

                            if (pawn.Map != null)
                            {
                                float light = pawn.Map.glowGrid.GameGlowAt(pawn.Position, false);
                                if (light < extension.minLightLevel || light > extension.maxLightLevel) return false;
                            }
                        }

                        if (!extension.requireOneOfHediffs.NullOrEmpty() && !SHGUtilities.PawnHasAnyOfHediffs(pawn, extension.requireOneOfHediffs)) return false;
                        if (!SHGUtilities.PawnHasAllOfHediffs(pawn, extension.requiredHediffs)) return false;
                        if (SHGUtilities.PawnHasAnyOfHediffs(pawn, extension.forbiddenHediffs)) return false;

                        if (!SHGUtilities.AllNeedLevelsMet(pawn, extension.needLevels)) return false;
                    }
                    return true;
                }
                return false;
            }
        }


        private const float MinAgeForDrain = 3f;

        public Gene_Resource Resource
        {
            get
            {
                if (!def.HasModExtension<DRGExtension>()) Log.Error(def + "doesn't have the DRG extension, meaning the main resource gene cannot be found.");
                else if (cachedResourceGene == null || !cachedResourceGene.Active)
                {
                    cachedResourceGene = (ResourceGene)pawn.genes.GetGene(def.GetModExtension<DRGExtension>().mainResourceGene);
                }
                return cachedResourceGene;
            }
        }

        public float ResourceLossPerDay => def.resourceLossPerDay;

        public Pawn Pawn => pawn;

        public string DisplayLabel => Label + " (" + "Gene".Translate() + ")";

        public override void PostAdd()
        {
            base.PostAdd();
            SHGExtension SHGextension = def.GetModExtension<SHGExtension>();
            if (SHGextension != null && !SHGextension.hediffsToApply.NullOrEmpty())
            {
                HediffAdder.HediffAdding(pawn, this);
                if (addedAbilities == null) addedAbilities = new List<AbilityDef>();
                cachedGeneCount = pawn.genes.GenesListForReading.Count;
            }
        }

        public override void PostRemove()
        {
            base.PostRemove();
            HediffAdder.HediffRemoving(pawn, this);

            if (!addedAbilities.NullOrEmpty())
            {
                SHGUtilities.RemovePawnAbilities(pawn, addedAbilities);
            }
        }

        public override void Tick()
        {
            base.Tick();

            if (pawn.IsHashIntervalTick(200))
            {
                SHGExtension SHGextension = def.GetModExtension<SHGExtension>();
                if (SHGextension != null && !SHGextension.geneAbilities.NullOrEmpty() && pawn.genes.GenesListForReading.Count != cachedGeneCount)
                {
                    if (addedAbilities == null) addedAbilities = new List<AbilityDef>();
                    addedAbilities = SHGUtilities.AbilitiesWithCertainGenes(pawn, SHGextension.geneAbilities, addedAbilities);
                    cachedGeneCount = pawn.genes.GenesListForReading.Count;
                }
            }

            if (extension == null && !extensionAlreadyChecked)
            {
                extension = def.GetModExtension<DRGExtension>();
                extensionAlreadyChecked = true;
            }
            if (Resource != null && CanOffset) ResourceGene.OffsetResource(pawn, ResourceLossPerDay * -1, cachedResourceGene, extension, false, true);
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo resourceDrainGizmo in GeneResourceDrainUtility.GetResourceDrainGizmos(this))
            {
                yield return resourceDrainGizmo;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref addedAbilities, "SHG_DRGDAddedAbilities");
        }
    }
}
