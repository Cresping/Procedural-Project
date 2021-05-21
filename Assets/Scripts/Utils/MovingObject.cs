using System.Collections;
using System.Collections.Generic;
using HeroesGames.ProjectProcedural.SO;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.Utils
{
    /// <summary>
    /// Clase encargada de controlar el movimiento de los objetos en el grid
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Collider2D))]
    public abstract class MovingObject : MonoBehaviour
    {
        [SerializeField] protected GameVariableSO gameVariableSO;
        [SerializeField] protected Pathfind.GridPathfind gridPathfind;

        protected bool _isMoving;

        protected virtual void Awake()
        {
            _isMoving = false;
            if (!this.gridPathfind)
            {
                this.gridPathfind = FindObjectOfType<Pathfind.GridPathfind>();
            }
        }
        protected virtual void Start() { }

        /// <summary>
        /// Mueve al objeto de su posici�n actual a la posici�n dada siempre que sea posible
        /// </summary>
        /// <param name="xPos">Posici�n X en el grid</param>
        /// <param name="yPos">Posici�n Y en el grid</param>
        /// <returns>'False' si no existe camino posible, 'True' si existe camino</returns>
        protected bool TeleportMovementPosition(float xPos, float yPos, out Vector2Int nextPosition)
        {

            List<Pathfind.Point> points;
            points = Pathfind.Pathfinding.FindPath(gridPathfind, new Pathfind.Point((int)transform.position.x, (int)transform.position.y), new Pathfind.Point((int)(xPos), (int)(yPos)));
            if (points.Count > 0)
            {
                transform.position = (Vector2)points[0].Position;
                nextPosition = points[0].Position;
                return true;
            }
            nextPosition = Vector2Int.zero;
            return false;
        }
        protected bool SmoothMovementPosition(float xPos, float yPos, out Vector2Int nextPosition)
        {
            List<Pathfind.Point> points;
            points = Pathfind.Pathfinding.FindPath(gridPathfind, new Pathfind.Point((int)transform.position.x, (int)transform.position.y), new Pathfind.Point((int)(xPos), (int)(yPos)));
            if (points.Count > 0)
            {
                StartCoroutine(LerpPosition(points[0].Position, gameVariableSO.LerpDuration));
                nextPosition = points[0].Position;
                return true;
            }
            nextPosition = Vector2Int.zero;
            return false;
        }
        protected IEnumerator LerpPosition(Vector2 targetPosition, float duration)
        {
            _isMoving = true;
            float time = 0;
            Vector2 startPosition = transform.position;
            while (time < duration)
            {
                transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            transform.position = targetPosition;
            _isMoving = false;
        }
        /// <summary>
        /// Intenta mover al objeto a la posici�n dada
        /// </summary>
        /// <param name="xPos">Posici�n X en el grid</param>
        /// <param name="yPos">Posici�n Y en el grid</param>
        /// <returns></returns>
        protected virtual bool AttemptMovement(int xPos, int yPos)
        {
            if (!gameVariableSO.SmoothGameplay)
            {
                Vector2Int lastPosition = Vector2Int.FloorToInt(transform.position);
                Vector2Int nextPosition;
                if (TeleportMovementPosition(xPos, yPos, out nextPosition))
                {
                    OnCanMove(lastPosition, nextPosition);
                    return true;
                }
                else
                {
                    OnCantMove();
                    return false;
                }
            }
            else if (!_isMoving)
            {
                Vector2Int lastPosition = Vector2Int.FloorToInt(transform.position);
                Vector2Int nextPosition;
                if (SmoothMovementPosition(xPos, yPos, out nextPosition))
                {
                    OnCanMove(lastPosition, nextPosition);
                    return true;
                }
                else
                {
                    OnCantMove();
                    return false;
                }
            }
            else
            {
                OnAlreadyMoving();
                return true;
            }
        }
        /// <summary>
        /// Intenta mover al objeto en una direcci�in aleatoria
        /// </summary>
        /// <returns></returns>
        protected bool WanderAround()
        {
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
                if (AttemptMovement(aux.x + Mathf.FloorToInt(transform.position.x), aux.y + Mathf.FloorToInt(transform.position.y)))
                {
                    return true;
                }
                indexOfDirections.Remove(randomNumber);
            }
            return false;
        }
        /// <summary>
        /// M�dulo que se ejecuta si el objeto se puede mover. Actualiza los nodos del grid
        /// </summary>
        /// <param name="lastPosition">Posici�n antes de moverse</param>
        /// <param name="currentPosition">Posici�n despues de moverse</param>
        protected virtual void OnCanMove(Vector2Int lastPosition, Vector2Int currentPosition)
        {
            gridPathfind.ChangeNode(currentPosition.x, currentPosition.y, false);
            gridPathfind.ChangeNode(lastPosition.x, lastPosition.y, true);
        }
        /// <summary>
        /// M�dulo que se ejecuta si el objeto no se puede mover
        /// </summary>
        protected virtual void OnCantMove() { }

        /// <summary>
        /// M�dulo que se ejecuta si el objeto ya se esta moviendo
        /// </summary>
        protected virtual void OnAlreadyMoving() { }
    }
}

