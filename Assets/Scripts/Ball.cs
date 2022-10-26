using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapedHorse.BallBattle
{
    public class Ball : MonoBehaviour
    {
        public enum State { Hold, Idle }
        public State status;
        public Animator ballAnim;
                
        public void Passed(Transform target)
        {

        }

        public void Hold(AttackerSoldier soldier)
        {
            transform.SetParent(soldier.transform);
            ballAnim.SetBool("Hold", true);
        }
    }
}

