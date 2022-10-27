using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CapedHorse.BallBattle
{
    public class ARSceneManager : MonoBehaviour
    {
        public static ARSceneManager instance;
        public Camera arCam;
        public GameObject arSceneCanvas;
        public Button detectGroundPlaneButton;

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
    }
}

