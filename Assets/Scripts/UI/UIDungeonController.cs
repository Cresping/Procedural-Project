using HeroesGames.ProjectProcedural.Player;
using HeroesGames.ProjectProcedural.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using HeroesGames.ProjectProcedural.SO;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace HeroesGames.ProjectProcedural.UI
{
    /// <summary>
    /// Clase encargada de controlar la UI dentro de la mazmorra
    /// </summary>
    public class UIDungeonController : MonoBehaviour
    {
        [SerializeField] CombatVariableSO combatVariableSO;
        [SerializeField] PlayerVariableSO playerVariableSO;
        [SerializeField] PlayerInventoryVariableSO playerInventoryVariableSO;
        [SerializeField] TextMeshPro damagePrefab;
        [SerializeField] GameObject pauseMenu;

        [SerializeField] GameObject movementButtons;

        [SerializeField] GameObject combatButtons;
        [SerializeField] private GameObject combatUI;
        [SerializeField] private Button normalAttackButton;
        [SerializeField] private Button superAttackButton;
        [SerializeField] private SpriteRenderer combatPlayerSprite;
        [SerializeField] private SpriteRenderer combatEnemySprite;
        [SerializeField] private Transform enemySpriteOriginalPosition;
        [SerializeField] private Transform playerSpriteOriginalPosition;
        [SerializeField] private Animator combatEnemyAnimator;
        [SerializeField] private TextMeshPro combatEnemyHP;
        [SerializeField] private TextMeshPro combatPlayerHP;
        [SerializeField] private RectTransform combatPlayerDamagePosition;
        [SerializeField] private RectTransform combatEnemyDamagePosition;


        [SerializeField] private TextMeshProUGUI hpStatValue;
        [SerializeField] private TextMeshProUGUI attackStatValue;
        [SerializeField] private TextMeshProUGUI speedStatValue;
        [SerializeField] private TextMeshProUGUI defStatValue;

        [SerializeField] private GameObject dialogueUI;
        [SerializeField] private TextMeshProUGUI textBox;
        [SerializeField] private List<Image> starsBox;
        [SerializeField] private Sprite lockedStarSprite;
        [SerializeField] private Sprite unlockedStarSprite;

        [SerializeField] private GameObject levelUPUI;
        [SerializeField] private TextMeshProUGUI textLevelUP;

        [SerializeField] private GameObject messagesUI;
        [SerializeField] private TextMeshProUGUI messageText;

        [SerializeField] private TimerVariableSO timerVariableSO;
        [SerializeField] private TextMeshProUGUI textTimer;

        [SerializeField] private GameOverBusSO gameOverBusSO;
        [SerializeField] private GameObject gameOverUI;
        [SerializeField] private Button buttonGameOverExitMenu;
        [SerializeField] private TextMeshProUGUI dungeonLevelStatValue;
        [SerializeField] private TextMeshProUGUI playerLevelStatValue;
        [SerializeField] private TextMeshProUGUI enemiesKilledStatValue;
        [SerializeField] private int textBoxFadeTime;

        private PlayerController _playerController;
        private PlayerCombatController _playerCombatController;
        private GameObject _cameraHandler;
        private IEnumerator _coroutineReduceTimer;
        private void OnEnable()
        {
            combatVariableSO.OnCombatActivation += SwitchCombatInterface;
            playerInventoryVariableSO.OnInventoryChange += DoEnableItemTextBox;
            playerVariableSO.PlayerLevelOnValueChange += DoLevelUP;
            gameOverBusSO.OnGameOverEvent += DoGameOver;
            StartCoroutine(_coroutineReduceTimer);
        }
        private void OnDisable()
        {
            combatVariableSO.OnCombatActivation -= SwitchCombatInterface;
            playerInventoryVariableSO.OnInventoryChange -= DoEnableItemTextBox;
            playerVariableSO.PlayerLevelOnValueChange -= DoLevelUP;
            gameOverBusSO.OnGameOverEvent -= DoGameOver;
            try { StopCoroutine(_coroutineReduceTimer); } catch (NullReferenceException) { }
        }

        private void Awake()
        {
            pauseMenu.SetActive(false);
            _coroutineReduceTimer = ReduceTimer();
        }
        private void Start()
        {
            _playerController = FindObjectOfType<PlayerController>();
            _playerCombatController = FindObjectOfType<PlayerCombatController>();
            _cameraHandler = GameObject.FindGameObjectWithTag("CameraHandler");
            combatUI.SetActive(false);
            dialogueUI.SetActive(false);
            levelUPUI.gameObject.SetActive(false);
            gameOverUI.gameObject.SetActive(false);
            buttonGameOverExitMenu.interactable = false;
            DoChangeStats();

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
        public void ButtonExitDungeon()
        {
            Debug.Log("Cargando menu");
            playerVariableSO.ResetValues();
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
        public void ButtonAttack()
        {
            _playerCombatController.DoAttackEnemy();
        }
        public void ButtonStrongAttack()
        {
            _playerCombatController.DoStrongAttackEnemy();
        }
        public void SwitchCombatInterface()
        {
            if (combatVariableSO.IsActive)
            {
                DoStartCombat();
                combatEnemySprite.transform.position = enemySpriteOriginalPosition.position;
                combatPlayerSprite.transform.position = playerSpriteOriginalPosition.position;
                dialogueUI.SetActive(false);
                combatUI.transform.position = _cameraHandler.transform.position;
                movementButtons.SetActive(false);
                combatButtons.SetActive(true);
                combatUI.SetActive(true);
                combatVariableSO.OnCombatPlayerAttackAnimation += DoPlayerAttackAnimation;
                combatVariableSO.OnCombatPlayerStrongAttackAnimation += DoPlayerStrongAttackAnimation;
                combatVariableSO.OnCombatEnemyAnimation += DoEnemyAttackAnimation;
                combatVariableSO.OnCombatChangeEnemy += DoEnemyDeadAnimation;
                combatVariableSO.OnCombatEnemyReceiveDamage += DoReceiveDamageEnemy;
                combatVariableSO.OnCombatPlayerReceiveDamage += DoReceiveDamagePlayer;
            }
            else
            {
                DoEnemyDeadAnimation();
            }
        }
        public void DoStartCombat()
        {
            DoChangeEnemy();
            ChangePlayerHeal();
        }
        public void DoChangeEnemy()
        {
            ChangeEnemyHeal();
            ChangeEnemySprite();
            ChangeEnemyAnimator();
            normalAttackButton.interactable = true;
        }
        public void DoEnemyDeadAnimation()
        {
            combatEnemyAnimator.SetTrigger("Dead");
            normalAttackButton.interactable = false;
            superAttackButton.interactable = false;
            if (combatVariableSO.IsActive)
            {
                Invoke(nameof(DoChangeEnemy), 0.8f);
            }
            else
            {
                Invoke(nameof(ExitCombat), 0.8f);
            }
        }
        public void ExitCombat()
        {
            DOTween.KillAll();
            normalAttackButton.interactable = true;
            combatButtons.SetActive(false);
            combatUI.SetActive(false);
            movementButtons.SetActive(true);
            combatVariableSO.OnCombatPlayerAttackAnimation -= DoPlayerAttackAnimation;
            combatVariableSO.OnCombatPlayerStrongAttackAnimation -= DoPlayerStrongAttackAnimation;
            combatVariableSO.OnCombatEnemyAnimation -= DoEnemyAttackAnimation;
            combatVariableSO.OnCombatChangeEnemy -= DoEnemyDeadAnimation;
            combatVariableSO.OnCombatEnemyReceiveDamage -= DoReceiveDamageEnemy;
            combatVariableSO.OnCombatPlayerReceiveDamage -= DoReceiveDamagePlayer;
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
            DoChangeStats();
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
            combatEnemyAnimator.Rebind();
        }
        public void ChangePlayerHeal()
        {
            combatPlayerHP.text = "";
            for (int i = 0; i < playerVariableSO.PlayerHP; i++)
            {
                combatPlayerHP.text = combatPlayerHP.text + "|";
            }
        }
        public void DoPlayerStrongAttackAnimation()
        {
            combatPlayerSprite.transform.position = playerSpriteOriginalPosition.position;
            Sequence playerAnimationStrongAttack = DOTween.Sequence();
            playerAnimationStrongAttack.Append(SequencesTween.DOMoveAnimation(combatPlayerSprite.transform, new Vector2(combatPlayerSprite.transform.position.x - 1, combatPlayerSprite.transform.position.y + 2), 0.1f));
            playerAnimationStrongAttack.Append(SequencesTween.DOMoveAnimation(combatPlayerSprite.transform, new Vector2(combatPlayerSprite.transform.position.x - 1, combatPlayerSprite.transform.position.y - 2), 0.1f));
            playerAnimationStrongAttack.Append(SequencesTween.DOMoveAnimation(combatPlayerSprite.transform, new Vector2(combatPlayerSprite.transform.position.x, combatPlayerSprite.transform.position.y), 0.05f));
            playerAnimationStrongAttack.Play();
        }
        public void DoPlayerAttackAnimation()
        {
            combatPlayerSprite.transform.position = playerSpriteOriginalPosition.position;
            Sequence playerAnimationAttack = DOTween.Sequence();
            playerAnimationAttack.Append(SequencesTween.DOMoveAnimation(combatPlayerSprite.transform, new Vector2(combatPlayerSprite.transform.position.x - 2, combatPlayerSprite.transform.position.y), 0.05f));
            playerAnimationAttack.Append(SequencesTween.DOMoveAnimation(combatPlayerSprite.transform, new Vector2(combatPlayerSprite.transform.position.x, combatPlayerSprite.transform.position.y), 0.05f));
            playerAnimationAttack.Play();
        }
        public void DoEnemyAttackAnimation()
        {
            combatEnemySprite.transform.position = enemySpriteOriginalPosition.position;
            Sequence enemyAnimationAttack = DOTween.Sequence();
            enemyAnimationAttack.Append(SequencesTween.DOMoveAnimation(combatEnemySprite.transform, new Vector2(combatEnemySprite.transform.position.x + 2, combatEnemySprite.transform.position.y), 0.05f));
            enemyAnimationAttack.Append(SequencesTween.DOMoveAnimation(combatEnemySprite.transform, new Vector2(combatEnemySprite.transform.position.x, combatEnemySprite.transform.position.y), 0.05f));
            enemyAnimationAttack.Play();
        }
        public void DoEnableItemTextBox(ObjectInventoryVariableSO objectInventory)
        {
            CancelInvoke(nameof(DisableDialogueUI));
            CancelInvoke(nameof(DisableLevelUPUI));
            DisableLevelUPUI();
            for (int i = 0; i < starsBox.Count; i++)
            {
                if (i + 1 <= objectInventory.ObjectRarity)
                {
                    starsBox[i].sprite = unlockedStarSprite;
                }
                else
                {
                    starsBox[i].sprite = lockedStarSprite;
                }
            }
            textBox.text = "¡Has obtenido " + objectInventory.name + "!";
            dialogueUI.gameObject.SetActive(true);
            Invoke(nameof(DisableDialogueUI), textBoxFadeTime);
        }

        public void ShowMessages(string message, float msgDuration)
        {
            messageText.text = message;
            messagesUI.gameObject.SetActive(true);
            Invoke(nameof(DisableMessagesUI), msgDuration);
        }
        public void DoLevelUP()
        {
            CancelInvoke(nameof(DisableLevelUPUI));
            CancelInvoke(nameof(DisableDialogueUI));
            DisableDialogueUI();
            textLevelUP.text = "¡HAS SUBIDO AL NIVEL " + playerVariableSO.PlayerLevel + "!";
            levelUPUI.gameObject.SetActive(true);
            DoChangeStats();
            Invoke(nameof(DisableLevelUPUI), textBoxFadeTime);
        }
        public void DoChangeStats()
        {
            hpStatValue.text = playerVariableSO.PlayerHP.ToString();
            attackStatValue.text = playerVariableSO.RuntimePlayerDamage.ToString();
            speedStatValue.text = playerVariableSO.RuntimePlayerSpeed.ToString();
            defStatValue.text = playerVariableSO.RuntimePlayerDef.ToString();
        }
        public void DoGameOver()
        {
            try { StopCoroutine(_coroutineReduceTimer); } catch (NullReferenceException) { }
            dungeonLevelStatValue.text = playerVariableSO.DungeonLevel.ToString();
            playerLevelStatValue.text = playerVariableSO.PlayerLevel.ToString();
            enemiesKilledStatValue.text = playerVariableSO.NumberEnemiesKilled.ToString();
            textTimer.text = "00:00:00";
            gameOverUI.gameObject.SetActive(true);
            Invoke(nameof(EnableButtonGameOver), 2f);
        }
        public void EnableButtonGameOver()
        {
            buttonGameOverExitMenu.interactable = true;
        }
        public void DisableLevelUPUI()
        {
            levelUPUI.gameObject.SetActive(false);
        }
        public void DisableDialogueUI()
        {
            dialogueUI.gameObject.SetActive(false);
        }
        public void DisableMessagesUI()
        {
            messagesUI.gameObject.SetActive(false);
        }
        private IEnumerator ReduceTimer()
        {
            while (timerVariableSO.TimeSeconds > 0)
            {
                timerVariableSO.TimeSeconds -= Time.deltaTime;
                int seconds = Mathf.FloorToInt(timerVariableSO.TimeSeconds % 60F);
                int milliseconds = Mathf.FloorToInt((timerVariableSO.TimeSeconds * 100F) % 100F);
                textTimer.text = seconds.ToString("00") + ":" + milliseconds.ToString("00");
                yield return null;
            }
            textTimer.text = "00:00:00";
        }

    }

}
