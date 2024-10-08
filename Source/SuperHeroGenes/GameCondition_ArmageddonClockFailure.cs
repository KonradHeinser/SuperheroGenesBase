﻿using UnityEngine;
using Verse;
using RimWorld;

namespace SuperHeroGenesBase
{
    public class GameCondition_ArmageddonClockFailure : GameCondition_Planetkiller
    {
        private static readonly Color FadeColor = Color.white;
        public override void End()
        {
            base.End();
            ScreenFader.SetColor(Color.clear);
            GenGameEnd.EndGameDialogMessage("SHG_VillainEndVictory".Translate(Find.World.info.name), false, FadeColor);
        }
    }
}
