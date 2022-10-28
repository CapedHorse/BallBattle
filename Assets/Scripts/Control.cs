using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapedHorse.BallBattle
{
    public class Control : MonoBehaviour
    {
        public enum ControlType { Player, Enemy}
        public ControlType controlType;
        public GameManager.Position position;
        public Collider spawnArea;
        public ControllerUI thisControllerUI;
        public Goal thisGoal, oppositeGoal;
        public float energyRegenTime = 0.5f;

        public enum Status { Loose, HoldingBall }
        public Status status;

        public LayerMask raycastLayer;

        public List<Soldier> soldiers;
        [Header("Status")]
        public float energy;
        public bool isInTurn;
        public int score;
        
        // Start is called before the first frame update
        void Start()
        {
            //energy = GameManager.instance.gameSetting.energyBar;
        }

        void OnEnable()
        {
            EventManager.SetTurn += InTurn;
            EventManager.OnNearestToBall += CheckIfNearBall;
            EventManager.OnNearestToPass += NearestToPass;
        }

        void OnDisable()
        {
            EventManager.SetTurn -= InTurn;
            EventManager.OnNearestToBall -= CheckIfNearBall;
            EventManager.OnNearestToPass -= NearestToPass;
        }

        void InTurn(Control control)
        {
            if (control == this)
            {
                isInTurn = true;
            }
            else
            {
                isInTurn = false;
            }
        }


        // Update is called once per frame
        void Update()
        {
            if (isInTurn)
            {
                OnTouchOrMouseDown();
            }
            
        }


        void OnTouchOrMouseDown()
        {
//#if UNITY_ANDROID || UNITY_IPHONE
            if (Input.touchCount> 0)
            {
                var input = Input.GetTouch(0);
                if (input.phase == TouchPhase.Ended)
                {
                    Debug.Log("Touch Up");
                    var inputLoc = input.position;
                    var ray = GameManager.instance.MainCamera.ScreenPointToRay(inputLoc);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastLayer.value))
                    {
                        
                        Debug.Log("hit" + hit.transform.name + position.ToString());
                        if (hit.transform.CompareTag(position.ToString() + "Area"))
                        {
                            
                            EventManager.OnSpawning?.Invoke(this, hit.point);
                        }
                    }
                }
            }
//#else
            if(Input.GetMouseButtonDown(0))
            {                 
                Debug.Log("Mouse Up");
                var inputLoc = Input.mousePosition;
                var ray = GameManager.instance.MainCamera.ScreenPointToRay(inputLoc);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastLayer.value))
                {
                    Debug.Log("hit" + hit.transform.name + position.ToString());
                    if (hit.transform.CompareTag(position.ToString()+"Area"))
                    {
                        EventManager.OnSpawning?.Invoke(this, hit.point);
                    }
                }
            }
//#endif
        }

        public void CostingEnergy(float energyCost)
        {
            energy = Mathf.Clamp(energy - energyCost, 0, energy);
            EventManager.OnCostingEnergy(this, energyCost);
        }


        /// <summary>
        /// When attacker solder spawned, check if there is nearby ball, chase them
        /// </summary>
        /// <param name="soldier"></param>
        public void CheckIfNearBall(AttackerSoldier soldier)
        {
            if (status == Status.HoldingBall)
                return;

            if (position != GameManager.Position.Attacker)            
                return;

            Debug.Log("If near by ball");
            if (soldier == Utility.NearestToTarget(soldiers, GameManager.instance.ball.transform))
            {
                soldier.ChaseBall();
            }            
        }

        /// <summary>
        /// Checking if there is still soldier that can be passed the ball
        /// </summary>
        /// <param name="soldier"></param>
        public void NearestToPass(AttackerSoldier soldier)
        {
            if (soldier.controller != this)
                return;

            var availableAttacker = soldiers.FindAll(x => !(x as AttackerSoldier).hasCaught);

            if (availableAttacker.Count > 0)
            {
                var nearest = Utility.NearestToTarget(availableAttacker, GameManager.instance.ball.transform);
                if (nearest != null)
                {
                    (nearest as AttackerSoldier).OnGettingPassed?.Invoke(soldier);
                }
                else
                {
                    EventManager.OnNoBallPasses?.Invoke();
                }
            }
            else
            {
                EventManager.OnNoBallPasses?.Invoke();
            }
            
        }

       

        public void GainScore()
        {
            score++;
            thisControllerUI.GainScoreUI();
        }
    }
}

