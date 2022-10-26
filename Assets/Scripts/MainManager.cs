using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CapedHorse.BallBattle
{
    public class MainManager : MonoBehaviour
    {
        public static MainManager instance;
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
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
        /// Will ask permision for camera first, if granted, then open AR Scene
        /// </summary>
        public void PlayGame()
        {
            var isCameraAllowed = true;
            if (isCameraAllowed)
            {
                SceneManager.LoadScene("ARScene");
            }
            else
            {
                SceneManager.LoadScene("MainScene");
            }
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}

