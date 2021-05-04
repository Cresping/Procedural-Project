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
        private const float TIME_BETWEEE_BUTTONSTROKES = 0.1f;

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
        /// Módulo encargado de mover al jugador mediante botones en la pantalla del movil
        /// </summary>
        /// <param name="direction">Dirección donde debe moverse</param>
        public void TryMoveButton(Vector2 direction)
        {
            if (_canPressButton)
            {
                CancelInvoke(nameof(EnableButton));
                _canPressButton = false;
                _moveDirection = direction;
                AttemptTeleportMovement((int)_moveDirection.x + Mathf.FloorToInt(transform.position.x), (int)_moveDirection.y + Mathf.FloorToInt(transform.position.y));
                Invoke(nameof(EnableButton), TIME_BETWEEE_BUTTONSTROKES);
            }

        }
        /// <summary>
        /// Módulo encargado de mover al jugador mediante el teclado, usar solo para testear
        /// </summary>
        /// <param name="ctx">Dirección donde moverse</param>
        private void TryMoveKeyboard(CallbackContext ctx)
        {
            _moveDirection = ctx.ReadValue<Vector2>();
            AttemptTeleportMovement((int)_moveDirection.x + Mathf.FloorToInt(transform.position.x), (int)_moveDirection.y + Mathf.FloorToInt(transform.position.y));
        }
        /// <summary>
        /// Módulo que se ejecuta si el jugador se puede mover. Cambia el 'Sprite' del jugador
        /// </summary>
        /// <param name="lastPosition">Posición antes de moverse</param>
        /// <param name="currentPosition">Posición despues de moverse</param>
        protected override void OnCanMove(Vector2Int lastPosition, Vector2Int currentPosition)
        {
            base.OnCanMove(lastPosition, currentPosition);
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
            playerVariableSO.PlayerPosition = transform.position;
            playerVariableSO.PlayerPreviousPosition = _previousPosition;
            _previousPosition = transform.position;
        }
        /// <summary>
        /// Módulo que se ejecuta si el objeto no se puede mover
        /// </summary>
        protected override void OnCantMove() { }

        /// <summary>
        /// Módulo encargado de volver a permitir moverse al jugador
        /// </summary>
        private void EnableButton()
        {
            _canPressButton = true;
        }
    }
}
    
