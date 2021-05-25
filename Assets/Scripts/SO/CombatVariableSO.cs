using System;
using System.Collections;
using System.Collections.Generic;
using HeroesGames.ProjectProcedural.Enemies;
using UnityEngine;


[CreateAssetMenu(fileName = nameof(CombatVariableSO), menuName = "Scriptables/" + nameof(CombatVariableSO) + "/" + nameof(CombatVariableSO) + "Variable")]
public class CombatVariableSO : ScriptableObject,ISerializationCallbackReceiver
{
    public Action OnCombatActivation;
    private bool _isActive;
    private EnemyVariableSO _combatEnemySO;
    private EnemyBehaviour _combatEnemyBehaviour;

    public bool IsActive
    {
        get => _isActive;
        private set
        {
            _isActive = value;
            OnCombatActivation?.Invoke();
        }
    }

    public EnemyVariableSO CombatEnemySO { get => _combatEnemySO; set => _combatEnemySO = value; }
    public EnemyBehaviour CombatEnemyBehaviour { get => _combatEnemyBehaviour; set => _combatEnemyBehaviour = value; }

    public void ActivateCombat(EnemyVariableSO combatEnemySO, EnemyBehaviour combatEnemyBehaviour)
    {
        _combatEnemySO = combatEnemySO;
        _combatEnemyBehaviour = combatEnemyBehaviour;
        IsActive = true;
    }
    public void DesactivateCombat()
    {
        _combatEnemySO = null;
        _combatEnemyBehaviour = null;
        IsActive = false;
    }

    public void OnAfterDeserialize()
    {
        _isActive = false;
    }

    public void OnBeforeSerialize(){}
}
