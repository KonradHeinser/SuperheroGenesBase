using UnityEngine;
using Verse;

namespace SuperHeroGenesBase
{
    public class HediffCompProperties_AttachMote : HediffCompProperties
    {
        public ThingDef moteDef;

        public Color color = new Color(1f, 1f, 1f);

        public bool brightnessBySeverity;

        public float staticBrightness = 1f;

        public bool displayWhileDowned = true;

        public bool rotateWithPawn = false;

        public bool scaleMoteWithSize = true;

        public HediffCompProperties_AttachMote()
        {
            compClass = typeof(HediffComp_AttachMote);
        }
    }
}
