using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.Utils
{
    /// <summary>
    /// Clase encargada de controlar el movimiento de los objetos en el grid
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Collider2D))]
    public abstract class MovingObject : MonoBehaviour
    {
        [SerializeField] protected Pathfind.GridPathfind gridPathfind;

        protected virtual void Awake()
        {
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
        protected bool MoveTeleportMovementPosition(float xPos, float yPos)
        {
            List<Pathfind.Point> points;
            points = Pathfind.Pathfinding.FindPath(gridPathfind, new Pathfind.Point((int)transform.position.x, (int)transform.position.y), new Pathfind.Point((int)(xPos), (int)(yPos)));
            if (points.Count > 0)
            {
                transform.position = (Vector2) points[0].Position;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Intenta mover al objeto a la posici�n dada
        /// </summary>
        /// <param name="xPos">Posici�n X en el grid</param>
        /// <param name="yPos">Posici�n Y en el grid</param>
        /// <returns></returns>
        protected virtual bool AttemptTeleportMovement(int xPos, int yPos)
        {
            Vector2Int lastPosition = Vector2Int.FloorToInt(transform.position);
            if (MoveTeleportMovementPosition(xPos, yPos))
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
                if (MoveTeleportMovementPosition(aux.x + Mathf.FloorToInt(transform.position.x), aux.y + Mathf.FloorToInt(transform.position.y)))
                {
                    OnCanMove(lastPosition, Vector2Int.FloorToInt(transform.position));
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
    }
}

