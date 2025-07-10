using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.SketchGen;
using RimWorld.BaseGen;
using RimWorld;
using Verse;
using Verse.AI;

namespace SuperHeroGenesBase
{
    public class SketchResolver_ConditionCauserRoom : SketchResolver
    {
        protected override bool CanResolveInt(SketchResolveParams parms)
        {
            if (parms.rect.HasValue)
                return parms.sketch != null;
            return false;

        }

        protected override void ResolveInt(SketchResolveParams parms)
        {
            return;
        }
    }
}
