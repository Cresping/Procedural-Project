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
    public static void CreateComplexWalls(HashSet<Vector2Int> floorPositions, ComplexTileMapGenerator tilemapVisualizer)
    {
        var basicWallPositions = FindWallsInDirection(floorPositions, Direction2D.cardinalDirectionList);
        var cornerWallPositions = FindWallsInDirection(floorPositions, Direction2D.diagonalDirectionList);
        CreateBasicWall(tilemapVisualizer, basicWallPositions, floorPositions);
        CreateCornerWalls(tilemapVisualizer, cornerWallPositions, floorPositions);
    }
    private static void CreateCornerWalls(ComplexTileMapGenerator tilemapVisualizer, HashSet<Vector2Int> cornerWallPositions, HashSet<Vector2Int> floorPositions)
    {
        foreach (var position in cornerWallPositions)
        {
            string neighboursBinaryType = "";
            foreach (var direction in Direction2D.eighDirectionList)
            {
                var neighbourPosition = position + direction;
                if (floorPositions.Contains(neighbourPosition))
                {
                    neighboursBinaryType += "1";
                }
                else
                {
                    neighboursBinaryType += "0";
                }
            }
            tilemapVisualizer.PaintSingleCornerWall(position, neighboursBinaryType);
        }

    }

    private static void CreateBasicWall(ComplexTileMapGenerator tilemapVisualizer, HashSet<Vector2Int> basicWallPositions, HashSet<Vector2Int> floorPositions)
    {
        foreach (var position in basicWallPositions)
        {
            string neigboursBinaryType = "";
            foreach (var direction in Direction2D.cardinalDirectionList)
            {
                var neigboursPosition = position + direction;
                if (floorPositions.Contains(neigboursPosition))
                {
                    neigboursBinaryType += "1";
                }
                else
                {
                    neigboursBinaryType += "0";
                }
            }
            tilemapVisualizer.PaintSingleBasicWall(position, neigboursBinaryType);
        }
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
