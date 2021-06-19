using System.Collections;
using System.Collections.Generic;
using HeroesGames.ProjectProcedural.SO;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.Sound
{
    public class SoundCombatController : SoundController
    {

        [SerializeField] private CombatVariableSO combatVariableSO;
        [SerializeField] private SoundVariableSO playerAttackSound;
        [SerializeField] private SoundVariableSO playerStrongAttackSound;
        [SerializeField] private SoundVariableSO enemyDeadSound;
        [SerializeField] private float pitchIncreasePerAttack = 0.05f;

        private void OnEnable()
        {
            combatVariableSO.OnCombatPlayerAttackAnimation += PlayPlayerAttackSound;
            combatVariableSO.OnCombatPlayerStrongAttackAnimation += PlayPlayerStrongAttackSound;
            combatVariableSO.OnCombatActivation += OnCombatActivation;
            combatVariableSO.OnCombatChangeEnemy += ResetPitchPlayerAttackSound;
            combatVariableSO.OnCombatEnemyDead += PlayEnemyDeadSound;
        }
        private void OnDisable()
        {
            combatVariableSO.OnCombatPlayerAttackAnimation -= PlayPlayerAttackSound;
            combatVariableSO.OnCombatPlayerStrongAttackAnimation -= PlayPlayerStrongAttackSound;
            combatVariableSO.OnCombatActivation -= OnCombatActivation;
            combatVariableSO.OnCombatChangeEnemy -= ResetPitchPlayerAttackSound;
            combatVariableSO.OnCombatEnemyDead -= PlayEnemyDeadSound;
        }
        private void OnCombatActivation()
        {
            ResetPitchPlayerAttackSound();
        }
        private void ResetPitchPlayerAttackSound()
        {
            playerAttackSound.ResetValues();
        }
        private void IncreasePitchPlayerAttackSound()
        {
            playerAttackSound.RuntimeMaxPitch += pitchIncreasePerAttack;
            playerAttackSound.RuntimeMinPitch += pitchIncreasePerAttack;
        }
        private void PlayPlayerAttackSound()
        {
            base.PlaySound(playerAttackSound);
            IncreasePitchPlayerAttackSound();
        }
        private void PlayPlayerStrongAttackSound()
        {
            base.PlaySound(playerStrongAttackSound);
        }
        private void PlayEnemyDeadSound()
        {
            base.PlaySound(enemyDeadSound);
        }

    }
}

