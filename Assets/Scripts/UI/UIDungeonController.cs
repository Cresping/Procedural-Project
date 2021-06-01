using HeroesGames.ProjectProcedural.Player;
using HeroesGames.ProjectProcedural.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using HeroesGames.ProjectProcedural.SO;
using UnityEngine.UI;

namespace HeroesGames.ProjectProcedural.UI
{
    /// <summary>
    /// Clase encargada de controlar la UI dentro de la mazmorra
    /// </summary>
    public class UIDungeonController : MonoBehaviour
    {
        [SerializeField] CombatVariableSO combatVariableSO;
        [SerializeField] GameObject pauseMenu;

        [SerializeField] GameObject movementButtons;

        [SerializeField] GameObject combatButtons;

        [SerializeField] GameObject combatUI;

        [SerializeField] Image combatPlayerSprite;

        [SerializeField] Image combatEnemySprite;

        private PlayerController _playerController;
        private PlayerCombatController _playerCombatController;

        private void Awake()
        {
            pauseMenu.SetActive(false);
            combatVariableSO.OnCombatActivation += SwitchCombatInterface;
            combatVariableSO.OnCombatPlayerAnimation += DoPlayerCombatAnimation;
            combatVariableSO.OnCombatEnemyAnimation += DoEnemyCombatAnimation;
            combatVariableSO.OnCombatChangeEnemy += DoChangeEnemySprite;

        }
        private void Start()
        {
            _playerController = FindObjectOfType<PlayerController>();
            _playerCombatController = FindObjectOfType<PlayerCombatController>();
        }
        public void OnPressButtonUP()
        {
            _playerController.StartPlayerMovement(Vector2.up);
        }
        public void OnPressButtonDOWN()
        {
            _playerController.StartPlayerMovement(Vector2.down);
        }
        public void OnPressButtonLEFT()
        {
            _playerController.StartPlayerMovement(Vector2.left);
        }
        public void OnPressButtonRIGHT()
        {
            _playerController.StartPlayerMovement(Vector2.right);
        }
        public void OnReleaseButtonMovement()
        {
            _playerController.StopPlayerMovement();
        }
        public void ButtonPauseMenu(bool value)
        {
            pauseMenu.SetActive(value);
        }
        public void ButtonAttack()
        {
            _playerCombatController.DoDamageEnemy();
        }
        public void SwitchCombatInterface()
        {
            if (combatVariableSO.IsActive)
            {
                movementButtons.SetActive(false);
                combatButtons.SetActive(true);
                combatUI.SetActive(true);
            }
            else
            {
                combatButtons.SetActive(false);
                combatUI.SetActive(false);
                movementButtons.SetActive(true);
            }
        }
        public void DoPlayerCombatAnimation()
        {
            Sequence playerAnimationAttack = SequencesTween.DOMoveAnimation(combatPlayerSprite.transform, new Vector2(combatPlayerSprite.transform.position.x - 50, combatPlayerSprite.transform.position.y), 0.1f);
            playerAnimationAttack.Play();
        }
        public void DoEnemyCombatAnimation()
        {
            Sequence enemyAnimationAttack = SequencesTween.DOMoveAnimation(combatEnemySprite.transform, new Vector2(combatEnemySprite.transform.position.x + 50, combatEnemySprite.transform.position.y), 0.1f);
            enemyAnimationAttack.Play();
        }
        public void DoChangeEnemySprite()
        {
            combatEnemySprite.sprite = combatVariableSO.GetCurrentCombatEnemySO().EnemySprite;
        }
    }

}
