using System.Collections;
using System.Collections.Generic;
using HeroesGames.ProjectProcedural.SO;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class SoundController : MonoBehaviour
    {
        [SerializeField] protected float timeBetweenSounds = 0;
        protected float _currentTimeBetweenSounds;
        protected AudioSource _myAudioSource;

        protected virtual void Awake()
        {
            this._currentTimeBetweenSounds = timeBetweenSounds;
            this._myAudioSource = GetComponent<AudioSource>();
        }
        public virtual void PlaySound(SoundVariableSO soundVariable)
        {
            if (_currentTimeBetweenSounds <= 0)
            {
                AudioClip randomClip = soundVariable.GetSoundClip();
                _myAudioSource.pitch = soundVariable.GetPitch();
                _myAudioSource.PlayOneShot(randomClip);
                _currentTimeBetweenSounds = timeBetweenSounds;
            }
            else
            {
                _currentTimeBetweenSounds -= Time.deltaTime;
            }
        }
        public virtual void PlaySound(SoundVariableSO soundVariable, out float soundDuration)
        {
            if (_currentTimeBetweenSounds <= 0)
            {
                AudioClip randomClip = soundVariable.GetSoundClip();
                _myAudioSource.pitch = soundVariable.GetPitch();
                soundDuration = randomClip.length;
                _myAudioSource.PlayOneShot(randomClip);
                _currentTimeBetweenSounds = timeBetweenSounds;
            }
            else
            {
                _currentTimeBetweenSounds -= Time.deltaTime;
                soundDuration = 0;
            }
        }
        public void StopSound()
        {
            _myAudioSource.Stop();
        }
    }

}
