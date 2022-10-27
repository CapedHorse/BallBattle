using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapedHorse.BallBattle
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        public AudioSource bgmAudio, sfxAudio;

        public List<Audio> audioClips;
        public Dictionary<string, Audio> audioTable;

        [System.Serializable]
        public class Audio
        {
            public string name;
            public AudioClip clip;
        }

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
    }
}

