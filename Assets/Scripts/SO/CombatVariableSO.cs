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
    private Stack<EnemyBehaviour> _combatEnemyBehaviour;
    private EnemyBehaviour _currentEnemyBehaviour;

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
        _combatEnemyBehaviour.Push(combatEnemyBehaviour);
        if (!_isActive)
        {
            StartCombat();
        }
    }
    public void StartCombat()
    {
        if (_combatEnemyBehaviour.Count > 0)
        {
            IsActive = true;
            NextEnemy();
        }
    }
    public void NextEnemy()
    {
        if (_combatEnemyBehaviour.Count > 0)
        {
            _currentEnemyBehaviour = _combatEnemyBehaviour.Pop();
        }
        else
        {
            EndCombat();
        }
    }
    public void DoDamageCurrentEnemy(int damage)
    {
        _currentEnemyBehaviour.ReceiveDamage(damage);
        if (!_currentEnemyBehaviour.gameObject.activeSelf)
        {
            NextEnemy();
        }
    }
    public void EndCombat()
    {
        _combatEnemyBehaviour = new Stack<EnemyBehaviour>();
        IsActive = false;
    }
    public void OnAfterDeserialize()
    {
        _isActive = false;
        _combatEnemyBehaviour = new Stack<EnemyBehaviour>();
    }

    public void OnBeforeSerialize() { }
}
