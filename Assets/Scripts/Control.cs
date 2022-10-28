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
        public Goal thisGoal, oppositeGoal;
        public float energyRegenTime = 0.5f;

        public enum Status { Loose, HoldingBall }
        public Status status;

        public LayerMask raycastLayer;

        public List<Soldier> soldiers;
        [Header("Status")]
        public float energy;
        public bool isInTurn;
        
        // Start is called before the first frame update
        void Start()
        {
            //energy = GameManager.instance.gameSetting.energyBar;
        }

        void OnEnable()
        {
            EventManager.SetTurn += InTurn;
            EventManager.OnNearestToBall += CheckIfNearBall;
        }

        void OnDisable()
        {
            EventManager.SetTurn -= InTurn;
            EventManager.OnNearestToBall -= CheckIfNearBall;
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

        public void PassingBall(AttackerSoldier soldier)
        {
            if (Utility.NearestToTarget(soldiers, GameManager.instance.ball.transform) == null)
            {
                EventManager.OnNoBallPasses?.Invoke();
            }
        }
    }
}

