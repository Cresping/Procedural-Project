using System;
using HeroesGames.ProjectProcedural.SO;
using HeroesGames.ProjectProcedural.UI;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HeroesGames.ProjectProcedural.Procedural
{
    public class ClockController : MonoBehaviour
    {
        [SerializeField] private MazeGenerator mazeGenerator;
        [SerializeField] private MazeVariableSO mazeVariableSO;
        [SerializeField] private SimpleTileMapGenerator simpleTileMapGenerator;
        [SerializeField] private TimerVariableSO timeVariableSO;
        [SerializeField] private PlayerVariableSO playerVariableSO;
        [SerializeField] private UIDungeonController uiController;
        [SerializeField, Range(1,2)] private float messageDuration;
        [SerializeField] private SoundController soundController;
        [SerializeField] private AudioClip clockClip;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            var time = CalculateTime();
            timeVariableSO.AddTime(time);
            soundController.PlaySound(clockClip);
            uiController.ShowMessages("¡Tiempo extra!",messageDuration);
            GetComponent<TilemapCollider2D>().enabled = false;
            GetComponent<Tilemap>().color = Color.black;
        }

        private float CalculateTime()
        {
            float time2Add;
            var initialDistance2Clock = Vector2Int.Distance(mazeGenerator.StartPosition, mazeGenerator.ClockPosition);

            if (initialDistance2Clock < mazeGenerator.Width * 0.5)
                time2Add = initialDistance2Clock +
                           Convert.ToInt32(0.8f * (mazeVariableSO.DungeonLvl * playerVariableSO.PlayerLevel) /
                               (playerVariableSO.PlayerLevel * 0.1));
            else
                time2Add = initialDistance2Clock +
                            Convert.ToInt32(0.5f * (mazeVariableSO.DungeonLvl * playerVariableSO.PlayerLevel) /
                                (playerVariableSO.PlayerLevel * 0.1));

            return time2Add;
        }
    }
}