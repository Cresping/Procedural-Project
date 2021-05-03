using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SimpleTileMapGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap floorTileMap;
    [SerializeField] private Tilemap wallTileMap;
    [SerializeField] private Tilemap startTileMap;
    [SerializeField] private Tilemap endTileMap;
    [SerializeField] private TileVariableSO floorTiles;
    [SerializeField] private TileVariableSO wallTiles;
    [SerializeField] private TileVariableSO startTiles;
    [SerializeField] private TileVariableSO endTiles;

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
    public void PaintWallTiles(IEnumerable<Vector2Int> wallPositions)
    {
        foreach (var position in wallPositions)
        {
            PaintSingleTitle(wallTileMap, wallTiles.PickRandomTile(), position);
        }
    }
    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        foreach (var position in floorPositions)
        {
            PaintSingleTitle(floorTileMap, floorTiles.PickRandomTile(), position);
        }
    }
    public void PaintStartTile(Vector2Int position)
    {
        PaintSingleTitle(startTileMap, startTiles.PickRandomTile(), position);
    }
    public void PaintEndTile(Vector2Int position)
    {
        PaintSingleTitle(endTileMap, endTiles.PickRandomTile(), position);
    }

    public void ClearAllTiles()
    {
        floorTileMap.ClearAllTiles();
        wallTileMap.ClearAllTiles();
        startTileMap.ClearAllTiles();
        endTileMap.ClearAllTiles();
    }
}
