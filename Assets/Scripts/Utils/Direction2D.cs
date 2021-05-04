using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.Utils
{
    /// <summary>
    /// Clase encargada de almacenar las posibles direcciones que hay en un mundo en 2D 
    /// </summary>
    public static class Direction2D
    {
        public static List<Vector2Int> cardinalDirectionList = new List<Vector2Int>
    {
        new Vector2Int(0,1), //UP
        new Vector2Int(1,0), //RIGHT
        new Vector2Int(0,-1), //DOWN
        new Vector2Int(-1,0) //LEFT
    };
        public static List<Vector2Int> diagonalDirectionList = new List<Vector2Int>
    {
        new Vector2Int(1,1), //UP-RIGHT
        new Vector2Int(1,-1), //RIGHT-DOWN
        new Vector2Int(-1,-1), //DOWN-LEFT
        new Vector2Int(-1,1) //LEFT-UP
    };
        public static List<Vector2Int> eighDirectionList = new List<Vector2Int>
    {
        new Vector2Int(0,1), //UP
        new Vector2Int(1,1), //UP-RIGHT
        new Vector2Int(1,0), //RIGHT
        new Vector2Int(1,-1), //RIGHT-DOWN
        new Vector2Int(0,-1), //DOWN
        new Vector2Int(-1,-1), //DOWN-LEFT
        new Vector2Int(-1,0), //LEFT
        new Vector2Int(-1,1) //LEFT-UP
    };
        /// <summary>
        /// Devuelve una direcci�n cardinal aleatoria
        /// </summary>
        /// <returns>Direcci�n aleatoria</returns>
        public static Vector2Int GetRandomCardinalDirection()
        {
            return cardinalDirectionList[Random.Range(0, cardinalDirectionList.Count)];
        }

        /// <summary>
        /// Devuelve la direcci�n que hay que seguir para llegar a un punto determinado
        /// </summary>
        /// <param name="currentPosition">Posici�n actual</param>
        /// <param name="nextPosition">Posici�n donde hay que llegar</param>
        /// <returns>Direcci�n hac�a la posici�n</returns>
        public static Vector2 GetFollowDirection(Vector2 currentPosition, Vector2 nextPosition)
        {
            return (nextPosition - currentPosition).normalized;
        }

        /// <summary>
        /// Devuelve la direcci�n que hay que huir de un punto determinado
        /// </summary>
        /// <param name="currentPosition">Posici�n actual</param>
        /// <param name="nextPosition">Posici�n de donde debes huir</param>
        /// <returns>Direcci�n de huida de la posici�n</returns>
        public static Vector2 GetFleeDirection(Vector2 currentPosition, Vector2 nextPosition)
        {
            return (currentPosition - nextPosition).normalized;
        }
    }

}
