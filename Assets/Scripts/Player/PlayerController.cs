using HeroesGames.ProjectProcedural.SO;
using HeroesGames.ProjectProcedural.Utils;
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
        [SerializeField] private CombatVariableSO combatVariableSO;
        [SerializeField] private PlayerVariableSO playerVariableSO;
        [SerializeField] private SpriteRenderer _mySprite;

        private InputActions _inputActions;
        private Vector2 _moveDirection;
        private Vector2 _previousPosition;
        private bool _canPressButton;

        protected override void Awake()
        {
            base.Awake();
            this._canPressButton = true;
            this._inputActions = new InputActions();
            this._previousPosition = transform.position;
        }
        protected override void Start()
        {
            base.Start();
            transform.position = playerVariableSO.PlayerStartPosition;
        }
        private void OnEnable()
        {
            _inputActions.Enable();
            _inputActions.PlayerController.Move.performed += TryMoveKeyboard;
        }
        private void OnDisable()
        {
            _inputActions.PlayerController.Move.performed -= TryMoveKeyboard;
            _inputActions.Disable();
        }
        /// <summary>
        /// M�dulo encargado de mover al jugador mediante botones en la pantalla del movil
        /// </summary>
        /// <param name="direction">Direcci�n donde debe moverse</param>
        public void TryMoveButton(Vector2 direction)
        {
            if (_canPressButton && !combatVariableSO.IsActive)
            {
                CancelInvoke(nameof(EnableButton));
                _canPressButton = false;
                _moveDirection = direction;
                AttemptMovement((int)_moveDirection.x + Mathf.FloorToInt(transform.position.x), (int)_moveDirection.y + Mathf.FloorToInt(transform.position.y));
                Invoke(nameof(EnableButton), gameVariableSO.TimeBetweenKeystrokes);
            }

        }
        /// <summary>
        /// M�dulo encargado de mover al jugador mediante el teclado, usar solo para testear
        /// </summary>
        /// <param name="ctx">Direcci�n donde moverse</param>
        private void TryMoveKeyboard(CallbackContext ctx)
        {
            if (!combatVariableSO.IsActive)
            {
                _moveDirection = ctx.ReadValue<Vector2>();
                AttemptMovement((int)_moveDirection.x + Mathf.FloorToInt(transform.position.x), (int)_moveDirection.y + Mathf.FloorToInt(transform.position.y));
            }
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
                    break;
                case 1:
                    _mySprite.flipX = false;
                    break;
                case -1:
                    _mySprite.flipX = true;
                    break;
            }
            playerVariableSO.PlayerPosition = currentPosition;
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
            Debug.Log("Ya me estoy moviendo");
        }

        /// <summary>
        /// M�dulo encargado de volver a permitir moverse al jugador
        /// </summary>
        private void EnableButton()
        {
            _canPressButton = true;
        }
    }
}

