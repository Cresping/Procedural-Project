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
            _playerCombatController.DoDamageEnemy();
        }
        public void SwitchCombatInterface()
        {
            if (combatVariableSO.IsActive)
            {
                dialogueUI.SetActive(false);
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
            combatPlayerSprite.transform.position = playerSpriteOriginalPosition.position;
            Sequence playerAnimationAttack = SequencesTween.DOMoveAnimation(combatPlayerSprite.transform, new Vector2(combatPlayerSprite.transform.position.x - 2, combatPlayerSprite.transform.position.y), 0.1f);
            playerAnimationAttack.Play();
        }
        public void DoEnemyCombatAnimation()
        {
            combatEnemySprite.transform.position = enemySpriteOriginalPosition.position;
            Sequence enemyAnimationAttack = SequencesTween.DOMoveAnimation(combatEnemySprite.transform, new Vector2(combatEnemySprite.transform.position.x + 2, combatEnemySprite.transform.position.y), 0.1f);
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
