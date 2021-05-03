using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDoungeonGenerator : MonoBehaviour
{
    [SerializeField] protected SimpleTileMapGenerator tileMapGenerator = null;
    [SerializeField] protected Vector2Int startPosition = Vector2Int.zero;
    
    private void Awake() 
    {
        GenerateDungeon();
    }
    public void GenerateDungeon()
    {
        tileMapGenerator.ClearAllTiles();
        RunProceduralGeneration();
    }
    protected abstract void RunProceduralGeneration();
}
