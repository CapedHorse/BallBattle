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

        void OnEnable()
        {
            
            EventManager.OnBallHolded += Hold;
        }

        void OnDisable()
        {
            
            EventManager.OnBallHolded -= Hold;
        }
        public void Hold(AttackerSoldier soldier)
        {
            transform.SetParent(soldier.transform);
            ballAnim.SetBool("Hold", true);
        }

        public void Loose()
        {
            transform.SetParent(GameManager.instance.field);
            ballAnim.SetBool("Hold", false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Goal")
            {
                EventManager.OnGoal?.Invoke();
            }   
        }
    }
}

