using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

namespace CapedHorse.BallBattle
{
    public class AttackerSoldier : Soldier
    {        
        //variables
        public float carryingSpeed;
        public float passingBallSpeed;
        bool gettingPassed;
        public bool hasCaught;

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
            EventManager.OnBallHolded += OnBallHolded;
        }

        void OnDisable()
        {
            OnGettingPassed -= GettingPassed;
            EventManager.OnBallHolded -= OnBallHolded;
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
                    MoveTo(target.position, normalSpeed);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Will invoke different calls & behaviour when changing state.
        /// </summary>
        /// <param name="state"></param>
        public override void OnChangingState(State state)
        {
            base.OnChangingState(state);
            
            switch (state)
            {
                case State.Inactive:                    
                    ResetAnimation();
                    //collider.enabled = false;
                    //if (!justSpawned)
                    //{ 
                    
                    //}
                        break;
                case State.HoldingBall:
                    collider.enabled = true;
                    SetAnimation("HoldingBall");
                    target = GameManager.instance.attackerTargetGate;
                    break;
                case State.ChasingBall:
                    collider.enabled = true;
                    SetAnimation("ChasingBall");
                    target = GameManager.instance.ball.transform;
                    break;
                case State.StraightThrough:
                    SetAnimation("StraightThrough");
                    target = GameManager.instance.attackerTargetGate;
                    break;
                case State.PassedBall:
                    SetAnimation("PassedBall");
                    RotateDirection(Ball.instance.transform.position);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// When passed a ball
        /// </summary>
        /// <param name="passer"></param>
        public void GettingPassed(AttackerSoldier passer)
        {
            target = passer.transform;
            OnChangingState(State.PassedBall);
            Ball.instance.Passed(this);
        }

        public void ChaseBall()
        {
            OnChangingState(State.ChasingBall);
        }

        public void OnBallHolded(AttackerSoldier soldier)
        {
            if (soldier != this)
            {
                OnChangingState(State.StraightThrough);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                Debug.Log("Holding Ball Now");
                OnChangingState(State.HoldingBall);
                other.GetComponent<Ball>().Hold(this);
                EventManager.OnBallHolded(this);
            }

            if (other.CompareTag("Fence"))
            {
                Debug.Log("Entering Fence");
                OnChangingState(State.Inactive);
                exploded.SetActive(true);
                controller.soldiers.Remove(this);
                GameManager.instance.spawnedSoldiers.Remove(this);
                Destroy(gameObject, 1);
            }

            if (other.CompareTag("Defender"))
            {
                if (other.GetComponent<DefenderSoldier>().status != State.Inactive)
                {
                    Debug.Log("Collide With Defender");
                    OnChangingState(State.Inactive);
                    EventManager.OnNearestToPass?.Invoke(this);
                    DOVirtual.DelayedCall(reactivateTime, () =>
                    {
                        collider.enabled = true;
                        OnChangingState(State.StraightThrough);
                    });
                }
                
            }
        }

        

        
    }
}

