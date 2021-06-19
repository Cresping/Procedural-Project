using System;
using System.Collections;
using System.Collections.Generic;
using HeroesGames.ProjectProcedural.Enemies;
using UnityEngine;


namespace HeroesGames.ProjectProcedural.SO
{
    [CreateAssetMenu(fileName = nameof(CombatVariableSO), menuName = "Scriptables/" + nameof(CombatVariableSO) + "/" + nameof(CombatVariableSO) + "Variable")]
    public class CombatVariableSO : ScriptableObject, ISerializationCallbackReceiver
    {
        private const int MIN_NORMAL_ATTACKS_TO_UNLOCK_STRONG_ATTACK = 10;
        public Action OnCombatActivation;
        public Action<int> OnCombatPlayerAttack;
        public Action OnCombatPlayerAttackAnimation;
        public Action OnCombatPlayerStrongAttackAnimation;
        public Action OnCombatEnemyAnimation;
        public Action OnCombatChangeEnemy;
        public Action OnCombatEnemyDead;
        public Action OnCombatPlayerStrongAttackUnlocked;
        public Action<int> OnCombatEnemyReceiveDamage;
        public Action<int> OnCombatPlayerReceiveDamage;
        [SerializeField] private PlayerVariableSO playerVariableSO;
        [SerializeField] private GameOverBusSO gameOverBusSO;
        private bool _isActive;
        private int _contNormalAttacksPlayer;

        private Stack<EnemyBehaviour> _stackCombatEnemyBehaviour;
        private EnemyBehaviour _currentCombatEnemyBehaviour;


        private void OnEnable()
        {
            gameOverBusSO.OnGameOverEvent += EndCombat;
        }
        private void OnDisable()
        {
            gameOverBusSO.OnGameOverEvent -= EndCombat;
        }
        public bool IsActive
        {
            get => _isActive;
            private set
            {
                _isActive = value;
                OnCombatActivation?.Invoke();
            }
        }

        public void AddEnemy(EnemyBehaviour combatEnemyBehaviour)
        {
            _stackCombatEnemyBehaviour.Push(combatEnemyBehaviour);
            if (!_isActive)
            {
                StartCombat();
            }
        }
        public void StartCombat()
        {
            if (_stackCombatEnemyBehaviour.Count > 0)
            {
                OnCombatPlayerAttack?.Invoke(_contNormalAttacksPlayer);
                NextEnemy();
                IsActive = true;
                if (_contNormalAttacksPlayer >= MIN_NORMAL_ATTACKS_TO_UNLOCK_STRONG_ATTACK)
                {
                    OnCombatPlayerStrongAttackUnlocked?.Invoke();
                    OnCombatPlayerAttack(_contNormalAttacksPlayer);
                }
            }
        }
        public void NextEnemy()
        {
            if (_stackCombatEnemyBehaviour.Count > 0)
            {
                _currentCombatEnemyBehaviour = _stackCombatEnemyBehaviour.Pop();
                if (_currentCombatEnemyBehaviour.CurrentEnemyHP > 0)
                {
                    if (IsActive)
                    {
                        _currentCombatEnemyBehaviour.EnableTurnAttackWithDelay(0.8f);
                        OnCombatChangeEnemy?.Invoke();
                        if (_contNormalAttacksPlayer >= MIN_NORMAL_ATTACKS_TO_UNLOCK_STRONG_ATTACK)
                        {
                            OnCombatPlayerStrongAttackUnlocked?.Invoke();
                            OnCombatPlayerAttack(_contNormalAttacksPlayer);
                        }
                    }
                    else
                    {
                        _currentCombatEnemyBehaviour.EnableTurnAttack();
                    }
                }
                else
                {
                    NextEnemy();
                }
            }
            else
            {
                EndCombat();
            }
        }
        public void DoDamageCurrentEnemy(int damage)
        {
            OnCombatPlayerAttackAnimation?.Invoke();
            OnCombatEnemyReceiveDamage?.Invoke(_currentCombatEnemyBehaviour.ReceiveDamage(damage));
            _contNormalAttacksPlayer++;
            if (_contNormalAttacksPlayer == MIN_NORMAL_ATTACKS_TO_UNLOCK_STRONG_ATTACK)
            {
                OnCombatPlayerStrongAttackUnlocked?.Invoke();
            }
            if (_contNormalAttacksPlayer <= MIN_NORMAL_ATTACKS_TO_UNLOCK_STRONG_ATTACK)
            {
                OnCombatPlayerAttack(_contNormalAttacksPlayer);
            }
            if (_currentCombatEnemyBehaviour.CurrentEnemyHP <= 0)
            {
                Debug.Log("Siguiente enemigo");
                playerVariableSO.NumberEnemiesKilled++;
                OnCombatEnemyDead?.Invoke();
                NextEnemy();
            }
        }
        public void DoStrongDamageCurrentEnemy(int damage)
        {
            OnCombatPlayerStrongAttackAnimation?.Invoke();
            OnCombatEnemyReceiveDamage?.Invoke(_currentCombatEnemyBehaviour.ReceiveDamage(damage));
            _contNormalAttacksPlayer = 0;
            OnCombatPlayerAttack(_contNormalAttacksPlayer);
            if (_currentCombatEnemyBehaviour.CurrentEnemyHP <= 0)
            {
                Debug.Log("Siguiente enemigo");
                playerVariableSO.NumberEnemiesKilled++;
                OnCombatEnemyDead?.Invoke();
                NextEnemy();
            }
        }
        public void DoDamagePlayer(int damage)
        {
            OnCombatEnemyAnimation?.Invoke();
            OnCombatPlayerReceiveDamage?.Invoke(playerVariableSO.ReceiveDamage(damage));
        }
        public EnemyVariableSO GetCurrentCombatEnemySO()
        {
            return _currentCombatEnemyBehaviour.EnemyVariableSO;
        }
        public int GetCurrentEnemyHP()
        {
            return _currentCombatEnemyBehaviour.CurrentEnemyHP;
        }
        public void EndCombat()
        {
            _stackCombatEnemyBehaviour = new Stack<EnemyBehaviour>();
            IsActive = false;
        }
        public void OnAfterDeserialize()
        {
            ResetValues();
        }
        public void ResetValues()
        {
            _contNormalAttacksPlayer = 0;
            _isActive = false;
            _stackCombatEnemyBehaviour = new Stack<EnemyBehaviour>();
        }

        public void OnBeforeSerialize() { }
    }

}
