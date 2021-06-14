using System;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.Procedural
{
    public abstract class AbstractMazeGenerator : MonoBehaviour
    {
        [SerializeField] protected SimpleTileMapGenerator forestTileGenerator = null;

        private void Start() => GenerateMaze();

        public void GenerateMaze()
        {
            forestTileGenerator.ClearAllTiles();
            RunProceduralGeneration();
        }
    
        protected abstract void RunProceduralGeneration();
    }
}