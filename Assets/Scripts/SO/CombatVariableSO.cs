using System;
using System.Collections;
using System.Collections.Generic;
using HeroesGames.ProjectProcedural.Enemies;
using UnityEngine;


[CreateAssetMenu(fileName = nameof(CombatVariableSO), menuName = "Scriptables/" + nameof(CombatVariableSO) + "/" + nameof(CombatVariableSO) + "Variable")]
public class CombatVariableSO : ScriptableObject, ISerializationCallbackReceiver
{
    public Action OnCombatActivation;
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
        }
        else
        {
            EndCombat();
        }
    }
    public void DoDamageCurrentEnemy(int damage)
    {
        _currentCombatEnemyBehaviour.ReceiveDamage(damage);
        if (!_currentCombatEnemyBehaviour.gameObject.activeSelf)
        {
            NextEnemy();
        }
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
