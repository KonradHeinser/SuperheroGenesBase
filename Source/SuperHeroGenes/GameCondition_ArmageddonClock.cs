using System;
using UnityEngine;
using Verse;
using RimWorld;

namespace SuperHeroGenesBase
{
    public class GameCondition_ArmageddonClock : GameCondition_Planetkiller
    {
        public override void End()
        {
            base.End();
            GenGameEnd.EndGameDialogMessage("SHG_VillainEnd".Translate(Find.World.info.name), allowKeepPlaying: false);
        }
    }
}
