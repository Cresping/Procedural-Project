using System.Collections;
using System.Collections.Generic;
using HeroesGames.ProjectProcedural.SO;
using HeroesGames.ProjectProcedural.Utils;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.Enemies
{
    /// <summary>
    /// Clase encargada de controlar el comportamiento general de todos los enemigos
    /// </summary>
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

        /// <summary>
        /// Devuelve la distancia respecto a una posición
        /// </summary>
        /// <param name="position">Posición</param>
        /// <returns></returns>
        protected float CalculateDistance(Vector2 position)
        {
            return Vector2.Distance(transform.position, position);
        }

        /// <summary>
        /// Devuelve si el enemigo puede perseguir al jugador
        /// </summary>
        /// <returns>True si esta a la distancia adecuada, false si no</returns>
        protected bool CanSeePlayer()
        {
            float aux = CalculateDistance(playerVariableSO.PlayerPosition);
            return aux <= enemyVariableSO.PursuePlayerDistance;
        }

        /// <summary>
        /// Devuelve si el enemigo puede atacar al jugador
        /// </summary>
        /// <returns>True si esta a la distancia adecuada, false si no</returns>
        protected bool CanAttackPlayer()
        {
            return CalculateDistance(playerVariableSO.PlayerPosition) <= enemyVariableSO.AttackPlayerDistance;
        }

        /// <summary>
        /// Cambia el turno del enemigo, se actualiza cada vez que cambia la velocidad del jugador
        /// </summary>
        protected void ChangeMyTurn()
        {
            int previousTurn = _myTurn;
            this._myTurn = Mathf.FloorToInt((playerVariableSO.PlayerSpeed - enemyVariableSO.EnemySpeed) / TURNS_VALUE);
            _currentTurn += _myTurn - previousTurn;
        }

        /// <summary>
        /// Comprueba el turno del enemigo, si es menor o igual a 0 el enemigo podrá hacer algo
        /// </summary>
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

        /// <summary>
        /// Intenta perseguir al jugador
        /// </summary>
        /// <returns>True si existe camino, false si no</returns>
        protected virtual bool Pursue()
        {
            return base.AttemptTeleportMovement(Mathf.FloorToInt(playerVariableSO.PlayerPreviousPosition.x), Mathf.FloorToInt(playerVariableSO.PlayerPreviousPosition.y));
        }

        /// <summary>
        /// Acciones que realiza el enemigo
        /// </summary>
        protected abstract void DoSomething();

        /// <summary>
        /// Si no puede ni ver ni atacar al jugador, hará su Idle
        /// </summary>
        /// <returns>True si puede, false si no</returns>
        protected abstract bool Idle();

        /// <summary>
        /// Acciones que deba de hacer para atacar al jugador
        /// </summary>
        /// <returns>True si lo ha atacado, false si no</returns>
        protected abstract bool Attack();




    }

}
