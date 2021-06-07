using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HeroesGames.ProjectProcedural.Enemies
{
    public class MimicBehaviour : EnemyBehaviour
    {
        /// <summary>
        /// Acciones que debe de realizar el esqueleto cuando es su turno
        /// </summary>
        protected override void DoSomething()
        {
            if (CanAttackPlayer())
            {
                if (Attack())
                {
                    StartCoroutine(coroutineCombat());
                }

            }
        }

        /// <summary>
        /// Implementación del Ataque del esqueleto
        /// </summary>
        /// <returns>True si ha conseguido hacer daño, false si no</returns>
        protected override bool Attack()
        {
            combatVariableSO.AddEnemy(this.gameObject.GetComponent<EnemyBehaviour>());
            return true;
        }

        protected override bool Idle()
        {
            //DoNothing
            return true;
        }
    }

}
