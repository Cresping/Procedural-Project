using System.Collections;
using System.Collections.Generic;
using HeroesGames.ProjectProcedural.SO;
using UnityEngine;
namespace HeroesGames.ProjectProcedural.Sound
{
    public class SoundGeneralController : SoundController
    {
        [SerializeField] private GameOverBusSO gameOverBusSO;
        [SerializeField] private SoundVariableSO gameOverSound;

        private void OnEnable()
        {
            gameOverBusSO.OnGameOverEvent += PlayGameOverSound;
        }
        private void OnDisable()
        {
            gameOverBusSO.OnGameOverEvent -= PlayGameOverSound;
        }
        private void PlayGameOverSound()
        {
            base.PlaySound(gameOverSound);
        }
    }
}
