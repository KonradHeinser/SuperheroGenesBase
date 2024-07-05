using RimWorld;
using Verse;
using System.Collections.Generic;
using Verse.Sound;
using UnityEngine;

namespace SuperHeroGenesBase
{
    public class CompAbilityEffect_TeleMoveIt : CompAbilityEffect
    {
        public new CompProperties_AbilityTeleMoveIt Props => (CompProperties_AbilityTeleMoveIt)props;

        public IntVec3 Destination(LocalTargetInfo target)
        {
            if (!target.HasThing || !(target.Thing is Pawn pawn) || target.Thing.Map == null || Props.distanceToMoveIt == 0) return IntVec3.Invalid;

            IntVec3 casterLocation = parent.pawn.Position;
            IntVec3 targetLocation = target.Thing.Position;

            if (Props.distanceToMoveIt < 0 && casterLocation.DistanceTo(targetLocation) <= Props.distanceToMoveIt * -1)
                return casterLocation;

            float xDiff = targetLocation.x - casterLocation.x;
            float zDiff = targetLocation.z - casterLocation.z;
            float absDiff = Mathf.Abs(xDiff) + Mathf.Abs(zDiff);

            IntVec3 newLocation = new IntVec3
            {
                x = Mathf.CeilToInt(xDiff == 0 ? targetLocation.x : // If the x's are the same then the pawn only moves along the y
                zDiff == 0 ? targetLocation.x + Props.distanceToMoveIt : // If the z's are the same then the pawn only moves along the x
                targetLocation.x + AxisChange(xDiff, absDiff)),

                z = Mathf.CeilToInt(xDiff == 0 ? targetLocation.z + Props.distanceToMoveIt : // If the x's are the same then the pawn only moves along the y
                zDiff == 0 ? targetLocation.z : // If the z's are the same then the pawn only moves along the x
                targetLocation.z + AxisChange(zDiff, absDiff))
            };

            if (newLocation.IsValid && newLocation.InBounds(target.Thing.Map) && !newLocation.Impassable(target.Thing.Map))
                return newLocation;

            return IntVec3.Invalid;
        }

        private float AxisChange(float axisDiff, float absDiff)
        {
            // Probably not getting a super accurate change amount, but it should be decentish
            float baseChange = Mathf.Sqrt(Mathf.Pow(axisDiff / absDiff, 2) * Mathf.Pow(Props.distanceToMoveIt, 2));
            if (axisDiff < 0)
                baseChange *= -1;
            if (Props.distanceToMoveIt < 0)
                baseChange *= -1;

            return baseChange;
        }

        public override IEnumerable<PreCastAction> GetPreCastActions()
        {
            yield return new PreCastAction
            {
                action = delegate (LocalTargetInfo t, LocalTargetInfo d)
                {
                    IntVec3 destination = Destination(t);
                    if (!parent.def.HasAreaOfEffect && destination.IsValid)
                    {
                        Pawn pawn = t.Pawn;
                        if (pawn != null)
                        {
                            FleckCreationData dataAttachedOverlay = FleckMaker.GetDataAttachedOverlay(pawn, FleckDefOf.PsycastSkipFlashEntry, new Vector3(-0.5f, 0f, -0.5f));
                            dataAttachedOverlay.link.detachAfterTicks = 5;
                            pawn.Map.flecks.CreateFleck(dataAttachedOverlay);
                        }
                        else
                        {
                            FleckMaker.Static(t.CenterVector3, parent.pawn.Map, FleckDefOf.PsycastSkipFlashEntry);
                        }
                        FleckMaker.Static(destination, parent.pawn.Map, FleckDefOf.PsycastSkipInnerExit);
                    }
                    FleckMaker.Static(destination, parent.pawn.Map, FleckDefOf.PsycastSkipOuterRingExit);
                    if (!parent.def.HasAreaOfEffect)
                    {
                        SoundDefOf.Psycast_Skip_Entry.PlayOneShot(new TargetInfo(t.Cell, parent.pawn.Map));
                        SoundDefOf.Psycast_Skip_Exit.PlayOneShot(new TargetInfo(destination, parent.pawn.Map));
                    }
                },
                ticksAwayFromCast = 5
            };
        }

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            IntVec3 destination = Destination(target);
            if (!destination.IsValid) return;
            base.Apply(target, dest);

            Log.Message(parent.pawn.Label + " : " + parent.pawn.Position.x + " : " + parent.pawn.Position.z);
            Log.Message(target.Thing.Label + " : " + target.Thing.Position.x + " : " + target.Thing.Position.z);
            Log.Message("New position : " + destination.x + " : " + destination.z);

            Pawn pawn = parent.pawn;
            if (!parent.def.HasAreaOfEffect)
                parent.AddEffecterToMaintain(EffecterDefOf.Skip_Entry.Spawn(target.Thing, pawn.Map), target.Thing.Position, 60);
            else
                parent.AddEffecterToMaintain(EffecterDefOf.Skip_EntryNoDelay.Spawn(target.Thing, pawn.Map), target.Thing.Position, 60);

            parent.AddEffecterToMaintain(EffecterDefOf.Skip_Exit.Spawn(destination, pawn.Map), destination, 60);
            target.Thing.TryGetComp<CompCanBeDormant>()?.WakeUp();
            target.Thing.Position = destination;
            if (target.Thing is Pawn pawn2)
            {
                if ((pawn2.Faction == Faction.OfPlayer || pawn2.IsPlayerControlled) && pawn2.Position.Fogged(pawn2.Map))
                    FloodFillerFog.FloodUnfog(pawn2.Position, pawn2.Map);

                pawn2.stances.stunner.StunFor(Props.stunTicks.RandomInRange, parent.pawn, false, false);
                pawn2.Notify_Teleported();
                SendSkipUsedSignal(pawn2.Position, pawn2);
            }
        }

        public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
        {
            return Valid(target, true);
        }

        public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
        {
            if (!target.HasThing || !(target.Thing is Pawn)) return false;
            AcceptanceReport acceptanceReport = CanSkipTarget(target);
            if (!acceptanceReport)
            {
                if (throwMessages && !acceptanceReport.Reason.NullOrEmpty() && target.Thing is Pawn pawn)
                {
                    Messages.Message("CannotSkipTarget".Translate(pawn.Named("PAWN")) + ": " + acceptanceReport.Reason, pawn, MessageTypeDefOf.RejectInput, false);
                }
                return false;
            }

            if (!Destination(target).IsValid)
            {
                if (throwMessages && target.HasThing)
                    Messages.Message("CannotSkipTarget".Translate(target.Thing.Label.Named("PAWN")) + ": " + "SHG_DestinationBlocked".Translate(), MessageTypeDefOf.RejectInput, false);
                return false;
            }

            return base.Valid(target, throwMessages);
        }

        private AcceptanceReport CanSkipTarget(LocalTargetInfo target)
        {
            if (target.Thing is Pawn pawn)
            {
                if (pawn.BodySize > Props.maxBodySize)
                    return "CannotSkipTargetTooLarge".Translate();
                if (pawn.kindDef.skipResistant)
                    return "CannotSkipTargetPsychicResistant".Translate();
            }
            return true;
        }

        public override string ExtraLabelMouseAttachment(LocalTargetInfo target)
        {
            return CanSkipTarget(target).Reason;
        }

        public static void SendSkipUsedSignal(LocalTargetInfo target, Thing initiator)
        {
            Find.SignalManager.SendSignal(new Signal("CompAbilityEffect.SkipUsed", target.Named("POSITION"), initiator.Named("SUBJECT")));
        }
    }
}
