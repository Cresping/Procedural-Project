using HeroesGames.ProjectProcedural.UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace HeroesGames.ProjectProcedural.Procedural
{
    public class KeyController : MonoBehaviour
    {
        [SerializeField] private MazeGenerator mazeGenerator;
        [SerializeField] private SimpleTileMapGenerator simpleTileMapGenerator;
        [SerializeField] private UIDungeonController uiController;
        [SerializeField, Range(1,2)] private float messageDuration;
        [SerializeField] private SoundController soundController;
        [SerializeField] private AudioClip doorClip;
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            soundController.PlaySound(doorClip);
            uiController.ShowMessages("¡Tienes la llave!\nYa puedes escapar",messageDuration);
            simpleTileMapGenerator.PaintOpenEndTile(mazeGenerator.ExitPosition);
            GetComponent<Tilemap>().color = Color.black;
        }
    }
}