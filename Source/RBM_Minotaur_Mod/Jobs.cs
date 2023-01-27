using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;

namespace RBM_Minotaur
{
    public class JobGiver_Milking : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            if (pawn.Spawned && pawn.abilities?.GetAbility(RBM_DefOf.RBM_Lactation)?.CanCast == true && pawn.workSettings.GetPriority(RBM_DefOf.BasicWorker) != 0)
            {
                Log.Message("Milking job given to " + pawn.Name);
                IntVec3 cellDest;
                if (RBM_Utils.TryFindMilkingSpot(pawn, out cellDest))
                {
                    LocalTargetInfo target_location = new LocalTargetInfo(cellDest);
                    LocalTargetInfo target_pawn = new LocalTargetInfo(pawn);
                    Job job = JobMaker.MakeJob(RBM_DefOf.JobDriver_GotoAndUseAbility, target_pawn, target_location);
                    //job.ability = pawn.abilities?.GetAbility(RBM_DefOf.RBM_Lactation);

                    job.preventFriendlyFire = true;
                    job.verbToUse = pawn.abilities?.GetAbility(RBM_DefOf.RBM_Lactation).verb;
                    return job;
                }
                else
                {
                    Log.Message("...Could not find Milking Spot");
                }
            }
            return null;
        }

    }
    public class JobDriver_GotoAndUseAbility : JobDriver_CastAbility
    {
        protected override IEnumerable<Toil> MakeNewToils()
        {
            if (ReservationUtility.Reserve(pawn, TargetB, job))
            {
                this.FailOnDespawnedOrNull(TargetIndex.A);
                yield return Toils_Goto.Goto(TargetIndex.B, PathEndMode.OnCell);

                Toil cast = Toils_Combat.CastVerb(TargetIndex.A, TargetIndex.B);
                cast.WithProgressBar(TargetIndex.A, () => job.verbToUse.WarmupProgress);
                yield return cast;

                Log.Message("I finished casting");
                yield break;
            }
            Log.Message("Tried to start job with reserved object");
            yield break;
        }
        public override string GetReport()
        {
            return "Going to milk";
        }
    }
}
