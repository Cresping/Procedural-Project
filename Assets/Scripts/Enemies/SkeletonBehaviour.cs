using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBehaviour : EnemyBehaviour
{
    protected override void OnCantMove() { }

    protected override bool Attack()
    {
        int damage = base.enemyVariableSO.EnemyAttack - base.playerVariableSO.PlayerDef;
        if (damage > 0)
        {
            base.playerVariableSO.PlayerHP -= damage;
            //Debug.Log("Soy " + this.gameObject.name + " y he hecho " + (base.enemyVariableSO.EnemyAttack - base.playerVariableSO.PlayerDef) + " de daño al jugador");
            return true;
        }
        return false;

    }

    protected override bool TryMove()
    {
        return base.WanderAround();
    }
}
