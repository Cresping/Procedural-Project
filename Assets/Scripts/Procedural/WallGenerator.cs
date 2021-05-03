using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateSimpleWalls(HashSet<Vector2Int> floorPositions, SimpleTileMapGenerator tilemapVisualizer)
    {
        var basicWallPositions = FindWallsInDirection(floorPositions, Direction2D.cardinalDirectionList);
        var cornerWallPositions = FindWallsInDirection(floorPositions, Direction2D.diagonalDirectionList);
        tilemapVisualizer.PaintWallTiles(basicWallPositions);
        tilemapVisualizer.PaintWallTiles(cornerWallPositions);
    }
    private static HashSet<Vector2Int> FindWallsInDirection(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        foreach (var position in floorPositions)
        {
            foreach (var direction in directionList)
            {
                var neighbourPosition = position + direction;
                if (floorPositions.Contains(neighbourPosition) == false)
                {
                    wallPositions.Add(neighbourPosition);
                }
            }
        }
        return wallPositions;
    }
}
