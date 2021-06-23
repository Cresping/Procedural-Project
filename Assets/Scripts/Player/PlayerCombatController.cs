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


        private void Awake()
        {
            if (!uIDungeonController)
            {
                this.uIDungeonController = FindObjectOfType<UIDungeonController>();
            }
        }
        public void DoAttackEnemy()
        {
            if (combatVariableSO.IsActive)
            {
                combatVariableSO.DoDamageCurrentEnemy(playerVariableSO.RuntimePlayerAtk);
            }
        }
        public void DoStrongAttackEnemy()
        {
            if (combatVariableSO.IsActive)
            {
                combatVariableSO.DoStrongDamageCurrentEnemy(playerVariableSO.RuntimePlayerAtk * 2);
            }
        }
    }
}

