using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.SO
{
    [CreateAssetMenu(fileName = "NewPlayerInventoryVariable", menuName = "Scriptables/Player/PlayerInventoryVariable")]
    public class PlayerInventoryVariableSO : ScriptableObject, ISerializationCallbackReceiver
    {
        public Action<ObjectInventoryVariableSO> OnInventoryChange;
        [SerializeField] private int maxCapacity = 10;
        private List<ObjectInventoryVariableSO> _objectInventory;

        public List<ObjectInventoryVariableSO> ObjectInventory { get => _objectInventory; set => _objectInventory = value; }

        public bool AddObjectInventory(ObjectInventoryVariableSO objectInventory)
        {
            if (_objectInventory.Count < maxCapacity)
            {
                _objectInventory.Add(objectInventory);
                Debug.Log("Se ha agregado el objeto "+objectInventory.name+" al inventario del jugador");
                OnInventoryChange?.Invoke(objectInventory);
                return true;
            }
            Debug.Log("La bolsa del jugador esta llena");
            return false;
        }
        public bool RemoveObjectInventory(int index)
        {
            if (index < 0 || index > _objectInventory.Count) return false;
            _objectInventory.RemoveAt(index);
            return true;
        }
        public void OnAfterDeserialize()
        {
            _objectInventory = new List<ObjectInventoryVariableSO>();
        }

        public void OnBeforeSerialize()
        {

        }
    }

}
