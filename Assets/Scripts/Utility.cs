using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapedHorse.BallBattle
{
    public class Utility : MonoBehaviour
    {
        public static Soldier NearestToTarget(List<Soldier> soldier, Transform target)
        {
            Soldier tMin = null;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = target.position;
            foreach (Soldier t in soldier)
            {
                float dist = Vector3.Distance(t.transform.position, currentPos);
                if (dist < minDist)
                {
                    tMin = t;
                    minDist = dist;
                }
            }
            return tMin;
        }
    }
}

