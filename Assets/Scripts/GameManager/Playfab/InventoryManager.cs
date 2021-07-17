using System.Collections;
using System.Collections.Generic;
using HeroesGames.ProjectProcedural.SO;
using PlayFab.ClientModels;
using UnityEngine;

namespace Playfab
{
    public class InventoryManager : MonoBehaviour
    {
         [ContextMenu("ShowPlayerInventory")]
        public void ShowPlayerInventory()
        {
            Debug.Log("Se va a mostrar el inventario del jugador");
            PlayfabManager.Instance.GetInventory(
            (inventory) =>
            {
                foreach(ItemInstance item in inventory.Inventory )
                {
                    Debug.Log("Player owns "+item.ItemId);
                }
            }
            );
        }
        public void AddItemInventory()
        {
            
        }
    }
}
