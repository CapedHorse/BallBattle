using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

namespace CapedHorse.BallBattle
{
    public class BarUI : MonoBehaviour
    {
        public ControllerUI controllerUI;
        public bool barFull;
        public GameObject barParent;
        public Image barLowImg;
        public Image barFullImg;


        public void InitRefill(ControllerUI _controllerUI, UnityAction onDoneRefill)
        {
            controllerUI = _controllerUI;
            barLowImg.fillAmount = 0;
            barLowImg.DOFillAmount(1, 1 / controllerUI.controller.energyRegenTime).onComplete = () =>
            {
                barParent.transform.DOPunchScale(Vector3.one * 0.25f, 0.1f, 1, 1);
                barFull = true;
                barFullImg.gameObject.SetActive(true);
                onDoneRefill?.Invoke();
            };
        }
    }
}

