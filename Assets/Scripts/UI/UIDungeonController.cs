using HeroesGames.ProjectProcedural.Player;
using HeroesGames.ProjectProcedural.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using HeroesGames.ProjectProcedural.SO;
using UnityEngine.UI;
using TMPro;

namespace HeroesGames.ProjectProcedural.UI
{
    /// <summary>
    /// Clase encargada de controlar la UI dentro de la mazmorra
    /// </summary>
    public class UIDungeonController : MonoBehaviour
    {
        [SerializeField] CombatVariableSO combatVariableSO;
        [SerializeField] PlayerVariableSO playerVariableSO;
        [SerializeField] TextMeshPro damagePrefab;
        [SerializeField] GameObject pauseMenu;

        [SerializeField] GameObject movementButtons;

        [SerializeField] GameObject combatButtons;
        [SerializeField] private GameObject combatUI;
        [SerializeField] private SpriteRenderer combatPlayerSprite;

        [SerializeField] private SpriteRenderer combatEnemySprite;
        [SerializeField] private Animator combatEnemyAnimator;
        [SerializeField] private TextMeshPro combatEnemyHP;
        [SerializeField] private TextMeshPro combatPlayerHP;
        [SerializeField] private RectTransform combatPlayerDamagePosition;
        [SerializeField] private RectTransform combatEnemyDamagePosition;

        private PlayerController _playerController;
        private PlayerCombatController _playerCombatController;
        private GameObject _cameraHandler;


        private void Awake()
        {
            pauseMenu.SetActive(false);
            combatVariableSO.OnCombatActivation += SwitchCombatInterface;
        }
        private void Start()
        {
            _playerController = FindObjectOfType<PlayerController>();
            _playerCombatController = FindObjectOfType<PlayerCombatController>();
            _cameraHandler = GameObject.FindGameObjectWithTag("CameraHandler");
            combatUI.SetActive(false);

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
                ChangePlayerHeal();
                combatUI.transform.position = _cameraHandler.transform.position;
                movementButtons.SetActive(false);
                combatButtons.SetActive(true);
                combatUI.SetActive(true);
                combatVariableSO.OnCombatPlayerAnimation += DoPlayerCombatAnimation;
                combatVariableSO.OnCombatEnemyAnimation += DoEnemyCombatAnimation;
                combatVariableSO.OnCombatChangeEnemy += DoChangeEnemy;
                combatVariableSO.OnCombatEnemyReceiveDamage += DoReceiveDamageEnemy;
                combatVariableSO.OnCombatPlayerReceiveDamage += DoReceiveDamagePlayer;
            }
            else
            {
                combatButtons.SetActive(false);
                combatUI.SetActive(false);
                movementButtons.SetActive(true);
                combatVariableSO.OnCombatPlayerAnimation -= DoPlayerCombatAnimation;
                combatVariableSO.OnCombatEnemyAnimation -= DoEnemyCombatAnimation;
                combatVariableSO.OnCombatChangeEnemy -= DoChangeEnemy;
                combatVariableSO.OnCombatEnemyReceiveDamage -= DoReceiveDamageEnemy;
                combatVariableSO.OnCombatPlayerReceiveDamage -= DoReceiveDamagePlayer;
            }
        }
        public void DoChangeEnemy()
        {
            ChangeEnemyHeal();
            ChangeEnemySprite();
            ChangeEnemyAnimator();
        }
        public void DoReceiveDamageEnemy(int damage)
        {
            InstantiateDamageText(damage, combatPlayerDamagePosition.position);
            ChangeEnemyHeal();
        }
        public void DoReceiveDamagePlayer(int damage)
        {
            InstantiateDamageText(damage, combatEnemyDamagePosition.position);
            ChangePlayerHeal();
        }
        public void InstantiateDamageText(int damage, Vector2 position)
        {
            damagePrefab.text = "" + damage;
            Instantiate(damagePrefab, position, Quaternion.identity, combatUI.transform);
        }
        public void ChangeEnemyHeal()
        {
            combatEnemyHP.text = "";
            for (int i = 0; i < combatVariableSO.GetCurrentEnemyHP(); i++)
            {
                combatEnemyHP.text = combatEnemyHP.text + "|";
            }
        }
        public void ChangeEnemySprite()
        {
            combatEnemySprite.sprite = combatVariableSO.GetCurrentCombatEnemySO().EnemySprite;
        }
        public void ChangeEnemyAnimator()
        {
            combatEnemyAnimator.runtimeAnimatorController = combatVariableSO.GetCurrentCombatEnemySO().EnemyAnimator;
        }
        public void ChangePlayerHeal()
        {
            combatPlayerHP.text = "";
            for (int i = 0; i < playerVariableSO.PlayerHP; i++)
            {
                combatPlayerHP.text = combatPlayerHP.text + "|";
            }
        }
        public void DoPlayerCombatAnimation()
        {
            Sequence playerAnimationAttack = SequencesTween.DOMoveAnimation(combatPlayerSprite.transform, new Vector2(combatPlayerSprite.transform.position.x - 2, combatPlayerSprite.transform.position.y), 0.1f);
            playerAnimationAttack.Play();
        }
        public void DoEnemyCombatAnimation()
        {
            Sequence enemyAnimationAttack = SequencesTween.DOMoveAnimation(combatEnemySprite.transform, new Vector2(combatEnemySprite.transform.position.x + 2, combatEnemySprite.transform.position.y), 0.1f);
            enemyAnimationAttack.Play();
        }

    }

}
