using System.Collections;
using System.Collections.Generic;
using HeroesGames.ProjectProcedural.Enemies;
using HeroesGames.ProjectProcedural.SO;
using HeroesGames.ProjectProcedural.UI;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.Player
{
    public class PlayerCombatController : MonoBehaviour
    {
        [SerializeField] private GameVariableSO gameVariableSO;
        [SerializeField] private CombatVariableSO combatVariableSO;
        [SerializeField] private UIDungeonController uIDungeonController;

        [SerializeField] private PlayerVariableSO playerVariableSO;


        public void DoDamageEnemy()
        {
            if (combatVariableSO.IsActive)
            {
                combatVariableSO.DoDamageCurrentEnemy(playerVariableSO.PlayerDamage);             
            }
        }
    }
}

