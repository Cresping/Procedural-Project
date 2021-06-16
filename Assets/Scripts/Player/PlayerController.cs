using HeroesGames.ProjectProcedural.SO;
using HeroesGames.ProjectProcedural.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace HeroesGames.ProjectProcedural.Player
{
    /// <summary>
    /// Clase encargada de controlar el jugador
    /// </summary>
    public class PlayerController : MovingObject
    {
        [SerializeField] private AnimatorHelper animatorHelper;
        [SerializeField] private GameOverBusSO gameOverBusSO;
        [SerializeField] private CombatVariableSO combatVariableSO;
        [SerializeField] private PlayerVariableSO playerVariableSO;
        [SerializeField] private TimerVariableSO timerVariableSO;
        [SerializeField] private SpriteRenderer _mySprite;
        [SerializeField] private float timeBetweenKeystrokes = 0.5f;

        private InputActions _inputActions;
        private Vector2 _moveDirection;
        private Vector2 _previousPosition;
        private float _currentTimeBetweenKeystrokes;
        private bool _gameOver;
        private IEnumerator _coroutineMovement;


        protected override void Awake()
        {
            base.Awake();
            playerVariableSO.IsOnEvent = false;
            this._inputActions = new InputActions();
            this._previousPosition = transform.position;
            _currentTimeBetweenKeystrokes = 0;
            this.animatorHelper = new AnimatorHelper(GetComponentInChildren<Animator>());
        }
        protected override void Start()
        {
            base.Start();
            transform.position = playerVariableSO.PlayerPosition;
            _gameOver = false;

        }
        private void OnEnable()
        {
            _inputActions.Enable();
            _inputActions.PlayerController.Move.performed += TryMoveKeyboard;
            gameOverBusSO.OnGameOverEvent += DoGameOver;
        }
        private void OnDisable()
        {
            _inputActions.PlayerController.Move.performed -= TryMoveKeyboard;
            gameOverBusSO.OnGameOverEvent -= DoGameOver;
            _inputActions.Disable();
        }
        private void DoGameOver()
        {
            _gameOver = true;
        }
        /// <summary>
        /// M�dulo encargado de mover al jugador mediante botones en la pantalla del movil
        /// </summary>
        /// <param name="direction">Direcci�n donde debe moverse</param>
        private IEnumerator TryMoveButton(Vector2 direction)
        {
            while (!combatVariableSO.IsActive && !_gameOver)
            {
                _currentTimeBetweenKeystrokes += Time.deltaTime;
                if (_currentTimeBetweenKeystrokes >= timeBetweenKeystrokes)
                {
                    _currentTimeBetweenKeystrokes = 0;
                    _moveDirection = direction;
                    AttemptMovement((int)_moveDirection.x + Mathf.FloorToInt(transform.position.x), (int)_moveDirection.y + Mathf.FloorToInt(transform.position.y));
                }
                yield return 0;
            }
        }
        /// <summary>
        /// M�dulo encargado de mover al jugador mediante el teclado, usar solo para testear
        /// </summary>
        /// <param name="ctx">Direcci�n donde moverse</param>
        private void TryMoveKeyboard(CallbackContext ctx)
        {
            if (!combatVariableSO.IsActive && !_gameOver)
            {
                _moveDirection = ctx.ReadValue<Vector2>();
                AttemptMovement((int)_moveDirection.x + Mathf.FloorToInt(transform.position.x), (int)_moveDirection.y + Mathf.FloorToInt(transform.position.y));
            }
        }
        public void StartPlayerMovement(Vector2 direction)
        {
            _currentTimeBetweenKeystrokes = timeBetweenKeystrokes;
            try { StopCoroutine(_coroutineMovement); } catch (NullReferenceException) { }
            _coroutineMovement = TryMoveButton(direction);
            StartCoroutine(_coroutineMovement);
        }
        public void StopPlayerMovement()
        {
            try { StopCoroutine(_coroutineMovement); } catch (NullReferenceException) { }
        }
        /// <summary>
        /// M�dulo que se ejecuta si el jugador se puede mover. Cambia el 'Sprite' del jugador
        /// </summary>
        /// <param name="lastPosition">Posici�n antes de moverse</param>
        /// <param name="currentPosition">Posici�n despues de moverse</param>
        protected override void OnCanMove(Vector2Int lastPosition, Vector2Int currentPosition)
        {
            //base.OnCanMove(lastPosition, currentPosition);
            switch (Direction2D.GetFollowDirection(lastPosition, currentPosition).x)
            {
                case 0:
                    switch (Direction2D.GetFollowDirection(lastPosition, currentPosition).y)
                    {
                        case 0:
                            break;
                        case 1:
                            animatorHelper.SetWalkDown(false);
                            animatorHelper.SetWalkUp(true);
                            break;
                        case -1:
                            animatorHelper.SetWalkDown(true);
                            animatorHelper.SetWalkUp(false);
                            break;
                    }
                    break;
                case 1:
                    animatorHelper.SetWalkDown(false);
                    animatorHelper.SetWalkUp(false);
                    _mySprite.flipX = false;
                    break;
                case -1:
                    animatorHelper.SetWalkDown(false);
                    animatorHelper.SetWalkUp(false);
                    _mySprite.flipX = true;
                    break;
            }
            playerVariableSO.PlayerPreviousPosition = _previousPosition;
            _previousPosition = transform.position;
        }
        /// <summary>
        /// M�dulo que se ejecuta si el objeto no se puede mover
        /// </summary>
        protected override void OnCantMove()
        {
            base.OnCantMove();
            Debug.Log("No puedo moverme");
        }
        protected override void OnAlreadyMoving()
        {
            base.OnAlreadyMoving();
        }
        protected override void OnFinishMoving()
        {
            base.OnFinishMoving();
            playerVariableSO.PlayerPosition = transform.position;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector2 direction = Direction2D.cardinalDirectionList[0];
            Gizmos.DrawRay((Vector2)transform.position + new Vector2(0.5f, 0.5f), direction * 0.7f);
        }

    }
}

