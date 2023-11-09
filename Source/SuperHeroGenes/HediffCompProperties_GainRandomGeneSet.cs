﻿using System;
using Verse;
using System.Collections.Generic;
using RimWorld;

namespace SuperHeroGenesBase
{
    public class HediffCompProperties_GainRandomGeneSet : HediffCompProperties
    {
        public List<RandomXenoGenes> geneSets; // Named RandomXenoGenes because it was originally solely to create entire xenotypes
        public bool removeGenesFromOtherLists = true; // This being true means that while activating, the comp will remove any gene that exists on the other list(s), even if they are from the xenotype
        public bool inheritable = true; // The default behaviour is to make the genes inheritable(germline)
        public int delayTicks = 10; // How long it waits until triggering. Will always wait at least 10 ticks, even if a lower number is given
        public List<GeneDef> alwaysAddedGenes; // These genes will be added regardless of the set picked
        public List<GeneDef> alwaysRemovedGenes; // These genes will be removed regardless of set picked
        public float minSeverity = 0; // Sets the minimum severity that the hediff must be at to start gene generation. Once this is reached, the hediff will disappear shortly after
        public float maxSeverity = 999; // If a hediff goes down in severity, use this instead of min

        /// For those interested, the order things are done in is 
        ///  1) Random set selection. A random number is selected between 0 and the total weight, and then from that a random set is picked, with lower numbers selecting earlier sets.
        ///  2) If remove removeGenesFromOtherLists is true, the program will iterate through all available lists, and remove any genes that the pawn has.
        ///    2.5) If removeGenesFromOtherLists is false, then all of the sets are checked for an override. If a set has an override, then that set is still removed from the pawn if the pawn has any of them.
        ///  3) Genes from alwaysAddedGenes that the pawn doesn't have are added to the pawn.
        ///  4) Genes from the selected set that the pawn doesn't have are added to the pawn.
        ///  5) Genes from alwaysRemovedGenes that the pawn has are removed from the pawn. If a gene is giving this hediff, and you want it removed, I recommend making it last on this list.
        ///  6) Hediff is removed
        /// 
        /// One of the reasons this was added to a hediffcomp instead of a gene extension to make it easier for fellow modders to create items that add items that add genes to pawns with items
        /// If you need an example of this, take a look at the Super Creating Pill, which does nothing but add the superhero gene to the pawn's germline genes

        public HediffCompProperties_GainRandomGeneSet()
        {
            compClass = typeof(HediffComp_GainRandomGeneSet);
        }
    }
}
