using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CapedHorse.BallBattle
{
    public class AttackerSoldier : Soldier
    {        
        //variables
        public float carryingSpeed;
        public float passingBallSpeed;
        bool gettingPassed;

        [Header("Attacker Object Refs")]
        public Transform target;
        public Transform ballTarget;

        public UnityAction<AttackerSoldier> OnGettingPassed;
        
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(BaseStart(() =>
            {
                OnChangingState(State.StraightThrough);
                EventManager.OnNearestToBall?.Invoke();
            }
            ));
        }

        void OnEnable()
        {
            OnGettingPassed += GettingPassed;
        }

        void OnDisable()
        {
            OnGettingPassed -= GettingPassed;
        }

        // Update is called once per frame
        void Update()
        {
            if (status == State.PassedBall)
            {

            }
        }

        public override void OnChangingState(State state)
        {
            base.OnChangingState(state);
            
            switch (state)
            {
                case State.Inactive:
                    anim.SetBool("Inactive", true);
                    break;
                case State.HoldingBall:
                    break;
                case State.StraightThrough:
                    break;
                case State.PassedBall:
                    anim.SetBool("Passed", true);
                    break;
                default:
                    break;
            }
        }

        public void GettingPassed(AttackerSoldier passer)
        {
            target = passer.transform;
            OnChangingState(State.PassedBall);

        }
    }
}

