using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public static Vector2Int GetRandomCardinalDirection()
    {
        return cardinalDirectionList[Random.Range(0, cardinalDirectionList.Count)];
    }
    public static Vector2 GetDirection(Vector2 currentPosition, Vector2 nextPosition)
    {
        return (nextPosition - currentPosition).normalized;
    }
    public static Vector2 GetFleeDirection(Vector2 currentPosition, Vector2 nextPosition)
    {
        return (currentPosition - nextPosition).normalized;
    }
}
