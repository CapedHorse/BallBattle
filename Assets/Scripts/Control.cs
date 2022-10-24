﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapedHorse.BallBattle
{
    public class Control : MonoBehaviour
    {
        public enum ControlType { PLAYER, AI}
        public ControlType controlType;
        public GameManager.Position position;
        
        public LayerMask raycastLayer;

        [Header("Status")]
        public float energy;
        public bool isInTurn;
        
        // Start is called before the first frame update
        void Start()
        {
            energy = GameManager.instance.gameSetting.energyBar;
        }

        void OnEnable()
        {
            GameManager.SetTurn += InTurn;
        }

        void OnDisable()
        {
            GameManager.SetTurn -= InTurn;
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
                switch (controlType)
                {
                    case ControlType.PLAYER:
                        OnTouchOrMouseDown();
                        break;
                    case ControlType.AI:
                        //Have different behaviour
                        break;
                    default:
                        break;
                }
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
                    var ray = Camera.main.ScreenPointToRay(inputLoc);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastLayer.value))
                    {
                        Debug.Log("hit" + hit.transform.name);
                    }
                }
            }
//#else
            if(Input.GetMouseButtonDown(0))
            {                 
                Debug.Log("Mouse Up");
                var inputLoc = Input.mousePosition;
                var ray = Camera.main.ScreenPointToRay(inputLoc);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastLayer.value))
                {
                    Debug.Log("hit" + hit.transform.name + position.ToString());
                    if (hit.transform.CompareTag(position.ToString()+"Area"))
                    {
                        GameManager.OnSpawning?.Invoke(this, hit.point);
                    }
                }
            }
//#endif
        }

        public void CostingEnergy(float energyCost)
        {
            energy = Mathf.Clamp(energy - energyCost, 0, energy);
            UIManager.OnCostingEnergy(this, energyCost);
        }
    }
}

