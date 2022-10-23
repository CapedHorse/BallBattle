using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CapedHorse.BallBattle
{
    public class ControllerUI : MonoBehaviour
    {
        public Control controller;
        public GameObject barObj;
        public Transform barsParent;
        public TextMeshProUGUI energyControllerNameText;
        public GameObject notTurnOverlay;

        [System.Serializable]
        public class BarUI
        {
            public GameObject barParent;
            public Image barLowImg;
        }
        public List<BarUI> bars;

        // Start is called before the first frame update
        void Start()
        {
            Init();
        }

        void OnEnable()
        {
            GameManager.SetTurn += InTurn;
        }

        void OnDisable()
        {
            GameManager.SetTurn -= InTurn;
        }
        void Init()
        {
            for (int i = 0; i < GameManager.instance.gameSetting.energyBar; i++)
            {
                var newBar = new BarUI();
                newBar.barParent = Instantiate(barObj, barsParent);
                newBar.barLowImg = newBar.barParent.transform.GetChild(1).GetComponent<Image>();
                newBar.barLowImg.gameObject.SetActive(false);
            }
            barObj.SetActive(false);
        }

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

        public void EnergyCostUI(Control controller, float cost)
        {

        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}

