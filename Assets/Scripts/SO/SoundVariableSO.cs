using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.SO
{
    [CreateAssetMenu(fileName = "SoundVariableSO", menuName = "Scriptables/Sound/SoundVariableSO")]
    public class SoundVariableSO : ScriptableObject
    {
        [SerializeField] private List<AudioClip> soundClip;
        [SerializeField] private float maxPitch = 1;
        [SerializeField] private float minPitch = 1;
        private float _runtimeMaxPitch;
        private float _runtimeMinPitch;

        public float RuntimeMinPitch { get => _runtimeMinPitch; set => _runtimeMinPitch = value; }
        public float RuntimeMaxPitch { get => _runtimeMaxPitch; set => _runtimeMaxPitch = value; }

        public AudioClip GetSoundClip()
        {
            if (soundClip.Count > 0)
            {
                int n = UnityEngine.Random.Range(0, soundClip.Count);
                return soundClip[n];
            }
            else
            {
                return null;
            }
        }
        public float GetPitch()
        {
            return Random.Range(_runtimeMinPitch, _runtimeMaxPitch);
        }
        public void ResetValues()
        {
            _runtimeMaxPitch = maxPitch;
            _runtimeMinPitch = minPitch;
        }
        private void OnEnable() 
        {
            ResetValues(); 
        }
        
    }
}
