using System.Collections.Generic;
using RimWorld;
using Verse;

namespace SuperHeroGenesBase
{
    public class Need_BloodThirst : Need
    {
        protected override bool IsFrozen
        {
            get
            {
                if (pawn.ageTracker.AgeBiologicalYearsFloat < 13f || pawn.WorkTagIsDisabled(WorkTags.Violent))
                    return true;

                return base.IsFrozen;
            }
        }

        public override bool ShowOnNeedList
        {
            get
            {
                if (pawn.ageTracker.AgeBiologicalYearsFloat < 13f)
                    return false;

                return base.ShowOnNeedList;
            }
        }

        public Need_BloodThirst(Pawn newPawn) : base(newPawn)
        {
            threshPercents = new List<float> { 0.3f };
        }

        public override void NeedInterval()
        {
            if (!IsFrozen)
                CurLevel -= 0.0333f / 400f; // 30 Days
        }

        public void Notify_KilledPawn(DamageInfo? dinfo, Pawn victim)
        {
            if (victim.RaceProps.IsMechanoid || !victim.health.CanBleed) return;
            CurLevel += 0.2f;

            if (dinfo.HasValue && (dinfo?.WeaponBodyPartGroup != null || dinfo?.WeaponLinkedHediff != null || dinfo.Value.Weapon != null))
                if (dinfo?.WeaponBodyPartGroup != null || dinfo?.WeaponLinkedHediff != null || (dinfo.Value.Weapon != null && dinfo.Value.Weapon.IsMeleeWeapon))
                    CurLevel += 0.3f;
        }
    }
}
