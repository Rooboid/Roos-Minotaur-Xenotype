using Verse;
using RimWorld;
using Verse.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Rimworld
{
    [DefOf]
    public static class DamageDefOf
    {
        public static DamageDef Terrified;
    }

    /*
[DefOf]
public static class HediffDefOf
{
    public static HediffDef HeDiffTerrified;
}


public class JobGiver_FleeSeeingRed : ThinkNode_JobGiver
{

    protected override Job TryGiveJob(Pawn pawn)
    {
        for (int index = 0; index < this.hediffs.Count; index++)
        {
            if (pawn.health.hediffSet.HasHediff())
            {
                HediffStage curStage = this.hediffs[index].CurStage;
                if (curStage != null)
                {
                    a *= curStage.hungerRateFactor;
                }
            }
        }
        {
            return null;
        }
        pawn.jobs.StartJob(flee)

        Thing enragedPawn = pawn.mindState.knownExploder;
        if ((float)(pawn.Position - knownExploder.Position).LengthHorizontalSquared > 81f)
        {
            return null;
        }
        IntVec3 result;
        if (!RCellFinder.TryFindDirectFleeDestination(knownExploder.Position, 9f, pawn, out result))
        {
            return null;
        }
        Job job = JobMaker.MakeJob(JobDefOf.Goto, result);
        job.locomotionUrgency = LocomotionUrgency.Sprint;
        return job;
    }

    public const float FleeDist = 9f;
    */

    public class CompAbilityEffect_Terrify : CompAbilityEffect
    {
        public new CompProperties_AbilityTerrify Props
        {
            get
            {
                return (CompProperties_AbilityTerrify)this.props;
            }
        }
        public bool ShouldHaveInspectString
        {
            get
            {
                return ModsConfig.BiotechActive;
            }
        }
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            GenExplosion.DoExplosion(target.Cell, this.parent.pawn.MapHeld, this.Props.terrorRadius, DamageDefOf.Terrified, null, -1, -1f, null, null, null, null, null, 0f, 1, null, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f);
        }

        public override void DrawEffectPreview(LocalTargetInfo target)
        {
            GenDraw.DrawRadiusRing(target.Cell, this.Props.terrorRadius);
        }
    }


    public class CompProperties_AbilityTerrify : CompProperties_AbilityEffect
    {
        public CompProperties_AbilityTerrify()
        {
            this.compClass = typeof(CompAbilityEffect_Terrify);
        }
        public float terrorRadius;
    }
}
