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

        public ThingDef cornerThing;

        protected override void ResolveInt(SketchResolveParams parms)
        {
            if (cornerThing != null)
            {
                SketchResolveParams parms2 = parms;
                parms2.cornerThing = cornerThing;
                parms2.requireFloor = true;
                SketchResolverDefOf.AddCornerThings.Resolve(parms2);
            }
        }
    }
}
