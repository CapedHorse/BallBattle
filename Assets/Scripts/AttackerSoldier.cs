using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapedHorse.BallBattle
{
    public class AttackerSoldier : Soldier
    {        
        //variables
        public float carryingSpeed;
        public float passingBallSpeed;

        
        // Start is called before the first frame update
        void Start()
        {
            BaseStart();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void OnChangingState(State state)
        {
            base.OnChangingState(state);
            
            switch (state)
            {
                case State.Inactive:
                    break;
                case State.HoldingBall:
                    break;
                case State.StraightThrough:
                    break;
                default:
                    break;
            }
        }
    }
}

