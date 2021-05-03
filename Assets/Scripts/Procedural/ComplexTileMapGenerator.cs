using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ComplexTileMapGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap floorTileMap;
    [SerializeField] private Tilemap wallTileMap;
    [SerializeField] private Tilemap startTileMap;
    [SerializeField] private Tilemap endTileMap;
    [SerializeField] private TileBase floorTile;
    [SerializeField] private TileBase startTile;
    [SerializeField] private TileBase endTile;
    [SerializeField] private TileBase wallTileSlideTop;
    [SerializeField] private TileBase wallTileSideRight;
    [SerializeField] private TileBase wallTileSideLeft;
    [SerializeField] private TileBase wallTileSideBotton;
    [SerializeField] private TileBase wallTileFull;
    [SerializeField] private TileBase wallTileInnerCornerDownLeft;
    [SerializeField] private TileBase wallTileInnerCornerDownRight;
    [SerializeField] private TileBase wallTileInnerCornerUpLeft;
    [SerializeField] private TileBase wallTileInnerCornerUpRight;
    [SerializeField] private TileBase wallTileDiagonalCornerDownRight;
    [SerializeField] private TileBase wallTileDiagonalCornerDownLeft;
    [SerializeField] private TileBase wallTileDiagonalCornerUpRight;
    [SerializeField] private TileBase wallTileDiagonalCornerUpLeft;

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in positions)
        {
            PaintSingleTitle(tilemap, tile, position);
        }
    }
    private void PaintSingleTitle(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }
    public void PaintSingleBasicWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;
        if (WallTypesHelper.wallTop.Contains(typeAsInt))
        {
            tile = wallTileSlideTop;
        }
        else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
        {
            tile = wallTileSideRight;
        }
        else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
        {
            tile = wallTileSideLeft;
        }
        else if (WallTypesHelper.wallBottm.Contains(typeAsInt))
        {
            tile = wallTileSideBotton;
        }
        else if (WallTypesHelper.wallFull.Contains(typeAsInt))
        {
            tile = wallTileFull;
        }
        if (tile != null)
        {
            PaintSingleTitle(wallTileMap, tile, position);
        }
    }
    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTileMap, floorTile);
    }
    public void PaintStartTile(Vector2Int position)
    {
        PaintSingleTitle(startTileMap, startTile, position);
    }
    public void PaintEndTile(Vector2Int position)
    {
        PaintSingleTitle(endTileMap, endTile, position);
    }
    public void ClearAllTiles()
    {
        floorTileMap.ClearAllTiles();
        wallTileMap.ClearAllTiles();
        startTileMap.ClearAllTiles();
        endTileMap.ClearAllTiles();
    }

    public void PaintSingleCornerWall(Vector2Int position, string binaryType)
    {
        int typeASInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;
        if (WallTypesHelper.wallInnerCornerDownLeft.Contains(typeASInt))
        {
            tile = wallTileInnerCornerDownLeft;
        }
        else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeASInt))
        {
            tile = wallTileInnerCornerDownRight;
        }
        else if (WallTypesHelper.wallInnerCornerUpRight.Contains(typeASInt))
        {
            tile = wallTileInnerCornerUpRight;
        }
        else if (WallTypesHelper.wallInnerCornerUpLeft.Contains(typeASInt))
        {
            tile = wallTileInnerCornerUpLeft;
        }
        else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeASInt))
        {
            tile = wallTileDiagonalCornerDownLeft;
        }
        else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeASInt))
        {
            tile = wallTileDiagonalCornerDownRight;
        }
        else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeASInt))
        {
            tile = wallTileDiagonalCornerUpRight;
        }
        else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeASInt))
        {
            tile = wallTileDiagonalCornerUpLeft;
        }
        else if (WallTypesHelper.wallFullEightDirections.Contains(typeASInt))
        {
            tile = wallTileFull;
        }

        if (tile != null)
        {
            PaintSingleTitle(wallTileMap, tile, position);
        }

    }
}
