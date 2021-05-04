using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HeroesGames.ProjectProcedural.Enemies
{
    /// <summary>
    /// Clase encargada de controlar la IA de los esqueletos
    /// </summary>
    public class SkeletonBehaviour : EnemyBehaviour
    {
        /// <summary>
        /// Acciones que debe de realizar el esqueleto cuando es su turno
        /// </summary>
        protected override void DoSomething()
        {
            if (CanAttackPlayer())
            {
                Attack();
                return;
            }
            else if (CanSeePlayer())
            {
                if (Pursue())
                {
                    return;
                }
            }
            Idle();
        }

        /// <summary>
        /// Implementación del Ataque del esqueleto
        /// </summary>
        /// <returns>True si ha conseguido hacer daño, false si no</returns>
        protected override bool Attack()
        {
            int damage = base.enemyVariableSO.EnemyAttack - base.playerVariableSO.PlayerDef;
            if (damage > 0)
            {
                base.playerVariableSO.PlayerHP -= damage;
                Debug.Log("Soy " + this.gameObject.name + " y he hecho " + (base.enemyVariableSO.EnemyAttack - base.playerVariableSO.PlayerDef) + " de daño al jugador");
                return true;
            }
            return false;

        }

        /// <summary>
        /// Implementación del Idle del esqueleto
        /// </summary>
        /// <returns></returns>
        protected override bool Idle()
        {
            return base.WanderAround();
        }

    }

}
