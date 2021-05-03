using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Collider2D))]
public abstract class MovingObject : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected Pathfind.GridPathfind gridPathfind;
    [SerializeField] protected LayerMask blockingLayer;
    [SerializeField] protected LayerMask moveLayer;

    private BoxCollider2D _mCollider;
    private Rigidbody2D _mRb;
    private IEnumerator _smoothMovement;
    private IEnumerator _changeMovement;
    private bool _isMoving;
    private bool _isChangeMovement;
    private bool _isCoroutineSmoothMoveActive;
    private float _remainingDistance;
    private Vector2 _lastPositionCall;
    private Vector2 _nextPoint;

    protected virtual void Awake()
    {
        this._mCollider = GetComponent<BoxCollider2D>();
        this._mRb = GetComponent<Rigidbody2D>();
        this._smoothMovement = SmoothMovementPosition(Vector3.zero);
        this._remainingDistance = 0;
        this._isMoving = false;
        this._isChangeMovement = false;
        this._lastPositionCall = Vector2.zero;
        this._isCoroutineSmoothMoveActive = false;
        if (!this.gridPathfind)
        {
            this.gridPathfind = FindObjectOfType<Pathfind.GridPathfind>();
        }
    }
    protected virtual void Start() { }


    protected bool MoveTeleportMovementPosition(float xPos, float yPos, out RaycastHit2D hit)
    {
        List<Pathfind.Point> points;
        hit = new RaycastHit2D();
        points = Pathfind.Pathfinding.FindPath(gridPathfind, new Pathfind.Point((int)transform.position.x, (int)transform.position.y), new Pathfind.Point((int)(xPos), (int)(yPos)));
        if (points.Count > 0)
        {
            transform.position = points[0].Position;
            return true;
            // Vector2Int direction = Vector2Int.FloorToInt(Direction2D.GetDirection(transform.position, points[0].Position));
            // return MoveTeleportMovementDirection(direction.x, direction.y, out hit);
        }
        return false;
    }
    protected virtual bool AttemptTeleportMovement<T>(int xPos, int yPos) where T : Component
    {
        RaycastHit2D hit;
        bool canMove;
        Vector2Int lastPosition = Vector2Int.FloorToInt(transform.position);
        //      if (xDir > 1 || xDir < -1 || yDir > 1 || yDir < -1)
        canMove = MoveTeleportMovementPosition(xPos, yPos, out hit);
        if (canMove)
        {
            OnCanMove(lastPosition, Vector2Int.FloorToInt(transform.position));
            return true;
        }
        else
        {
            OnCantMove();
            return false;
        }
    }
    protected bool WanderAround()
    {
        RaycastHit2D hitComponent = new RaycastHit2D();
        List<int> indexOfDirections = new List<int>();
        Vector2Int lastPosition = Vector2Int.FloorToInt(transform.position);
        for (int i = 0; i < Direction2D.cardinalDirectionList.Count; i++)
        {
            indexOfDirections.Add(i);
        }
        while (indexOfDirections.Count > 0)
        {
            int randomNumber = indexOfDirections[Random.Range(0, indexOfDirections.Count)];
            Vector2Int aux = Direction2D.cardinalDirectionList[randomNumber];
            if (MoveTeleportMovementPosition(aux.x + Mathf.FloorToInt(transform.position.x), aux.y + Mathf.FloorToInt(transform.position.y), out hitComponent))
            {
                OnCanMove(lastPosition, Vector2Int.FloorToInt(transform.position));
                return true;
            }

            indexOfDirections.Remove(randomNumber);
        }
        return false;
    }
    protected virtual void OnCanMove(Vector2Int lastPosition, Vector2Int currentPosition)
    {
        gridPathfind.ChangeNode(currentPosition.x, currentPosition.y, false);
        gridPathfind.ChangeNode(lastPosition.x, lastPosition.y, true);
    }
    protected abstract void OnCantMove();

    #region Deprecated
    protected bool MoveTeleportMovementDirection(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = (Vector2)transform.position + new Vector2(0.5f, 0.5f);
        Vector2 end = start + new Vector2(xDir, yDir);
        hit = new RaycastHit2D();
        RaycastHit2D[] allHits;
        allHits = Physics2D.LinecastAll(start, end, blockingLayer);
        foreach (var checkHit in allHits)
        {
            if (checkHit.collider != _mCollider)
            {
                hit = checkHit;
            }
        }
        if (hit.transform == null)
        {
            transform.position = new Vector2(xDir + transform.position.x, yDir + transform.position.y);
            return true;
        }
        return false;
    }
    protected bool MoveSmoothMovementRaycast(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = (Vector2)transform.position + new Vector2(0.5f, 0.5f);
        Vector2 end = start + new Vector2(xDir, yDir);
        _mCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        _mCollider.enabled = true;
        if (hit.transform == null && !_isMoving)
        {
            _smoothMovement = SmoothMovementPosition(end);
            StartCoroutine(_smoothMovement);
            return true;
        }
        return false;
    }
    protected bool MoveToSmoothMovementPathfinding(int xPos, int yPos)
    {
        List<Pathfind.Point> points;
        Vector2 startPosition = transform.position;
        if (_isMoving)
        {
            startPosition = _nextPoint;
        }
        points = Pathfind.Pathfinding.FindPath(gridPathfind, new Pathfind.Point((int)startPosition.x, (int)startPosition.y), new Pathfind.Point((int)(xPos), (int)(yPos)));
        if (points.Count > 0 && _lastPositionCall != new Vector2(xPos, yPos))
        {
            _lastPositionCall = new Vector2(xPos, yPos);
            if (_isMoving)
            {
                if (_isChangeMovement)
                {
                    StopCoroutine(_changeMovement);
                }
                _changeMovement = ChangeMovement(points);
                StartCoroutine(_changeMovement);
            }
            else
            {
                _smoothMovement = SmoothMovementPoints(points);
                StartCoroutine(_smoothMovement);
            }

            return true;
        }
        return false;
    }
    protected IEnumerator SmoothMovementPoints(List<Pathfind.Point> points)
    {
        if (!_isCoroutineSmoothMoveActive)
        {
            _isCoroutineSmoothMoveActive = true;
            _isMoving = true;
            foreach (Pathfind.Point point in points)
            {
                float sqrtRemainingDistance = (transform.position - point.Position).sqrMagnitude + _remainingDistance;
                _nextPoint = point.Position;
                while (sqrtRemainingDistance > float.Epsilon)
                {
                    Vector3 newPosition = Vector3.MoveTowards(_mRb.position, point.Position, moveSpeed * Time.deltaTime);
                    _mRb.MovePosition(newPosition);
                    sqrtRemainingDistance = (transform.position - point.Position).sqrMagnitude;
                    _remainingDistance = sqrtRemainingDistance;
                    yield return null;
                }
                if (_isChangeMovement)
                {
                    _isChangeMovement = false;
                    break;
                }
            }
            _remainingDistance = 0;
            _isMoving = false;
            _isCoroutineSmoothMoveActive = false;
        }
    }
    protected IEnumerator SmoothMovementPosition(Vector3 end)
    {
        if (!_isCoroutineSmoothMoveActive)
        {
            _isCoroutineSmoothMoveActive = true;
            _isMoving = true;
            float sqrtRemainingDistance = (transform.position - end).sqrMagnitude + _remainingDistance;
            while (sqrtRemainingDistance > float.Epsilon)
            {
                Vector3 newPosition = Vector3.MoveTowards(_mRb.position, end, moveSpeed * Time.deltaTime);
                _mRb.MovePosition(newPosition);
                sqrtRemainingDistance = (transform.position - end).sqrMagnitude;
                _remainingDistance = sqrtRemainingDistance;
                yield return null;
            }
            _remainingDistance = 0;
            _isMoving = false;
            _isCoroutineSmoothMoveActive = false;
        }
    }
    protected IEnumerator ChangeMovement(List<Pathfind.Point> points)
    {
        _isChangeMovement = true;
        while (_isMoving)
        {
            yield return null;
        }
        _smoothMovement = SmoothMovementPoints(points);
        StartCoroutine(_smoothMovement);
    }
    protected virtual void AttemptSmoothMovement<T>(int xDir, int yDir) where T : Component
    {
        RaycastHit2D hit;
        bool canMove = MoveSmoothMovementRaycast(xDir, yDir, out hit);
        if (hit.transform == null)
        {
            return;
        }
        T hitComponent = hit.transform.GetComponent<T>();
        if (!canMove && hitComponent != null)
        {
            OnCantMove();
        }
    }
    #endregion
}
