using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapedHorse.BallBattle
{
    public class DefenderSoldier : Soldier, IOnTriggerNotifiable
    {
               

        //variables
        public float returnSpeed;
        public float detectionRangeFromWidth;
        public Vector3 initialPosition;
        public bool haveTarget;

        [Header("Defender Object Refs")]
        public GameObject detectionRadius;
        public Collider detectionRadiusCollider;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(BaseStart(() =>  {
                OnChangingState(State.StandBy); 
                initialPosition = transform.position;
                }));
        }

        // Update is called once per frame
        void Update()
        {
            if (!GameManager.instance.isPlaying)
                return;

            switch (status)
            {
                case State.Inactive:
                    break;
                case State.StandBy:
                    break;
                case State.ReturnBack:
                    if(transform.position != initialPosition) {
                        MoveTo(initialPosition, normalSpeed);
                    } else
                    {
                        OnChangingState(State.StandBy);
                    }
                    break;
                case State.Chasing:
                    MoveTo(target.position, normalSpeed);
                    break;
                default:
                    break;
            }
        }

        public override void OnChangingState(State state)
        {
            base.OnChangingState(state);

            switch (state)
            {
                case State.Inactive:
                    ResetAnimation();
                    break;
                case State.StandBy:
                    SetAnimation("StandBy");
                    break;
                case State.Chasing:
                    SetAnimation("Chasing");
                    break;
                case State.ReturnBack:
                    SetAnimation("Chasing");

                    break;
                default:
                    break;
            }
        }



        public void onChild_OnTriggerEnter(Collider myEnteredTrigger, Collider other)
        {
            Debug.Log("Entered detection radius");
            if (other.CompareTag("Attacker"))
            {
                if(!haveTarget) {
                    target = other.transform;
                    OnChangingState(State.Chasing);
                    haveTarget = true;
                }
                
            }
        }

        public void onChild_OnTriggerExit(Collider myEnteredTrigger, Collider other)
        {
            Debug.Log("Exited detection radius");
        }

        public void onChild_OnTriggerStay(Collider myEnteredTrigger, Collider other)
        {
            //Debug.Log("Stayed detection radius");
        }

        void OnTriggerEnter(Collider other) {
            if(other.CompareTag("Attacker")) {
                OnChangingState(State.ReturnBack);
                target = null;
                haveTarget = false;
            }
        }
    }
}

