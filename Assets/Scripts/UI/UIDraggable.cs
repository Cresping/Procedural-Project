using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using HeroesGames.ProjectProcedural.SO;
namespace HeroesGames.ProjectProcedural.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private ObjectInventoryVariableSO objectInventoryVariableSO;
        [SerializeField] private MainMenuBusSO mainMenuBusSO;
        [SerializeField] private Transform canvas;
        private Transform _parentToReturn;
        private CanvasGroup _canvasGroup;
        private bool _isEquiped;

        public Transform ParentToReturn { get => _parentToReturn; set => _parentToReturn = value; }
        public ObjectInventoryVariableSO ObjectInventoryVariableSO { get => objectInventoryVariableSO; set => objectInventoryVariableSO = value; }
        public bool IsEquiped { get => _isEquiped; set => _isEquiped = value; }

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        public void OnBeginDrag(PointerEventData eventData)
        {

            _parentToReturn = this.transform.parent;
            this.transform.SetParent(canvas);
            _canvasGroup.blocksRaycasts = false;
            //Vector2 pos = Camera.main.ScreenToViewportPoint(Mouse.current.position.ReadValue());
            //transform.position = pos;
        }

        public void OnDrag(PointerEventData eventData)
        {
            this.transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            this.transform.SetParent(_parentToReturn);
            _canvasGroup.blocksRaycasts = true;
        }
    }
}
