using System.Collections;
using System.Collections.Generic;
using HeroesGames.ProjectProcedural.SO;
using UnityEngine;
using UnityEngine.EventSystems;
namespace HeroesGames.ProjectProcedural.UI
{
    public class UIDropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private MainMenuBusSO mainMenuBusSO;
        [SerializeField] private Transform dropParent;
        [SerializeField] private bool isStorage;

        [Range(0, 3)]
        [SerializeField] private int positionEquipment;
        private ObjectInventoryVariableSO _currentObjectInventoryVariableSO;

        public int PositionEquipment { get => positionEquipment; set => positionEquipment = value; }

        public void OnDrop(PointerEventData eventData)
        {
            UIDraggable draggable = eventData.pointerDrag.GetComponent<UIDraggable>();
            if (draggable)
            {

                if (!isStorage)
                {
                    _currentObjectInventoryVariableSO = draggable.ObjectInventoryVariableSO;
                    if (!draggable.IsEquiped)
                    {
                        mainMenuBusSO.OnUnequipItemEvent?.Invoke(_currentObjectInventoryVariableSO);
                        draggable.IsEquiped = true;
                        _currentObjectInventoryVariableSO.PlayerPositionEquipment = positionEquipment;
                        mainMenuBusSO.OnEquipItemEvent?.Invoke(_currentObjectInventoryVariableSO);
                        draggable.ParentToReturn = dropParent;
                    }
                }
                else
                {
                    _currentObjectInventoryVariableSO = draggable.ObjectInventoryVariableSO;
                    draggable.IsEquiped = false;
                    mainMenuBusSO.OnUnequipItemEvent?.Invoke(_currentObjectInventoryVariableSO);
                    draggable.ParentToReturn = dropParent;
                }
            }

        }

        public void OnPointerEnter(PointerEventData eventData)
        {

        }

        public void OnPointerExit(PointerEventData eventData)
        {

        }
    }

}
