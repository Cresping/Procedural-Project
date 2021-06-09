using System.Collections;
using System.Collections.Generic;
using HeroesGames.ProjectProcedural.Procedural;
using UnityEngine;

public class ExampleTileMap : MonoBehaviour
{
    [SerializeField] private SimpleTileMapGenerator simpleTileMapGenerator; 
    void Start()
    {
        HashSet<Vector2Int> wallpositions = new HashSet<Vector2Int>();
        wallpositions.Add(new Vector2Int(0,0));
        simpleTileMapGenerator.PaintWallTiles(wallpositions);
    }
}
