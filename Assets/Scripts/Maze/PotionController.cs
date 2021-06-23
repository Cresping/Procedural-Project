using System;
using HeroesGames.ProjectProcedural.SO;
using HeroesGames.ProjectProcedural.UI;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HeroesGames.ProjectProcedural.Procedural
{
    public class PotionController : MonoBehaviour
    {
        [SerializeField] private PlayerVariableSO playerVariableSO;
        [SerializeField] private MazeVariableSO mazeVariableSO;
        [SerializeField] private UIDungeonController uiController;
        [SerializeField, Range(1,2)] private float messageDuration;
        [SerializeField] private SoundController soundController;
        [SerializeField] private AudioClip potionClip;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            RecoverHealth();
            soundController.PlaySound(potionClip);
            uiController.ShowMessages("You've found a health potion!",messageDuration);
            GetComponent<TilemapCollider2D>().enabled = false;
            GetComponent<Tilemap>().color = Color.black;
        }

        private void RecoverHealth()
        {
            var hp2Recover = Convert.ToInt32((mazeVariableSO.DungeonLvl * playerVariableSO.PlayerLevel) / (playerVariableSO.PlayerLevel * 0.1));
            playerVariableSO.RuntimePlayerHP += hp2Recover;
            Debug.Log(hp2Recover + "health points regained" + "Total HP Points = " +
                      playerVariableSO.RuntimePlayerHP);
        }
    }
}
