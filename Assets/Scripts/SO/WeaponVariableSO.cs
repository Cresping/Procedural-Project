using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HeroesGames.ProjectProcedural.SO
{

    [CreateAssetMenu(fileName = "WeaponVariableSO", menuName = "Scriptables/Inventory/WeaponVariableSO")]
    public class WeaponVariableSO : ObjectInventoryVariableSO
    {
        [SerializeField] private int weaponAttack;
        [SerializeField] private int weaponSpeed;

        public int WeaponAttack { get => weaponAttack; set => weaponAttack = value; }
        public int WeaponSpeed { get => weaponSpeed; set => weaponSpeed = value; }

        public override void OnAfterDeserialize() { }
        public override void OnBeforeSerialize() { }
    }
}

