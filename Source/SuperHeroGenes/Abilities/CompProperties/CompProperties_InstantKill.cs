﻿using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class CompProperties_InstantKill : CompProperties_AbilityEffect
    {
        public bool deleteCorpse = true;

        public DamageDef damageDefToReport;

        public SoundDef explosionSound;

        public ThingDef filthReplacement;

        public IntRange bloodFilthToSpawnRange;

        public bool multiplyBloodByBodySize = true;

        public CompProperties_InstantKill()
        {
            compClass = typeof(CompAbilityEffect_InstantKill);
        }
    }
}
