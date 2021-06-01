using System.Collections;
using System.Collections.Generic;
using HeroesGames.ProjectProcedural.SO;
using HeroesGames.ProjectProcedural.Utils;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.Inventory
{
    [RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Collider2D))]
    public class ChestController : MonoBehaviour
    {
        [SerializeField] private PlayerVariableSO playerVariableSO;
        [SerializeField] private PlayerInventoryVariableSO playerInventoryVariableSO;
        [SerializeField] private ChestVariableSO chestVariableSO;
        private ObjectInventoryVariableSO _chestObject;
        private SpriteRenderer _chestSpriteRenderer;
        private void Awake()
        {
            _chestSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (_chestSpriteRenderer)
            {
                _chestObject = chestVariableSO.PickRandomItem();
                if (_chestObject)
                {
                    playerVariableSO.PlayerPositionOnValueChange += OpenChest;
                }
                else
                {
                    _chestSpriteRenderer.sprite = chestVariableSO.OpenChestSprite;
                    Debug.LogError("No hay objetos disponibles para el cofre " + gameObject.name);
                }
            }
            else
            {
                Debug.LogError("El cofre " + gameObject.name + " no tiene SpriteRenderer");
            }
        }
        public bool TryGetItem()
        {

            if (playerInventoryVariableSO.AddObjectInventory(_chestObject))
            {
                _chestSpriteRenderer.sprite = chestVariableSO.OpenChestSprite;
                _chestObject = null;
                return true;
            }
            return false;
        }
        private void OpenChest()
        {

            foreach (var direction in Direction2D.cardinalDirectionList)
            {
                if (playerVariableSO.PlayerPosition == new Vector2(transform.position.x + direction.x, transform.position.y + direction.y))
                {
                    if (TryGetItem())
                    {
                        playerVariableSO.PlayerPositionOnValueChange -= OpenChest;
                    }
                    return;
                }
            }
        }

    }
}
