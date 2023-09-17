using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MUGCUP
{
    public enum SoundType
    {
        SLOT_DESTROYED,
        SLOT_MATCHED,
        SHOOT_SMALL_BULLET,
        SHOOT_LARGE_BULLET,
        BULLET_HIT, 
        ENEMY_DEAD, 
        TOWER_HIT,
    }

    [Serializable]
    public class SoundData
    {
        public SoundType Type;
        public AudioClip AudioClip;
    }
    
    public class AudioManager : Service
    {
        public AudioSource BGMSource;
        public AudioSource Source;

        public List<SoundData> SoundData = new List<SoundData>();

        private Dictionary<SoundType, AudioClip> soundTable = new Dictionary<SoundType, AudioClip>();

        public bool BGMOn { get; private set; } = true;
        public bool SFXOn { get; private set; } = true;

        public override void Init()
        {
            if(IsInit)
                return;
            IsInit = true;
            
            BGMSource.Play();

            foreach (var _data in SoundData)
            {
                if(!soundTable.ContainsKey(_data.Type))
                    soundTable.Add(_data.Type, _data.AudioClip);
            }
        }

        public void Play(SoundType _type)
        {
            if (soundTable.TryGetValue(_type, out var _audioClip))
            {
                Source.clip = _audioClip;
                Source.PlayOneShot(_audioClip);
            }
        }

        public void ToggleBGM()
        {
            BGMOn = !BGMOn;
            BGMSource.enabled = BGMOn;
        }

        public void ToggleSFX()
        {
            SFXOn = !SFXOn;
            Source.enabled = SFXOn;
        }
    }
}
