using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

namespace CapedHorse.BallBattle
{
    public class AttackerSoldier : Soldier, IOnTriggerNotifiable
    {        
        //variables
        public float carryingSpeed;
        public float passingBallSpeed;
        bool gettingPassed;

        [Header("Attacker Object Refs")]
        
        public Transform ballTarget;

        public UnityAction<AttackerSoldier> OnGettingPassed;
        
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(BaseStart(() =>
            {
                OnChangingState(State.StraightThrough);
                EventManager.OnNearestToBall?.Invoke(this);
            }));
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

        //moving logic is false
        void Update()
        {
            if (!GameManager.instance.isPlaying)
                return;
            switch (status)
            {
                case State.Inactive:
                    break;
                case State.PassedBall:
                    break;
                case State.HoldingBall:
                    //rb.MovePosition(target.position * 0.001f * normalSpeed * Time.deltaTime);
                    MoveTo(target.position, carryingSpeed);
                    break;
                case State.StraightThrough:
                    MoveTo(new Vector3(transform.position.x, target.position.y, target.position.z), normalSpeed);
                    //rb.MovePosition(new Vector3(transform.position.x, target.position.y, target.position.z) * 0.001f * normalSpeed * Time.deltaTime);
                    break;
                case State.ChasingBall:
                    //rb.MovePosition(target.position * 0.001f * normalSpeed * Time.deltaTime);
                    MoveTo(target.position, carryingSpeed);
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
                case State.HoldingBall:
                    SetAnimation("HoldingBall");
                    target = GameManager.instance.attackerTargetGate;
                    break;
                case State.ChasingBall:
                    SetAnimation("ChasingBall");
                    target = GameManager.instance.ball.transform;
                    break;
                case State.StraightThrough:
                    SetAnimation("StraightThrough");
                    target = GameManager.instance.attackerTargetGate;
                    break;
                case State.PassedBall:
                    SetAnimation("PassedBall");
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

        public void ChaseBall()
        {
            OnChangingState(State.ChasingBall);

            
        }



        public void onChild_OnTriggerEnter(Collider myEnteredTrigger, Collider other)
        {
            

        }

        public void onChild_OnTriggerStay(Collider myEnteredTrigger, Collider other)
        {
            //throw new System.NotImplementedException();
        }

        public void onChild_OnTriggerExit(Collider myEnteredTrigger, Collider other)
        {
            //throw new System.NotImplementedException();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                Debug.Log("Holding Ball Now");
                OnChangingState(State.HoldingBall);
                other.GetComponent<Ball>().Hold(this);
            }

            if (other.CompareTag("Fence"))
            {
                Debug.Log("Entering Fence");
                OnChangingState(State.Inactive);
                exploded.SetActive(true);
                controller.soldiers.Remove(this);
                Destroy(gameObject, 1);
            }
        }

        

        
    }
}

