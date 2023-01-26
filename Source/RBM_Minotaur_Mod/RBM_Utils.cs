using RimWorld;
using RimWorld.BaseGen;
using System.Collections.Generic;
using System.Data.Common;
using System.Security.Cryptography;
using UnityEngine;
using Verse;
using Verse.AI;
using static HarmonyLib.Code;
using static UnityEngine.GraphicsBuffer;

namespace RBM_Minotaur
{
    internal class RBM_Utils
    {
        // Generate a tile to flee to
        public static IntVec3 genFleeTile(Vector3 startPosition, Vector3 fleeFrom, int fleeDistance)
        {
            Vector3 relativePos = (startPosition - fleeFrom);
            Vector3 NormalizedDirection = relativePos.normalized;

            Vector3 fleeToVector = startPosition + (NormalizedDirection * fleeDistance);
            IntVec3 fleeToIntVec = fleeToVector.ToIntVec3();

            return fleeToIntVec;
        }
        public static IntVec3 genFleeTile(IntVec3 startPosition, IntVec3 fleeFrom, int fleeDistance)
        {
            return genFleeTile(startPosition.ToVector3(), fleeFrom.ToVector3(), fleeDistance);
        }


        // Get Heading from two coordinates
        public static Vector3 getDirection(Vector3 from, Vector3 to)
        {
            Vector3 relativePos = (to - from);
            float distance = relativePos.magnitude;
            Vector3 direction = relativePos / distance;
            return direction;
        }
        public static Vector3 getDirection(IntVec3 from, IntVec3 to)
        {
            return getDirection(from.ToVector3(), to.ToVector3());
        }


        // Apply terrify effect in an area
        public static bool terrifyInArea(IntVec3 position, Map map, float radius = 5)
        {
            if (map == null) { return false; }
            List<Pawn> mapPawns = map.mapPawns.AllPawnsSpawned;

            for (int i = 0; i < mapPawns.Count; i++)
            {
                bool isHumanlike = mapPawns[i].RaceProps.Humanlike;
                bool isInRange = position.InHorDistOf(mapPawns[i].Position, radius);
                bool isDowned = mapPawns[i].Downed;
                bool isInMentalState = mapPawns[i].InMentalState;
                
                if ( isHumanlike && isInRange && !isDowned && !isInMentalState )
                {
                    LocalTargetInfo t = new LocalTargetInfo(RBM_Utils.genFleeTile(mapPawns[i].Position, position, 10));
                    Job job = new Job(JobDefOf.FleeAndCower, t);
                    mapPawns[i].mindState.mentalStateHandler.TryStartMentalState(RBM_DefOf.RBM_TerrifiedFlee, "scared by something nearby", true, false, null, true);
                    mapPawns[i].jobs.TryTakeOrderedJob(job, JobTag.Misc);
                }
            }
            return true;
        }




    }
}
