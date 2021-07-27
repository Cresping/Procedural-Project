using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.SO
{
    [CreateAssetMenu(fileName = "NewPlayerInventoryVariable", menuName = "Scriptables/Player/PlayerInventoryVariable")]
    public class PlayerInventoryVariableSO : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private InventoryManagerSO inventoryManagerSO;
        [SerializeField] private MainMenuBusSO mainMenuBusSO;
        [SerializeField] private PlayfabBusDataSO playfabBusSO;
        public Action<bool, ObjectInventoryVariableSO> OnInventoryChange;
        private Dictionary<int, ObjectInventoryVariableSO> _inventory;
        private Dictionary<int, ObjectInventoryVariableSO> _temporalInventory;
        public Dictionary<int, ObjectInventoryVariableSO> Inventory { get => _inventory; set => _inventory = value; }

        public bool AddObjectTemporalInventory(ObjectInventoryVariableSO objectInventory)
        {
            if (!_inventory.ContainsKey(objectInventory.Id) && !_temporalInventory.ContainsKey(objectInventory.Id))
            {
                _temporalInventory.Add(objectInventory.Id, objectInventory);
                //_inventory.Add(objectInventory.Id, objectInventory);
                Debug.Log("Se ha agregado el objeto " + objectInventory.name + " al inventario del jugador");
                OnInventoryChange?.Invoke(true, objectInventory);
                return true;
            }
            else
            {
                OnInventoryChange?.Invoke(false, objectInventory);
                Debug.Log("El jugador ya tiene ese objeto");
            }

            return false;
        }
        public bool AddObjectInventory(ObjectInventoryVariableSO objectInventory)
        {
            if (!_inventory.ContainsKey(objectInventory.Id))
            {
                _inventory.Add(objectInventory.Id, objectInventory);
                return true;
            }
            return false;
        }
        public void UpdateInventory()
        {
            List<string> objectsId = new List<string>();
            foreach(ObjectInventoryVariableSO objectInventory in _temporalInventory.Values)
            { 
                objectsId.Add(objectInventory.Id.ToString());
                _inventory.Add(objectInventory.Id, objectInventory);
            }
            if(objectsId.Count>0)
            {
                Debug.Log("Se va actualizar los objetos del usuario");
                inventoryManagerSO.GrantItemToUserRequest(objectsId);
            }
        }
        private void ResetValues()
        {
            _temporalInventory = new Dictionary<int, ObjectInventoryVariableSO>();
        }
        private void OnEnable()
        {
            _inventory = new Dictionary<int, ObjectInventoryVariableSO>();
            _temporalInventory = new Dictionary<int, ObjectInventoryVariableSO>();
            playfabBusSO.OnSucessUpdateInventory += ResetValues;
        }
        public void OnAfterDeserialize()
        {
            _inventory = new Dictionary<int, ObjectInventoryVariableSO>();
            _temporalInventory = new Dictionary<int, ObjectInventoryVariableSO>();
            playfabBusSO.OnSucessUpdateInventory -= ResetValues;
        }

        public void OnBeforeSerialize()
        {

        }
    }

}
