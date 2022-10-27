using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
namespace CapedHorse.BallBattle
{
    /// <summary>
    /// To handle UI Interaction in Main Menu
    /// </summary>
    public class MainMenuManager : MonoBehaviour
    {

        public Button playARModeButton, playNormalModeButton, quitButton, yesQuitButton, noQuitButton;
        public Toggle audioToggle;
        public GameObject quitPopUp, quitBg, unsupportedARDevice;
        // Start is called before the first frame update
        void Start()
        {
            playARModeButton.onClick.AddListener(() =>
            {
                AudioManager.instance.PlaySFX("Click");
                MainManager.instance.PlayGameARMode();
            });
            playNormalModeButton.onClick.AddListener(() =>
            {
                AudioManager.instance.PlaySFX("Click");
                MainManager.instance.PlayGameNormalMode();
            });
            quitButton.onClick.AddListener(() => 
            {
                AudioManager.instance.PlaySFX("Click");
                PopUpQuit(true); 
            });
            yesQuitButton.onClick.AddListener(() =>
            {
                AudioManager.instance.PlaySFX("Click");
                MainManager.instance.QuitGame(); 
            });
            noQuitButton.onClick.AddListener(() =>
            {
                AudioManager.instance.PlaySFX("Click");
                PopUpQuit(false); 
            });

            audioToggle.onValueChanged.AddListener((on) =>
            {
                AudioManager.instance.AudioMute(AudioManager.AudioSourceType.bgm, on);
                AudioManager.instance.AudioMute(AudioManager.AudioSourceType.sfx, on);
                AudioManager.instance.PlaySFX("Click");
            });

            //disable ARModePlayButton if device not supported by ARCore or Vuforia
            //also show a text to notify
            if (!MainManager.instance.IsDeviceSupported)
            {
                playARModeButton.interactable = false;
                unsupportedARDevice.SetActive(true);
            }

            AudioManager.instance.PlayBGM("Menu");
        }

        

        /// <summary>
        /// Tweening pop up quit with scale.
        /// </summary>
        /// <param name="open"></param>
        void PopUpQuit(bool open)
        {
            if (open)
            {
                quitPopUp.SetActive(true);
                quitBg.transform.localScale = Vector3.zero;
                quitBg.transform.DOScale(Vector3.one, 0.25f);
            }
            else
            {
                quitPopUp.SetActive(true);
                quitBg.transform.DOScale(Vector3.zero, 0.25f).onComplete = () =>
                {
                    quitPopUp.SetActive(false);
                    quitBg.transform.localScale = Vector3.one;
                };
            }
        }
    }
}

