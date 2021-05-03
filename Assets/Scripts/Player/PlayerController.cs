using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MovingObject
{
    [SerializeField] PlayerVariableSO playerVariableSO;
    [SerializeField] private SpriteRenderer _mySprite;
    private InputActions _inputActions;
    private Vector2 _moveDirection;
    private Vector2 _previousPosition;

    protected override void Awake()
    {
        base.Awake();
        _inputActions = new InputActions();
        _previousPosition = transform.position;
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
        _moveDirection = direction;
        AttemptTeleportMovement<Collider2D>((int)_moveDirection.x, (int)_moveDirection.y);
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

    protected override void OnCantMove()
    {

    }

    //private void TryMoveTo(CallbackContext ctx)
    //{
    //Vector2 cameraRayPosition = Camera.main.ScreenToWorldPoint(ctx.ReadValue<Vector2>());
    //Vector3 aux = new Vector3(cameraRayPosition.x, cameraRayPosition.y, -10);
    //RaycastHit2D hit = Physics2D.Raycast(aux, Vector2.zero, Mathf.Infinity, moveLayer);
    //if (hit.collider != null)
    //{
    //Vector2 worldPoint = aux;
    //Vector3Int position = grid.WorldToCell(worldPoint);
    // Debug.Log(gridPathfind.GetNode(position.x,position.y).Walkable);
    // Debug.Log(Pathfind.Pathfinding.FindPath(gridPathfind,new Pathfind.Point((int) (transform.position.x-0.5f),(int) (transform.position.y-0.5f)),new Pathfind.Point((int) (position.x),(int) (position.y))).Count);
    //MoveToSmoothMovementPathfinding(position.x, position.y);
    //}
    // }

    protected override void AttemptSmoothMovement<T>(int xDir, int yDir)
    {
        base.AttemptSmoothMovement<T>(xDir, yDir);
    }

}
