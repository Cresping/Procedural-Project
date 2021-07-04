using System.Collections;
using System.Collections.Generic;
using HeroesGames.ProjectProcedural.SO;
using UnityEngine;
namespace HeroesGames.ProjectProcedural.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicController : MonoBehaviour
    {
        [SerializeField] private GameOverBusSO gameOverBusSO;
        private AudioSource _musicSource;
        private void Awake()
        {
            _musicSource = GetComponentInChildren<AudioSource>();
        }
        private void OnEnable()
        {
            gameOverBusSO.OnGameOverEvent += DisableMusic;
        }
        private void OnDisable()
        {
            gameOverBusSO.OnGameOverEvent -= DisableMusic;
        }
        private void DisableMusic()
        {
            _musicSource.Stop();
        }
    }

}
