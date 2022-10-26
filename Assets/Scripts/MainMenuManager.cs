using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
namespace CapedHorse.BallBattle
{
    public class MainMenuManager : MonoBehaviour
    {

        public Button playButton, quitButton, yesQuitButton, noQuitButton;
        public Toggle audioToggle;
        public GameObject quitPopUp, quitBg;
        // Start is called before the first frame update
        void Start()
        {
            playButton.onClick.AddListener(() => MainManager.instance.PlayGame() );
            quitButton.onClick.AddListener(() => PopUpQuit(true));
            yesQuitButton.onClick.AddListener(() => MainManager.instance.QuitGame());
            noQuitButton.onClick.AddListener(() => PopUpQuit(false));
        }

        // Update is called once per frame
        void Update()
        {

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

