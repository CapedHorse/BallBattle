using DG.Tweening;
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
            initialPosition = transform.position; //initiate spawning position
            
            //initiate the detection radius, percentage of field width. Find the width using the difference between maximum and minimum of the field bounds.
            var fieldBounds = GameManager.instance.fieldCollider.bounds;
            var fieldWidth = new Vector2(fieldBounds.min.x, fieldBounds.max.x);
            var wide = (fieldWidth.y - fieldWidth.x) * detectionRangeFromWidth;
            detectionRadius.transform.localScale = new Vector3(wide, 0, wide);

            StartCoroutine(BaseStart(() =>  {
                OnChangingState(State.StandBy);                 
                }));
        }

        void OnEnable()
        {
            EventManager.OnAttackerCaught += OnAttackerCaught;
        }

        void OnDisable()
        {
            EventManager.OnAttackerCaught -= OnAttackerCaught;
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
                    collider.enabled = false;
                    if (!justSpawned)
                    {                        
                        DOVirtual.DelayedCall(reactivateTime, () =>
                        {
                            
                            OnChangingState(State.ReturnBack);
                        });
                    }
                    
                    break;
                case State.StandBy:
                    SetAnimation("StandBy");
                    collider.enabled = true;
                    break;
                    
                case State.Chasing:
                    SetAnimation("Chasing");
                    collider.enabled = true;
                    break;
                case State.ReturnBack:
                    SetAnimation("ReturnBack");
                    collider.enabled = true;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// To handle if multiple defender targeting the same attacker.
        /// </summary>
        /// <param name="defender"></param>
        /// <param name="attacker"></param>
        public void OnAttackerCaught(DefenderSoldier defender, AttackerSoldier attacker)
        {
            if (status != State.Chasing)
                return;
            if (defender == this)
                return;
            if (attacker != target.GetComponent<AttackerSoldier>())
                return;
            OnChangingState(State.ReturnBack);
        }

        public void onChild_OnTriggerEnter(Collider myEnteredTrigger, Collider other)
        {
            Debug.Log("Entered detection radius");
            if (other.CompareTag("Attacker"))
            {
                if(!haveTarget) {
                    if (other.GetComponent<AttackerSoldier>().status == State.HoldingBall)
                    {
                        target = other.transform;
                        OnChangingState(State.Chasing);
                        haveTarget = true;
                    }
                    
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
                if (other.GetComponent<AttackerSoldier>().status == State.HoldingBall)
                {
                    OnChangingState(State.Inactive);
                    target = null;
                    haveTarget = false;
                    EventManager.OnAttackerCaught?.Invoke(this, other.GetComponent<AttackerSoldier>());
                    EventManager.OnBallLoose?.Invoke();
                }
                
            }
        }
    }
}

