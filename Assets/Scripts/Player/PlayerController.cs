using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MovingObject
{
    private const float TIME_BETWEEE_KEYSTROKES = 0.1f;
    [SerializeField] PlayerVariableSO playerVariableSO;
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
        _inputActions.PlayerController.Move.performed += TryMove;
    }
    private void OnDisable()
    {
        _inputActions.PlayerController.Move.performed -= TryMove;
        _inputActions.Disable();
    }
    public void TryMoveButton(Vector2 direction)
    {
        if (_canPressButton)
        {
            CancelInvoke(nameof(EnableKeystroke));
            _canPressButton = false;
            _moveDirection = direction;
            AttemptTeleportMovement<Collider2D>((int)_moveDirection.x + Mathf.FloorToInt(transform.position.x), (int)_moveDirection.y + Mathf.FloorToInt(transform.position.y));
            Invoke(nameof(EnableKeystroke), TIME_BETWEEE_KEYSTROKES);
        }

    }
    private void TryMove(CallbackContext ctx)
    {
        _moveDirection = ctx.ReadValue<Vector2>();
        AttemptTeleportMovement<Collider2D>((int)_moveDirection.x + Mathf.FloorToInt(transform.position.x), (int)_moveDirection.y + Mathf.FloorToInt(transform.position.y));
    }

    protected override void OnCanMove(Vector2Int lastPosition, Vector2Int currentPosition)
    {
        base.OnCanMove(lastPosition, currentPosition);
        switch (Direction2D.GetDirection(lastPosition, currentPosition).x)
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

    protected override void OnCantMove() { }

    private void EnableKeystroke()
    {
        _canPressButton = true;
    }
}
