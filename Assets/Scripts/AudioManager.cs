using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapedHorse.BallBattle
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        public AudioSource
            bgmAudioSource,
            sfxAudioSource;

        [InfoBox("You can change the audio clip but don't change the ID.", EInfoBoxType.Normal)]
        public List<AudioClipHelper> sfx;
        public List<AudioClipHelper> bgm;

        public Dictionary<string, AudioClip> sfxTable = new Dictionary<string, AudioClip>();
        public Dictionary<string, AudioClip> bgmTable = new Dictionary<string, AudioClip>();

        private void Awake()
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

            sfxTable = new Dictionary<string, AudioClip>();
            foreach (var item in sfx)
            {
                sfxTable.Add(item.id, item.audioClip);
            }

            bgmTable = new Dictionary<string, AudioClip>();
            foreach (var item in bgm)
            {
                bgmTable.Add(item.id, item.audioClip);
            }
        }       

        public void PlaySFX(string sfxName)
        {
            if (isOn(AudioSourceType.sfx))
            {
                if (!sfxTable.ContainsKey(sfxName) || sfxTable[sfxName] == null)
                {
                    return;
                }

                AudioClip audioClip = sfxTable[sfxName];
                sfxAudioSource.loop = false;
                sfxAudioSource.PlayOneShot(audioClip);
            }
        }

        public void PlayBGM(string bgmName)
        {
            if (isOn(AudioSourceType.bgm))
            {
                if (!bgmTable.ContainsKey(bgmName) || bgmTable[bgmName] == null)
                {
                    return;
                }
                bgmAudioSource.Stop();

                AudioClip audioClip = bgmTable[bgmName];
                bgmAudioSource.clip = audioClip;
                bgmAudioSource.Play();
            }
        }

        public enum AudioSourceType { bgm, sfx }


        public void AudioMute(AudioSourceType _type, bool _value)
        {
            PlayerPrefs.SetInt(_type.ToString() + "Mute", _value ? 0 : 1);
            Debug.Log($"MUTE {_type} Audio {_value}", gameObject);
            if (_type == AudioSourceType.bgm)
            {
                bgmAudioSource.mute = _value;
                PlayBGM("Menu");
            }

            else
            {
                sfxAudioSource.mute = _value;                
            }
        }

        public bool isOn(AudioSourceType _type)
        {
            var key = _type.ToString() + "Mute";
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetInt(key) == 1;
            }
            else
            {
                PlayerPrefs.SetInt(key, 1);
                return true;
            }
        }

        [System.Serializable]
        public struct AudioClipHelper
        {
            public string id;
            public AudioClip audioClip;
        }
    }
}

