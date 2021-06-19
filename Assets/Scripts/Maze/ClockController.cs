using HeroesGames.ProjectProcedural.SO;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HeroesGames.ProjectProcedural.Procedural
{
    public class ClockController : MonoBehaviour
    {
        [SerializeField] private MazeGenerator mazeGenerator;
        [SerializeField] private SimpleTileMapGenerator simpleTileMapGenerator;
        [SerializeField] private TimerVariableSO timeVariableSO;
        [SerializeField] private MazeVariableSO mazeVariableSO;
        
        // TODO: change the time coefficient by a factor that depends on the maze's difficulty
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            simpleTileMapGenerator.PaintFloorTile(mazeGenerator.ClockPosition); // TODO: Why this doesn't work?
            timeVariableSO.AddTime(CalculateTime());
            GetComponent<TilemapCollider2D>().enabled = false;
            GetComponent<Tilemap>().color = Color.black;
        }

        private float CalculateTime()
        {
            float time2Add;
            var initialDistance2Clock = Vector2Int.Distance(mazeGenerator.StartPosition, mazeGenerator.ClockPosition);

            if (initialDistance2Clock < mazeGenerator.Width * 0.5)
                time2Add = initialDistance2Clock + mazeVariableSO.DungeonLvl;
            else
                time2Add = (initialDistance2Clock + mazeVariableSO.DungeonLvl) * 0.8f;
            
            return time2Add;
        }
    }
}