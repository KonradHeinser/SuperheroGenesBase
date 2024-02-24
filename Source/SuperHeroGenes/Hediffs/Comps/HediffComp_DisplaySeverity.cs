using Verse;
using System;

namespace SuperHeroGenesBase
{
    public class HediffComp_DisplaySeverity : HediffComp
    {
        public HediffCompProperties_DisplaySeverity Props => (HediffCompProperties_DisplaySeverity)props;

        public override string CompTipStringExtra
        {
            get
            {
                string output = Props.prependString;

                if (output == null) output = Math.Round((decimal)(parent.Severity * Props.postFactor), Props.roundDigits).ToString();
                else output += (parent.Severity * Props.postFactor).ToString();

                if (Props.appendString != null && output != null) output += Props.appendString;

                return output;
            }
        }
    }
}
