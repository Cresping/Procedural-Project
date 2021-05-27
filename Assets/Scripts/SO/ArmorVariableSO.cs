using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HeroesGames.ProjectProcedural.SO
{

    [CreateAssetMenu(fileName = nameof(ArmorVariableSO), menuName = "Scriptables/" + nameof(ArmorVariableSO) + "/" + nameof(ArmorVariableSO) + "Variable")]
    public class ArmorVariableSO : ObjectInventoryVariableSO
    {
        [SerializeField] private int armorDefense;
        [SerializeField] private int armorHP;

        public int ArmorHP { get => armorHP; set => armorHP = value; }
        public int ArmorDefense { get => armorDefense; set => armorDefense = value; }


        public override void OnAfterDeserialize() { }
        public override void OnBeforeSerialize() { }
    }
}
