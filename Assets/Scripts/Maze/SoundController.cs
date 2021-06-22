using UnityEngine;

namespace HeroesGames.ProjectProcedural.Procedural
{
    public class SoundController : MonoBehaviour
    {
        private AudioSource _audioSource;

        private void Awake() => _audioSource = GetComponent<AudioSource>();

            public void PlaySound(AudioClip audioClip)
        {
            _audioSource.clip = audioClip;
            _audioSource.pitch = Random.Range(0.8f, 1.0f);
            _audioSource.panStereo = Random.Range(-0.6f, 0.6f);
            _audioSource.volume = 1f;
            _audioSource.Play();
        }
    }
}

