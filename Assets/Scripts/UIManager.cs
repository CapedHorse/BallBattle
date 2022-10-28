using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;

namespace CapedHorse.BallBattle
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;

        public Canvas mainCanvas;

        public Button pauseButton, continueButton, quitButton, yesQuitBtn, noQuitBtn;
        public Toggle ARmodeToggle;

        public GameObject pauseGame, quitPopUp;
        public Transform attackerUIParent;
        public Transform defenderUIParent;
        public Transform countDownTweenParent;
        public RectTransform timerParent;
        public RectTransform menuParent;

        //controller UI tweening position properties
        public Vector2
            defenderUIStart,
            defenderUITo,
            attackerUIStart,
            attackerUITo;


        public GameObject countDownParent;
        public TextMeshProUGUI countDownText;
        public TextMeshProUGUI timerText;

        public List<ControllerUI> controllerUIs;
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            foreach (var item in controllerUIs)
            {
                item.Init();
            }

            if (!MainManager.instance.IsDeviceSupported)
            {
                ARmodeToggle.gameObject.SetActive(false);
            }

            pauseButton.onClick.AddListener(() =>
            {
                AudioManager.instance.PlaySFX("Click");
                pauseGame.SetActive(true);
                MainManager.instance.PauseGame();
            });
            continueButton.onClick.AddListener(() =>
            {
                AudioManager.instance.PlaySFX("Click");
                pauseGame.SetActive(false);
                MainManager.instance.PauseGame();
            });
            quitButton.onClick.AddListener(() =>
            {
                AudioManager.instance.PlaySFX("Click");
                quitPopUp.SetActive(true);
            });

            yesQuitBtn.onClick.AddListener(() =>
            {
                AudioManager.instance.PlaySFX("Click");
                MainManager.instance.BackToHomeScene();                
            });

            noQuitBtn.onClick.AddListener(() =>
            {
                AudioManager.instance.PlaySFX("Click");
                quitPopUp.SetActive(false);                
            });

        }

        private void OnEnable()
        {
            EventManager.SetPosition += SwitchControllerUIPosition;
        }

        private void OnDisable()
        {
            EventManager.SetPosition -= SwitchControllerUIPosition;
        }

        // Update is called once per frame
        void Update()
        {

        }

       

        /// <summary>
        /// To Update Timer in the UI.
        /// <param name="currentTime"> current updated time in the UI </param>
        /// </summary>
        public void UpdateTimerUI(float currentTime)
        {
            System.TimeSpan time = System.TimeSpan.FromSeconds(currentTime);
            timerText.text = time.ToString("mm':'ss");
        }

        /// <summary>
        /// Coroutine to counting down before match started. Will automatically invoke a callback after that.
        /// </summary>
        /// <param name="afterCountDown"></param>
        /// <returns></returns>
        public IEnumerator CountingDown(UnityAction afterCountDown)
        {
            var initTimerPos = timerParent.anchoredPosition;
            timerParent.anchoredPosition = new Vector2(initTimerPos.x* -1, initTimerPos.y);

            var initMenuPos = menuParent.anchoredPosition;
            menuParent.anchoredPosition = new Vector2(initMenuPos.x * -2, initMenuPos.y);
            countDownParent.SetActive(true);
            TweenCountDown("3");
            yield return new WaitForSeconds(1);
            TweenCountDown("2");
            yield return new WaitForSeconds(1);
            TweenCountDown("1");
            yield return new WaitForSeconds(1);
            TweenCountDown("GO!");
            yield return new WaitForSeconds(1);
            countDownParent.SetActive(false);
            foreach (var item in controllerUIs)
            {
                item.SwitchSides();
                item.RefillEnergy();
            }

            timerParent.DOAnchorPos(initTimerPos, 0.25f);
            menuParent.DOAnchorPos(initMenuPos, 0.25f);

            AudioManager.instance.PlaySFX("PopUp");
            afterCountDown?.Invoke();
        }

        public void TweenCountDown(string text)
        {
            countDownText.text = text;
            countDownTweenParent.DOPunchScale(Vector3.one * 0.5f, 0.25f, 1, 1);
            AudioManager.instance.PlaySFX(text == "GO!" ? "Whistle": "CountDown"); //on Go text, play the whistle
        }

        public void SwitchControllerUIPosition()
        {
            DOVirtual.DelayedCall(0.25f, () =>
            {
                foreach (var item in controllerUIs)
                {
                    item.SwitchSides();
                }
            });            
        }
               

        
    }
}

