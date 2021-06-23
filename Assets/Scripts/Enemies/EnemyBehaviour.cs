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
        [SerializeField] protected AnimatorHelper animatorHelper;
        [SerializeField] protected CombatVariableSO combatVariableSO;
        [SerializeField] protected EnemyVariableSO enemyVariableSO;
        [SerializeField] protected PlayerVariableSO playerVariableSO;
        [SerializeField] protected SpriteRenderer spriteRenderer;

        private int _currentEnemyHP;

        private int _myTurn;
        private int _currentTurn;
        private bool _inCombat = false;
        private bool _isMyTurnAttack = false;

        public EnemyVariableSO EnemyVariableSO { get => enemyVariableSO; protected set => enemyVariableSO = value; }
        public int CurrentEnemyHP { get => _currentEnemyHP; set => _currentEnemyHP = value; }

        protected override void Awake()
        {
            base.Awake();
            animatorHelper = new AnimatorHelper(GetComponentInChildren<Animator>());
        }
        protected override void Start()
        {
            base.Start();
            this._currentEnemyHP = enemyVariableSO.MaxEnemyHP;
            this._myTurn = Mathf.FloorToInt((playerVariableSO.RuntimePlayerSpd - enemyVariableSO.EnemySpeed) / base.gameVariableSO.TurnSpeedValue);
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
        /// Devuelve la distancia respecto a una posici�n
        /// </summary>
        /// <param name="position">Posici�n</param>
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
            this._myTurn = Mathf.FloorToInt((playerVariableSO.RuntimePlayerSpd - enemyVariableSO.EnemySpeed) / gameVariableSO.TurnSpeedValue);
            _currentTurn += _myTurn - previousTurn;
        }

        /// <summary>
        /// Comprueba el turno del enemigo, si es menor o igual a 0 el enemigo podr� hacer algo
        /// </summary>
        protected void CheckTurn()
        {
            if (_currentEnemyHP > 0)
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
                if (CanAttackPlayer() && !_inCombat)
                {
                    if (Attack())
                    {
                        _inCombat = true;
                        StartCoroutine(coroutineCombat());
                    }
                }
            }
        }

        /// <summary>
        /// Intenta perseguir al jugador
        /// </summary>
        /// <returns>True si existe camino, false si no</returns>
        protected virtual bool Pursue()
        {
            return base.AttemptMovement(Mathf.FloorToInt(playerVariableSO.PlayerPreviousPosition.x), Mathf.FloorToInt(playerVariableSO.PlayerPreviousPosition.y));
        }

        /// <summary>
        /// Acciones que realiza el enemigo
        /// </summary>
        protected abstract void DoSomething();

        /// <summary>
        /// Si no puede ni ver ni atacar al jugador, har� su Idle
        /// </summary>
        /// <returns>True si puede, false si no</returns>
        protected abstract bool Idle();

        /// <summary>
        /// Acciones que deba de hacer para atacar al jugador
        /// </summary>
        /// <returns>True si lo ha atacado, false si no</returns>
        protected abstract bool Attack();
        protected override void OnFinishMoving()
        {
            if (CanAttackPlayer() && !_inCombat)
            {
                if (Attack())
                {
                    _inCombat = true;
                    StartCoroutine(coroutineCombat());
                }
            }
        }
        public int ReceiveDamage(int damage)
        {
            int actualDamage = damage - enemyVariableSO.EnemyDefense;
            if (actualDamage <= 0)
            {
                actualDamage = 1;
            }
            _currentEnemyHP -= actualDamage;
            if (_currentEnemyHP <= 0)
            {
                playerVariableSO.ReceiveExperience(enemyVariableSO.EnemyExperience);
                DisableEnemy();
            }
            return actualDamage;
        }
        public void DisableEnemy()
        {
            RemoveObjectPathfind(Vector2Int.FloorToInt((Vector2)transform.position));
            this.gameObject.SetActive(false);
        }
        public void EnableTurnAttackWithDelay(float delay)
        {
            Invoke(nameof(EnableTurnAttack), delay);
        }
        public void EnableTurnAttack()
        {
            _isMyTurnAttack = true;
        }
        protected IEnumerator coroutineCombat()
        {
            while (combatVariableSO.IsActive && _currentEnemyHP > 0)
            {
                if (_isMyTurnAttack)
                {
                    yield return new WaitForSecondsRealtime(enemyVariableSO.EnemyAttackSpeed);
                    if (combatVariableSO.IsActive && _currentEnemyHP > 0)
                    {
                        combatVariableSO.DoDamagePlayer(enemyVariableSO.EnemyAttack);
                    }
                }
                else
                {
                    yield return null;
                }
            }
            _isMyTurnAttack = false;
        }
    }
}
