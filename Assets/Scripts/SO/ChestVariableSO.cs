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
        [SerializeField] private Sprite openChestSprite;
        [SerializeField] private GameObject chestPrefab;

        [Range(1, 5)]
        [SerializeField]
        private int chestRarity;
        
        public GameObject ChestPrefab { get => chestPrefab; set => chestPrefab = value; }
        public Sprite OpenChestSprite { get => openChestSprite; set => openChestSprite = value; }

        public ObjectInventoryVariableSO PickRandomItem()
        {
           return objectContainer.PickRandomItem(chestRarity);
        }

    }
}

