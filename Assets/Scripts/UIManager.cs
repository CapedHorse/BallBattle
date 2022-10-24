using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

namespace CapedHorse.BallBattle
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;

        public Canvas mainCanvas;

        public Transform attackerEnergyParent;
        public Transform defenderEnergyParent;

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
            countDownParent.SetActive(true);
            countDownText.text = "3";
            yield return new WaitForSeconds(1);
            countDownText.text = "2";
            yield return new WaitForSeconds(1);
            countDownText.text = "1";
            yield return new WaitForSeconds(1);
            countDownText.text = "GO!";
            yield return new WaitForSeconds(1);
            countDownParent.SetActive(false);
            foreach (var item in controllerUIs)
            {
                item.Init();
            }
            afterCountDown?.Invoke();
        }
    }
}

