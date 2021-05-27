using System.Collections;
using System.Collections.Generic;
using HeroesGames.ProjectProcedural.SO;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.SO
{
    public class ChestVariableSO : ScriptableObject, ISerializationCallbackReceiver
    {

        [SerializeField] private GameObject chestPrefab;

        [Range(1, 5)]
        [SerializeField]
        private int chestRarity;
        private ObjectInventoryVariableSO _objectChest;



        private void PickRandomItem()
        {

        }
        public void OnAfterDeserialize() { }

        public void OnBeforeSerialize() { }

    }
}

