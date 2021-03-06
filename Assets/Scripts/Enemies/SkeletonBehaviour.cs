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
            if (!CanAttackPlayer())
            {
                if (CanSeePlayer())
                {
                    if (Pursue())
                    {
                        return;
                    }
                }
                Idle();
            }
        }

        /// <summary>
        /// Implementación del Ataque del esqueleto
        /// </summary>
        /// <returns>True si ha conseguido hacer daño, false si no</returns>
        protected override bool Attack()
        {
            if (!playerVariableSO.IsOnEvent)
            {
                combatVariableSO.AddEnemy(this);
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
