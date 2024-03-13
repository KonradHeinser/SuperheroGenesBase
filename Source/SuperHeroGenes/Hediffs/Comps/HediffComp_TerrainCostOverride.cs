using Verse;

namespace SuperHeroGenesBase
{
    public class HediffComp_TerrainCostOverride : HediffComp
    {
        public HediffCompProperties_TerrainCostOverride Props => (HediffCompProperties_TerrainCostOverride)props;

        private SHGCache_Component cache;

        public SHGCache_Component Cache
        {
            get
            {
                if (cache == null)
                    cache = Current.Game.GetComponent<SHGCache_Component>();

                if (cache != null && cache.loaded)
                    return cache;
                return null;
            }
        }

        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            if (Cache != null)
                cache.RegisterTerrainPawn(Pawn, parent);
        }

        public override void CompPostPostRemoved()
        {
            if (Cache != null)
                cache.DeRegisterTerrainPawn(Pawn);
        }
    }
}
