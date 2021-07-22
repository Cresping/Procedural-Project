using System;
using System.Collections;
using System.Collections.Generic;
using HeroesGames.ProjectProcedural.SO;
using PlayFab.ClientModels;
using UnityEngine;

namespace Playfab
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private PlayerInventoryVariableSO playerInventory;
        [SerializeField] private ObjectContainerVariableSO objectContainer;
        [SerializeField] private PlayfabBusSO playfabBusSO;
        private void OnEnable()
        {
            playfabBusSO.OnUpdateInventory += GrantItemToUserRequest;
            playfabBusSO.OnLoadPlayfabInventory += LoadInventory;
        }
        private void OnDisable()
        {
            playfabBusSO.OnUpdateInventory -= GrantItemToUserRequest;
            playfabBusSO.OnLoadPlayfabInventory -= LoadInventory;
        }

        private void GrantItemToUserRequest(List<string> obj)
        {
            Debug.Log("Se va a agregar objetos al inventario del jugador en el servidor");
            PlayfabManager.Instance.GetAccountInfoRequest(
            (onSuccessAccountInfo) =>
            {
                Debug.Log("Se ha obtenido la informacion del usuario " + onSuccessAccountInfo.AccountInfo.PlayFabId);
                PlayfabManager.Instance.GrantItemToUserRequest(onSuccessAccountInfo.AccountInfo.PlayFabId,obj,
                (onSuccessGrantItemToUserRequest) =>
                {
                    Debug.Log("Se han agregado correctamente los objetos al inventario del jugador " + onSuccessAccountInfo.AccountInfo.PlayFabId);
                    playfabBusSO.OnSucessUpdateInventory?.Invoke();
                }, 
                (onError) =>
                {
                    Debug.LogError("Ha ocurrido un error al agregar los objetos al inventario del jugador "+ onSuccessAccountInfo.AccountInfo.PlayFabId);
                    playfabBusSO.OnErrorPlayFab?.Invoke(onError.GenerateErrorReport().ToString());
                });
            },
            (onError) =>
            {
                Debug.LogError("No se ha podido obtener la informacion del jugador");
                playfabBusSO.OnErrorPlayFab?.Invoke(onError.GenerateErrorReport().ToString());
            });

        }

        [ContextMenu("ShowPlayerInventory")]
        public void ShowPlayerInventory()
        {
            Debug.Log("Se va a mostrar el inventario del jugador");
            PlayfabManager.Instance.GetUserInventoryRequest(
            (onSucess) =>
            {
                foreach(ItemInstance item in onSucess.Inventory )
                {
                    Debug.Log("Player owns "+item.ItemId);
                }
            },
            (onError) =>
            {

            });    
        }  

        [ContextMenu("Load Inventory")]
        public void LoadInventory()
        {
            PlayfabManager.Instance.GetUserInventoryRequest(
            (onSucess) =>
            {
                foreach (ItemInstance item in onSucess.Inventory)
                {
                    ObjectInventoryVariableSO aux = objectContainer.GetObjectInventory(item.ItemId);
                    if (aux)
                    {
                        playerInventory.AddObjectInventory(aux);
                    }
                    else
                    {
                        Debug.LogError("ID no reconocido del objeto " + item.ItemId);
                    }
                    Debug.Log("Player owns " + item.ItemId);
                }
            },
            (onError) =>
            {
                Debug.LogError("No se ha podido cargar el inventario del jugador");
                playfabBusSO.OnErrorPlayFab?.Invoke(onError.GenerateErrorReport().ToString());
            });
        }
    }
}
