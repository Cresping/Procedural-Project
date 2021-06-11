using UnityEngine;

namespace HeroesGames.ProjectProcedural.Procedural
{
    public abstract class AbstractMazeGenerator : MonoBehaviour
    {
        [SerializeField] protected SimpleTileMapGenerator forestMapGenerator = null;
    
        private void Awake() => GenerateMaze();

        private void GenerateMaze()
        {
            forestMapGenerator.ClearAllTiles();
            RunProceduralGeneration();
        }
    
        protected abstract void RunProceduralGeneration();
    }
}