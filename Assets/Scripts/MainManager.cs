using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ANDROID
using UnityEngine.Android;
#elif UNITY_IPHONE
using UnityEngine.iOS;
#endif
using UnityEngine.SceneManagement;

namespace CapedHorse.BallBattle
{
    public class MainManager : MonoBehaviour
    {
        public static MainManager instance;
        PermissionCallbacks permissionCallbacks = new PermissionCallbacks();
        public bool grantedCamera;
        public bool paused;
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
            CheckIfCameraPermissionGranted();
            if (grantedCamera)
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

        public void PauseGame()
        {
            Time.timeScale = paused? 1 : 0;
            paused = true;
        }

        /// <summary>
        /// Checking if user has granted camera or not. Different trait for each platform.
        /// </summary>
        public void CheckIfCameraPermissionGranted()
        {
#if UNITY_ANDROID
            if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            {                
                permissionCallbacks.PermissionDenied += Callbacks_PermissionDenied;
                permissionCallbacks.PermissionGranted += PermissionCallbacks_PermissionGranted;
                permissionCallbacks.PermissionDeniedAndDontAskAgain += PermissionCallbacks_PermissionDeniedAndDontAskAgain;
                Permission.RequestUserPermission(Permission.Camera, permissionCallbacks);
            }
            else
            {
                grantedCamera = true;
            }

#elif UNITY_IPHONE

#endif
        }

        //Callbacks when asking permission.

        private void PermissionCallbacks_PermissionDeniedAndDontAskAgain(string obj)
        {
            grantedCamera = false;
            permissionCallbacks.PermissionDenied -= PermissionCallbacks_PermissionDeniedAndDontAskAgain;
        }

        private void PermissionCallbacks_PermissionGranted(string obj)
        {
            Debug.Log("Camera Granted");
            grantedCamera = true;
            permissionCallbacks.PermissionGranted -= PermissionCallbacks_PermissionGranted;
        }

        private void Callbacks_PermissionDenied(string obj)
        {            
            grantedCamera = false;
            permissionCallbacks.PermissionDenied -= Callbacks_PermissionDenied;
        }
    }
}

