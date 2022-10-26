using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace CapedHorse.BallBattle
{
    public class ControllerUI : MonoBehaviour
    {
        public RectTransform thisRect;
        public GameManager.Position controllerPosition;
        public Control controller;
        public GameObject barObj;

        public Transform scoreParent;
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

            thisRect.anchoredPosition = controllerPosition == GameManager.Position.Attacker ? UIManager.instance.attackerUIStart : UIManager.instance.defenderUIStart;
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

        /// <summary>
        /// Rearrange UI layout based on controller's position.
        /// </summary>
        public void SwitchSides()
        {
            controllerPosition = controller.position;
            var sibblingId = 0;
            var energyBarRotate = 0f;
            var tweenStartPos = new Vector2();
            var tweenToPos = new Vector2();
            switch (controllerPosition)
            {
                case GameManager.Position.Attacker:
                    tweenStartPos = UIManager.instance.attackerUIStart;
                    tweenToPos = UIManager.instance.attackerUITo;
                    thisRect.SetParent(UIManager.instance.attackerUIParent);
                    break;
                case GameManager.Position.Defender:
                    sibblingId = 1;
                    energyBarRotate = 180f;
                    tweenStartPos = UIManager.instance.defenderUIStart;
                    tweenToPos = UIManager.instance.defenderUITo;                    
                    thisRect.SetParent(UIManager.instance.defenderUIParent);
                    break;
                default:
                    break;
            }

            scoreParent.SetSiblingIndex(sibblingId);
            barsParent.SetSiblingIndex(sibblingId);
            barsParent.localEulerAngles = new Vector3(0, 0, energyBarRotate);
            TweenToUI(tweenStartPos, tweenToPos);
        }

        public void TweenToUI(Vector2 from, Vector2 to )
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(thisRect.DOAnchorPos(from, 0));
            seq.Append(thisRect.DOAnchorPos(to, 0.25f));
        }

        public void SetPenaltyMode()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}

