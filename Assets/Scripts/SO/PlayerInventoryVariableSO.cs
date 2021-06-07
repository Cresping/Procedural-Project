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
        private List<ObjectInventoryVariableSO> _objectInventoryVariableSOs;

        public bool AddObjectInventory(ObjectInventoryVariableSO objectInventory)
        {
            if (_objectInventoryVariableSOs.Count < maxCapacity)
            {
                _objectInventoryVariableSOs.Add(objectInventory);
                Debug.Log("Se ha agregado el objeto "+objectInventory.name+" al inventario del jugador");
                OnInventoryChange?.Invoke(objectInventory);
                return true;
            }
            Debug.Log("La bolsa del jugador esta llena");
            return false;
        }
        public bool RemoveObjectInventory(int index)
        {
            if (index < 0 || index > _objectInventoryVariableSOs.Count) return false;
            _objectInventoryVariableSOs.RemoveAt(index);
            return true;
        }
        public void OnAfterDeserialize()
        {
            _objectInventoryVariableSOs = new List<ObjectInventoryVariableSO>();
        }

        public void OnBeforeSerialize()
        {

        }
    }

}
