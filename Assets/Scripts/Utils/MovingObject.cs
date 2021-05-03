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

    protected virtual void Awake()
    {
        this._mCollider = GetComponent<BoxCollider2D>();
        this._mRb = GetComponent<Rigidbody2D>();
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
}
