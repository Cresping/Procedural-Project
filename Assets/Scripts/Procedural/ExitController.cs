using System.Collections;
using System.Collections.Generic;
using HeroesGames.ProjectProcedural.SO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HeroesGames.ProjectProcedural.Procedural
{
    public class ExitController : MonoBehaviour
    {
        [SerializeField] private PlayerVariableSO playerVariableSO;
        [SerializeField] private CombatVariableSO combatVariableSO;
        [SerializeField] private bool exitToDungeon = true;
        private void NextDungeon()
        {
            if (exitToDungeon)
            {
                SceneManager.LoadScene("Dungeon", LoadSceneMode.Single);
            }
            else
            {
                SceneManager.LoadScene("Maze", LoadSceneMode.Single);
            }

        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (!combatVariableSO.IsActive)
                {
                    playerVariableSO.IsOnEvent = true;
                    playerVariableSO.DungeonLevel++;
                    NextDungeon();
                }
            }
        }
    }

}
