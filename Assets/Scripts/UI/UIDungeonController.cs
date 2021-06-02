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

        private SpriteRenderer _combatPlayerSprite;

        private SpriteRenderer _combatEnemySprite;
        private Animator _combatEnemyAnimator;

        private PlayerController _playerController;
        private PlayerCombatController _playerCombatController;
        private GameObject _combatUI;

        private void Awake()
        {
            pauseMenu.SetActive(false);
            combatVariableSO.OnCombatActivation += SwitchCombatInterface;
            combatVariableSO.OnCombatPlayerAnimation += DoPlayerCombatAnimation;
            combatVariableSO.OnCombatEnemyAnimation += DoEnemyCombatAnimation;
            combatVariableSO.OnCombatChangeEnemy += DoChangeEnemySprite;
            combatVariableSO.OnCombatChangeEnemy += DoChangeEnemyAnimator;

        }
        private void Start()
        {
            _playerController = FindObjectOfType<PlayerController>();
            _playerCombatController = FindObjectOfType<PlayerCombatController>();
            _combatUI = GameObject.FindGameObjectWithTag("CombatUI");
            _combatPlayerSprite = GameObject.FindGameObjectWithTag("CombatUIPlayerSprite").GetComponent<SpriteRenderer>();
            _combatEnemySprite = GameObject.FindGameObjectWithTag("CombatUIEnemySprite").GetComponent<SpriteRenderer>();
            _combatEnemyAnimator = GameObject.FindGameObjectWithTag("CombatUIEnemySprite").GetComponent<Animator>();
            _combatUI.SetActive(false);

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
                _combatUI.SetActive(true);
            }
            else
            {
                combatButtons.SetActive(false);
                _combatUI.SetActive(false);
                movementButtons.SetActive(true);
            }
        }
        public void DoPlayerCombatAnimation()
        {
            Sequence playerAnimationAttack = SequencesTween.DOMoveAnimation(_combatPlayerSprite.transform, new Vector2(_combatPlayerSprite.transform.position.x - 2, _combatPlayerSprite.transform.position.y), 0.1f);
            playerAnimationAttack.Play();
        }
        public void DoEnemyCombatAnimation()
        {
            Sequence enemyAnimationAttack = SequencesTween.DOMoveAnimation(_combatEnemySprite.transform, new Vector2(_combatEnemySprite.transform.position.x + 2, _combatEnemySprite.transform.position.y), 0.1f);
            enemyAnimationAttack.Play();
        }
        public void DoChangeEnemySprite()
        {
            _combatEnemySprite.sprite = combatVariableSO.GetCurrentCombatEnemySO().EnemySprite;
        }
        public void DoChangeEnemyAnimator()
        {
            _combatEnemyAnimator.runtimeAnimatorController = combatVariableSO.GetCurrentCombatEnemySO().EnemyAnimator;
        }
    }

}
