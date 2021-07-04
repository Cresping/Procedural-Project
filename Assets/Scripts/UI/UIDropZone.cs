using System.Collections;
using System.Collections.Generic;
using HeroesGames.ProjectProcedural.SO;
using UnityEngine;
using UnityEngine.EventSystems;
namespace HeroesGames.ProjectProcedural.UI
{
    public class UIDropZone : MonoBehaviour, IDropHandler
    {
        [SerializeField] private MainMenuBusSO mainMenuBusSO;
        [SerializeField] private Transform dropParent;
        [SerializeField] private bool isStorage;

        [Range(0, 3)]
        [SerializeField] private int positionEquipment;
        private ObjectInventoryVariableSO _currentObjectInventoryVariableSO;
        public int PositionEquipment { get => positionEquipment; set => positionEquipment = value; }
        public bool IsTaken { get => _isTaken; set => _isTaken = value; }
        public ObjectInventoryVariableSO CurrentObjectInventoryVariableSO { get => _currentObjectInventoryVariableSO; set => _currentObjectInventoryVariableSO = value; }

        private bool _isTaken = false;
        private void OnEnable()
        {
            if (!isStorage)
            {
                mainMenuBusSO.OnEnableSlot += EnableSlot;
            }
        }
        private void OnDisable()
        {
            if (!isStorage)
            {
                mainMenuBusSO.OnEnableSlot -= EnableSlot;
            }
        }
        public void OnDrop(PointerEventData eventData)
        {
            UIDraggable draggable = eventData.pointerDrag.GetComponent<UIDraggable>();
            if (draggable)
            {
                if (!isStorage)
                {
                    _currentObjectInventoryVariableSO = draggable.ObjectInventoryVariableSO;
                    if (!_currentObjectInventoryVariableSO.IsEquiped && !_isTaken)
                    {
                        _currentObjectInventoryVariableSO.PlayerPositionEquipment = positionEquipment;
                        mainMenuBusSO.OnDropItem?.Invoke(_currentObjectInventoryVariableSO);
                        mainMenuBusSO.OnEquipItemEvent?.Invoke(_currentObjectInventoryVariableSO);
                        draggable.ParentToReturn = dropParent;
                        _isTaken = true;
                    }
                }
                else
                {
                    _currentObjectInventoryVariableSO = draggable.ObjectInventoryVariableSO;
                    mainMenuBusSO.OnDropItem?.Invoke(_currentObjectInventoryVariableSO);
                    mainMenuBusSO.OnEnableSlot?.Invoke(_currentObjectInventoryVariableSO.PlayerPositionEquipment);
                    mainMenuBusSO.OnUnequipItemEvent?.Invoke(_currentObjectInventoryVariableSO);
                    draggable.ParentToReturn = dropParent;
                }
            }
        }
        private void EnableSlot(int slot)
        {
            if (!isStorage && _currentObjectInventoryVariableSO)
            {
                if (slot == positionEquipment)
                {
                    _isTaken = false;
                }
            }
        }
    }

}
