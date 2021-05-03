using System.Collections;
using System.Collections.Generic;
using HeroesGames.ProjectProcedural.SO;
using UnityEngine;

public abstract class EnemyBehaviour : MovingObject
{
    private const int TURNS_VALUE = 5;
    [SerializeField] protected EnemyVariableSO enemyVariableSO;
    [SerializeField] protected PlayerVariableSO playerVariableSO;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    private int _currentEnemyHP;

    private int _myTurn;
    private int _currentTurn;
    protected override void Awake()
    {
        base.Awake();
        this._currentEnemyHP = enemyVariableSO.MaxEnemyHP;
        this._myTurn = Mathf.FloorToInt((playerVariableSO.PlayerSpeed - enemyVariableSO.EnemySpeed) / TURNS_VALUE);
        this._currentTurn = _myTurn;
    }

    protected virtual void OnEnable()
    {
        playerVariableSO.PlayerPositionOnValueChange += CheckTurn;
        playerVariableSO.PlayerSpeedOnValueChange += ChangeMyTurn;
    }
    protected virtual void OnDisable()
    {
        playerVariableSO.PlayerPositionOnValueChange -= CheckTurn;
        playerVariableSO.PlayerSpeedOnValueChange -= ChangeMyTurn;
    }
    protected float CalculateDistance(Vector2 position)
    {
        return Vector2.Distance(transform.position, position);
    }
    protected bool CanPursuePlayer()
    {
        float aux = CalculateDistance(playerVariableSO.PlayerPosition);
        return aux <= enemyVariableSO.PursuePlayerDistance;
    }
    protected bool CanAttackPlayer()
    {
        return CalculateDistance(playerVariableSO.PlayerPosition) <= enemyVariableSO.AttackPlayerDistance;
    }
    protected void ChangeMyTurn()
    {
        int previousTurn = _myTurn;
        this._myTurn = Mathf.FloorToInt((playerVariableSO.PlayerSpeed - enemyVariableSO.EnemySpeed) / TURNS_VALUE);
        _currentTurn += _myTurn- previousTurn;
    }
    protected void CheckTurn()
    {
        if (_currentTurn <= 0)
        {
            DoSomething();
            _currentTurn = _myTurn;
        }
        else
        {
            _currentTurn--;
        }
    }
    protected void DoSomething()
    {
        if (CanAttackPlayer())
        {
            Attack();
        }
        else if (CanPursuePlayer())
        {
            //   Debug.Log("Soy " + this.gameObject.name + " estoy persiguiendo al Player");
            if (Pursue())
            {
                return;
            }
        }
        TryMove();
    }
    protected virtual bool Pursue()
    {
        return base.AttemptTeleportMovement<Collider2D>(Mathf.FloorToInt(playerVariableSO.PlayerPreviousPosition.x), Mathf.FloorToInt(playerVariableSO.PlayerPreviousPosition.y));
    }
    protected abstract bool TryMove();
    protected abstract bool Attack();


}
