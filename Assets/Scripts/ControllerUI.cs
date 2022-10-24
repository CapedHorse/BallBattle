using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CapedHorse.BallBattle
{
    public class ControllerUI : MonoBehaviour
    {
        public GameManager.Position controllerPosition;
        public Control controller;
        public GameObject barObj;
        public Transform barsParent;
        public TextMeshProUGUI energyControllerNameText;
        public GameObject notTurnOverlay;

        [System.Serializable]
        public class BarUI
        {
            public bool barFull;
            public GameObject barParent;
            public Image barLowImg;
            public Image barFullImg;
        }
        public List<BarUI> bars;
        public List<BarUI> emptyBars;

        // Start is called before the first frame update
        void Start()
        {
            //Init();
        }

        void OnEnable()
        {
            EventManager.SetTurn += InTurn;
            EventManager.OnCostingEnergy += EnergyCostUI;
        }

        void OnDisable()
        {
            EventManager.SetTurn -= InTurn;
            EventManager.OnCostingEnergy -= EnergyCostUI;
        }

        

        /// <summary>
        /// Initiate the energy bar and its controller.
        /// </summary>
        public void Init()
        {
            for (int i = 0; i < GameManager.instance.gameSetting.energyBar; i++)
            {
                var newBar = new BarUI();
                newBar.barParent = Instantiate(barObj, barsParent);
                newBar.barFullImg = newBar.barParent.transform.GetChild(0).GetComponent<Image>();
                newBar.barLowImg = newBar.barParent.transform.GetChild(1).GetComponent<Image>();
                newBar.barLowImg.gameObject.SetActive(false);
                newBar.barFull = true;
                bars.Add(newBar);
            }
            barObj.SetActive(false);

            controller = GameManager.instance.controllers.Find(x => x.position == controllerPosition);
        }

        /// <summary>
        /// Setting turn of the UI, will grayscale if inactivated
        /// </summary>
        /// <param name="control"></param>
        void InTurn(Control control)
        {
            if (control == controller)
            {
                notTurnOverlay.SetActive(false);
            }
            else
            {
                notTurnOverlay.SetActive(true);
            }
        }

        [SerializeField] int lastFilledBar; //log last filled bar index

        public void EnergyCostUI(Control _controller,  float cost)
        {
            if (controller != _controller)
                return;

            for (int i = 0; i < bars.Count; i++)
            {
                if (i!= 0 && !bars[i].barFull)
                {
                    lastFilledBar = i;
                }
            }

            var startingBarCost = bars.Count -  bars.FindAll(x => x.barFull).Count;

            var realCost = Mathf.Clamp(startingBarCost + cost, 0, bars.Count);
            Debug.Log("Real cost " + realCost);
            for (int i = startingBarCost; i < realCost ; i++)
            {
                bars[i].barFullImg.gameObject.SetActive(false);
                bars[i].barLowImg.gameObject.SetActive(true);
                bars[i].barLowImg.fillAmount = 0;
                bars[i].barFull = false;
                emptyBars.Add(bars[i]);
            }


        }

        /// <summary>
        /// Need this to make newly used bar always on the front of the filled/currently refilling ones
        /// </summary>
        public void RearrangeEnergyBar()
        {
            
        }

        public void RefillEnergy(Control _controller) 
        {
            if (controller != _controller)
                return;

            if (emptyBars.Count > 0)
            {
                
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}

