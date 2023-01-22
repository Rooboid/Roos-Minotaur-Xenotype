using UnityEngine;
using Verse;

namespace RBM_Minotaur
{
    internal class RBM_Utils
    {
        //Generate a tile to flee to
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

        public static Vector3 GetDirection(Vector3 from, Vector3 to)
        {
            Vector3 relativePos = (to - from);
            float distance = relativePos.magnitude;
            Vector3 direction = relativePos / distance;
            return direction;
        }

        public static Vector3 GetDirection(IntVec3 from, IntVec3 to)
        {
            return GetDirection(from.ToVector3(), to.ToVector3());
        }


    }
}
