using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using HeroesGames.ProjectProcedural.SO;
using UnityEngine.UI;

namespace HeroesGames.ProjectProcedural.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Image))]
    public class UIDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private ObjectInventoryVariableSO objectInventoryVariableSO;
        [SerializeField] private MainMenuBusSO mainMenuBusSO;
        [SerializeField] private Transform dragZone;
        private Transform _parentToReturn;
        private CanvasGroup _canvasGroup;
        private Image _imageObject;

        public Transform ParentToReturn { get => _parentToReturn; set => _parentToReturn = value; }
        public ObjectInventoryVariableSO ObjectInventoryVariableSO { get => objectInventoryVariableSO; set => objectInventoryVariableSO = value; }

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _imageObject = GetComponent<Image>();
            _imageObject.sprite = objectInventoryVariableSO.ObjectSprite;

        }
        private void OnEnable()
        {
            dragZone = GameObject.FindGameObjectWithTag("DragZone").transform;
        }
        public void OnBeginDrag(PointerEventData eventData)
        {

            _parentToReturn = this.transform.parent;
            this.transform.SetParent(dragZone);
            _canvasGroup.blocksRaycasts = false;
            mainMenuBusSO.OnDragItem?.Invoke(objectInventoryVariableSO);
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
