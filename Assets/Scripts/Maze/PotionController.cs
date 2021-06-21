using System;
using HeroesGames.ProjectProcedural.SO;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HeroesGames.ProjectProcedural.Procedural
{
    public class PotionController : MonoBehaviour
    {
        [SerializeField] private PlayerVariableSO playerVariableSO;
        [SerializeField] private MazeVariableSO mazeVariableSO;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            RecoverHealth();
            GetComponent<TilemapCollider2D>().enabled = false;
            GetComponent<Tilemap>().color = Color.black;
        }

        private void RecoverHealth()
        {
            var hp2Recover = Convert.ToInt32((mazeVariableSO.DungeonLvl * playerVariableSO.PlayerLevel) / (playerVariableSO.PlayerLevel * 0.1));
            playerVariableSO.PlayerHP += hp2Recover;
            Debug.Log("Se han añadido " + hp2Recover + " puntos de salud. Ahora el player tiene un total de " +
                      playerVariableSO.PlayerHP);
            // TODO: La UI está rota ¿verdad?
        }
    }
}
