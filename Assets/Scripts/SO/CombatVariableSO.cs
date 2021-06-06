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
        public Action OnCombatActivation;
        public Action OnCombatPlayerAnimation;
        public Action OnCombatEnemyAnimation;
        public Action OnCombatChangeEnemy;
        public Action<int> OnCombatEnemyReceiveDamage;
        public Action<int> OnCombatPlayerReceiveDamage;
        [SerializeField] private PlayerVariableSO playerVariableSO;
        private bool _isActive;
        private Stack<EnemyBehaviour> _stackCombatEnemyBehaviour;
        private EnemyBehaviour _currentCombatEnemyBehaviour;

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
                IsActive = true;
                NextEnemy();
            }
        }
        public void NextEnemy()
        {
            if (_stackCombatEnemyBehaviour.Count > 0)
            {
                _currentCombatEnemyBehaviour = _stackCombatEnemyBehaviour.Pop();
                OnCombatChangeEnemy?.Invoke();
            }
            else
            {
                EndCombat();
            }
        }
        public void DoDamageCurrentEnemy(int damage)
        {
            Debug.Log("voy a hacer daño");
            OnCombatPlayerAnimation?.Invoke();     
            OnCombatEnemyReceiveDamage?.Invoke(_currentCombatEnemyBehaviour.ReceiveDamage(damage));
            if (!_currentCombatEnemyBehaviour.gameObject.activeSelf)
            {
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
            _isActive = false;
            _stackCombatEnemyBehaviour = new Stack<EnemyBehaviour>();
        }

        public void OnBeforeSerialize() { }
    }

}
