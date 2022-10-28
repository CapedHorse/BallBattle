using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapedHorse.BallBattle
{
    public class Ball : MonoBehaviour
    {
        public static Ball instance;
        public enum State { Hold, Idle }
        public State status;
        public Animator ballAnim;
        public AttackerSoldier target;
        bool passing;
                
        public void Passed(AttackerSoldier passed)
        {
            target = passed;
            passing = true;
        }

        void OnEnable()
        {
            if (instance == null)
            {
                instance = this;

            }

            else
            {
                Destroy(gameObject);
                return;
            }
            EventManager.OnBallHolded += Hold;
            EventManager.OnBallLoose += Loose;
        }

        void OnDisable()
        {            
            EventManager.OnBallHolded -= Hold;
            EventManager.OnBallLoose -= Loose;
        }

        private void Start()
        {
            //PlaceRandom();
        }

        private void Update()
        {
            if (passing)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, target.passingBallSpeed * Time.deltaTime);
            }
        }

        public void Hold(AttackerSoldier soldier)
        {
            transform.SetParent(soldier.transform);
            ballAnim.SetBool("Hold", true);
            passing = false;
        }

        public void PlaceRandom()
        {
            Loose();
            var attackerFieldBounds = GameManager.instance.currentAttacker.spawnArea.bounds;
            var minArea = attackerFieldBounds.min;
            var maxArea = attackerFieldBounds.max;

            var randXpos = Random.Range(minArea.x+1, maxArea.x-1);
            var randYpos = Random.Range(minArea.y+1, maxArea.y-1);

            transform.position = new Vector3(randXpos, 0, randYpos);
        }

        public void Loose()
        {
            transform.SetParent(GameManager.instance.field);
            ballAnim.SetBool("Hold", false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Gate")
            {
                EventManager.OnGoal?.Invoke();
            }   
        }
    }
}

