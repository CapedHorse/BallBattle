using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

namespace CapedHorse.BallBattle
{
    public class ARSceneManager : MonoBehaviour
    {
        public static ARSceneManager instance;
        public Camera arCam;
        public GameObject arSceneCanvas, arSceneEventSystem, arNotSupportedNotif;
        public Button detectGroundPlaneButton, exitToMenu;
        public ContentPositioningBehaviour planeFinder;
        public Transform groundPlane;

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
            exitToMenu.onClick.AddListener(MainManager.instance.BackToHomeScene);

            //handle if device is supported or not, if yes, load game to AR Scene, if not, show notification instead
            if (!MainManager.instance.IsDeviceSupported)
            {
                arNotSupportedNotif.SetActive(true);
            } else
            {
                planeFinder.OnContentPlaced.AddListener((gameObject) =>
                {
                    arSceneCanvas.SetActive(false);
                    arSceneEventSystem.SetActive(false);
                    planeFinder.gameObject.SetActive(false);
                    MainManager.instance.LoadGameToAR(() => StartCoroutine(StartMovingFieldToGroundPlane()));
                });
            }
        }

        /// <summary>
        /// Placing field to AR Ground plane need to be using coroutine so we could have a delay for MainScene's script to be initialized
        /// </summary>
        /// <returns></returns>
        IEnumerator StartMovingFieldToGroundPlane()
        {
            yield return new WaitForSeconds(0.5f);
            GameManager.instance.field.SetParent(groundPlane);
            GameManager.instance.field.localPosition = Vector3.zero;
            GameManager.instance.field.localScale = Vector3.one;
            GameManager.instance.field.localEulerAngles = Vector3.zero;
        }

        
    }
}

