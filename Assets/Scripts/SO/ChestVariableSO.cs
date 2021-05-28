using System.Collections;
using System.Collections.Generic;
using HeroesGames.ProjectProcedural.SO;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.SO
{
    [CreateAssetMenu(fileName = "NewChestVariable", menuName = "Scriptables/Chest/ChestVariableSO")]
    public class ChestVariableSO : ScriptableObject
    {

        [SerializeField] ObjectContainerVariableSO objectContainer;
        [SerializeField] private GameObject chestPrefab;

        [Range(1, 5)]
        [SerializeField]
        private int chestRarity;
        private ObjectInventoryVariableSO _objectChest;

        public ObjectInventoryVariableSO ObjectChest { get => _objectChest; set => _objectChest = value; }
        public GameObject ChestPrefab { get => chestPrefab; set => chestPrefab = value; }

        private void PickRandomItem()
        {
            _objectChest = objectContainer.PickRandomItem(chestRarity);
        }

    }
}

