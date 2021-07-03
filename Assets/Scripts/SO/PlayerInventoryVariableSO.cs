using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.SO
{
    [CreateAssetMenu(fileName = "NewPlayerInventoryVariable", menuName = "Scriptables/Player/PlayerInventoryVariable")]
    public class PlayerInventoryVariableSO : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private MainMenuBusSO mainMenuBusSO;
        public Action<bool, ObjectInventoryVariableSO> OnInventoryChange;
        private Dictionary<int, ObjectInventoryVariableSO> _inventory;

        public Dictionary<int, ObjectInventoryVariableSO> Inventory { get => _inventory; set => _inventory = value; }

        public bool AddObjectInventory(ObjectInventoryVariableSO objectInventory)
        {
            if (!_inventory.ContainsKey(objectInventory.Id))
            {
                _inventory.Add(objectInventory.Id, objectInventory);
                Debug.Log("Se ha agregado el objeto " + objectInventory.name + " al inventario del jugador");
                OnInventoryChange?.Invoke(true, objectInventory);
                return true;
            }
            else
            {
                OnInventoryChange?.Invoke(false, objectInventory);
                Debug.Log("El jugador ya tiene se objeto");
            }

            return false;
        }
        public void OnAfterDeserialize()
        {
            _inventory = new Dictionary<int, ObjectInventoryVariableSO>();
        }

        public void OnBeforeSerialize()
        {

        }
    }

}
