﻿using System.Collections.Generic;
using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class GRCExtension : DefModExtension
    {
        /// The Genetic Romance Chance Extension allows for more options on romancing ability.
        /// While non-percent stats can be used, it's not recommended as they can end up giving unexpected results. It is recommended that only percent stats with a default of 1 are used
        /// This CAN make the romance chance 0 if the relevant stats are 0
        /// This ONLY affects the carrier's ability to romance a target

        public StatDef carrierStat; // Stat on the carrier that multiplies romance chance
        public StatDef otherStat; // Stat on target that multiplies romance chance
        public bool onlyWhileLoweredCarrier; // Only use romancing stat if it is below 1
        public bool onlyWhileRaisedCarrier; // Only use romancing stat if it is above 1
        public bool onlyWhileLoweredOther; // Only use target's stat if it is below 1
        public bool onlyWhileRaisedOther; // Only use romancing stat if it is above 1
    }
}
