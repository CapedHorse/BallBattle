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
        public BarUI barPrefab;

        public Transform scoreParent;
        public Transform barsParent;


        public TextMeshProUGUI energyControllerNameText;
        public GameObject notTurnOverlay;
        public List<BarUI> bars;
        public List<BarUI> costedBars;
        //public List<UIBar> emptyBars;

        //[System.Serializable]
        //public class UIBar
        //{
        //    public bool barFull;
        //    public GameObject barParent;
        //    public Image barLowImg;
        //    public Image barFullImg;
        //}


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
            //for (int i = 0; i < GameManager.instance.gameSetting.energyBar; i++)
            //{
            //    var newBar = new UIBar();
            //    newBar.barParent = Instantiate(barObj, barsParent);
            //    newBar.barFullImg = newBar.barParent.transform.GetChild(0).GetComponent<Image>();
            //    newBar.barLowImg = newBar.barParent.transform.GetChild(1).GetComponent<Image>();
            //    newBar.barLowImg.gameObject.SetActive(false);
            //    newBar.barFull = true;
            //    bars.Add(newBar);
            //}
            //barObj.SetActive(false);

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


        /// <summary>
        /// Costing Energy bar will destroy BarUI based on how much the cost is, this is to make the UI auto arrange themself. 
        /// Differentiate the current costed energy bar with default energy bar so it's easier to destroy.
        /// After costing, invoke refill.
        /// </summary>
        /// <param name="_controller"></param>
        /// <param name="cost"></param>
        public void EnergyCostUI(Control _controller,  float cost)
        {
            if (controller != _controller)
                return;

            costedBars.Clear();

            for (int i = 0; i < cost; i++)
            {
                if (bars[i].barFull)
                {
                    costedBars.Add(bars[i]);
                }                
            }

            for (int i = costedBars.Count -1; i >= 0; i--)
            {
                bars.Remove(costedBars[i]);
                Destroy(costedBars[i].gameObject);
            }

            costedBars.Clear();

            RefillEnergy();
        }
        
        /// <summary>
        /// Refill energy will instantiate a new BarUI
        /// </summary>
        bool currentlyRefilling;

        public void RefillEnergy() 
        {
            if (currentlyRefilling)
                return;
            currentlyRefilling = true;
            var newEnergy = Instantiate(barPrefab, barsParent);
            newEnergy.InitRefill(this, () =>
            {
                controller.energy = Mathf.Clamp(controller.energy+1, 0, GameManager.instance.gameSetting.energyBar);
                currentlyRefilling = false;
                if (controller.energy < GameManager.instance.gameSetting.energyBar)
                {
                    RefillEnergy();
                }
            });
            bars.Add(newEnergy);
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

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}

