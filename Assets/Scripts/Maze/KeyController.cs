using UnityEngine;
using UnityEngine.Tilemaps;

namespace HeroesGames.ProjectProcedural.Procedural
{
    public class KeyController : MonoBehaviour
    {
        [SerializeField] private MazeGenerator mazeGenerator;
        [SerializeField] private SimpleTileMapGenerator simpleTileMapGenerator;
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            simpleTileMapGenerator.PaintOpenEndTile(mazeGenerator.ExitPosition);
            simpleTileMapGenerator.PaintFloorTile(mazeGenerator.KeyPosition); // TODO: Why this doesn't work?
            GetComponent<Tilemap>().color = Color.black; // It's a little dirty...?
        }
    }
}