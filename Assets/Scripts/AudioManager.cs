using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapedHorse.BallBattle
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;
        public AudioMixer audioMixer;

        public AudioSource
            bgmAudioSource,
            sfxAudioSource;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        [InfoBox("You can change the audio clip but don't change the ID.", EInfoBoxType.Normal)]
        public List<AudioClipHelper> sfx;
        public List<AudioClipHelper> bgm;

        public Dictionary<string, AudioClip> sfxTable = new Dictionary<string, AudioClip>();
        public Dictionary<string, AudioClip> bgmTable = new Dictionary<string, AudioClip>();

        private void Start()
        {
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

            /*
            for (int i = 0; i < sfxList.Count; i++)
            {
                sfxTable.Add(sfxList[i].name, sfxList[i]);
            }

            for (int i = 0; i < bgmList.Count; i++)
            {
                bgmTable.Add(bgmList[i].name, bgmList[i]);
            }
            */
            Corgi.corgiTalks += LowerBGM;
            Corgi.corgiStopTalks += NormalBGM;
        }

        private void OnDisable()
        {
            Corgi.corgiTalks -= LowerBGM;
            Corgi.corgiStopTalks -= NormalBGM;
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
                bgmAudioSource.loop = false;
                bgmAudioSource.clip = audioClip;
                bgmAudioSource.Play();
            }
        }


        public void PlayBGM(string bgmName, bool loop)
        {
            if (isOn(AudioSourceType.bgm))
            {
                if (!bgmTable.ContainsKey(bgmName) || bgmTable[bgmName] == null)
                {
                    return;
                }
                bgmAudioSource.Stop();

                AudioClip audioClip = bgmTable[bgmName];
                bgmAudioSource.loop = loop;
                bgmAudioSource.clip = audioClip;
                bgmAudioSource.Play();
            }
        }

        //there might be another audio effect for when the heat get increased

        //mungkin butuh juga sfx kalau dapet coin / habis purchase

        public void PauseBGM(bool pause)
        {
            if (pause)
            {
                bgmAudioSource.Pause();
            }
            else
            {
                bgmAudioSource.UnPause();
            }
        }
        float baseBgmVolume = 100, baseSfxVolume = 100;


        public void SetBaseBgmVolume(float value) //set in menu
        {
            baseBgmVolume = value;
            SetVolume(AudioSourceType.bgm, baseBgmVolume);
        }

        public void SetBaseSfxVolume(float value) //set in settings
        {
            baseSfxVolume = value;
            SetVolume(AudioSourceType.sfx, baseSfxVolume);
        }

        public void LowerBGM()
        {
            SetVolume(AudioSourceType.bgm, 60);
            Debug.Log("Lower Volume");
        }

        public void NormalBGM()
        {
            SetVolume(AudioSourceType.bgm, baseBgmVolume);
            Debug.Log("Normal Volume");
        }

        public enum AudioSourceType { bgm, sfx, corgi }

        public void SetVolume(AudioSourceType _type, float _value)
        {
            if (isOn(_type))
            {
                float volume = -80 + ((_value / 100) * 80); //since volume db is from -80 to 0, then -80 substracted by percentage of given value from settings
                audioMixer.DOSetFloat(_type.ToString(), volume, 1f);
            }

        }

        public void AudioMute(AudioSourceType _type, bool _value)
        {
            PlayerPrefs.SetInt(_type.ToString() + "Mute", _value ? 0 : 1);
            Debug.Log($"MUTE {_type} Audio {_value}", gameObject);
            if (_type == AudioSourceType.bgm)
            {
                bgmAudioSource.mute = _value;
                PlayBGM("BGM-Intro");
            }

            else
            {
                sfxAudioSource.mute = _value;
                PlaySFX("SFX-Click");
            }

            //float baseVolume = _type == AudioSourceType.sfx ? baseSfxVolume : baseBgmVolume;
            //float volume = (_value) ? baseVolume : -80;
            //audioMixer.SetFloat(_type.ToString(), volume);
            ////audioMixer.GetFloat(_type.ToString(), out float value);
            ////Debug.Log(value.ToString());
            //PlayerPrefs.SetFloat(_type.ToString() + "Volume", volume);
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

