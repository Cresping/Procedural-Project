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

        private void NextDungeon()
        {
            SceneManager.LoadScene("Dungeon", LoadSceneMode.Single);
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerVariableSO.IsOnEvent = true;
                playerVariableSO.DungeonLevel++;
                NextDungeon();
            }
        }
    }

}
