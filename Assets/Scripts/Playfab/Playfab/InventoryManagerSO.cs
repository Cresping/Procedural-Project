using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.SO
{
    [CreateAssetMenu(fileName = "InventoryManagerSO", menuName = "Scriptables/Playfab/InventoryManagerSO")]
    public class InventoryManagerSO : ScriptableObject
    {
        private const string ERROR_GRANT_ITEM_TO_USER = "002";
        private const string ERROR_SHOW_INVENTORY = "004";
        [SerializeField] private LoginManagerSO loginManager;
        [SerializeField] private PlayfabManagerSO playfabManager;
        [SerializeField] private PlayerInventoryVariableSO playerInventory;
        [SerializeField] private ObjectContainerVariableSO objectContainer;
        [SerializeField] private PlayfabBusDataSO playfabBusSO;
        private List<string> _currentObj;
        private void OnEnable()
        {
            playfabBusSO.OnSucessLogin += LoadInventory;
            playfabBusSO.OnReplyErrorPlayFab += OnErrorResponse;
        }
        private void OnDisable()
        {
            playfabBusSO.OnSucessLogin -= LoadInventory;
            playfabBusSO.OnReplyErrorPlayFab -= OnErrorResponse;
        }
        public void OnErrorResponse(string errorCode, bool retry)
        {
            switch (errorCode)
            {
                case ERROR_GRANT_ITEM_TO_USER:
                    if (retry)
                    {
                        GrantItemToUserRequest(_currentObj);
                    }
                    else
                    {
                        playfabBusSO.OnIgnoreUpdateInventory?.Invoke();
                    }

                    break;
                case ERROR_SHOW_INVENTORY:
                    if (retry)
                    {
                        LoadInventory();
                    }
                    break;
            }
        }
        public void GrantItemToUserRequest(List<string> obj)
        {
            if (loginManager.IsAlreadyLogged)
            {
                _currentObj = obj;
                Debug.Log("Se va a agregar objetos al inventario del jugador en el servidor");
                playfabManager.GetAccountInfoRequest(
                (onSuccessAccountInfo) =>
                {
                    Debug.Log("Se ha obtenido la informacion del usuario " + onSuccessAccountInfo.AccountInfo.PlayFabId);
                    playfabManager.GrantItemToUserRequest(onSuccessAccountInfo.AccountInfo.PlayFabId, obj,
                    (onSuccessGrantItemToUserRequest) =>
                    {
                        Debug.Log("Se han agregado correctamente los objetos al inventario del jugador " + onSuccessAccountInfo.AccountInfo.PlayFabId);
                        playfabBusSO.OnSucessUpdateInventory?.Invoke();
                    },
                    (onError) =>
                    {
                        Debug.LogError("Ha ocurrido un error al agregar los objetos al inventario del jugador " + onSuccessAccountInfo.AccountInfo.PlayFabId);
                        playfabBusSO.OnErrorPlayFab?.Invoke(ERROR_GRANT_ITEM_TO_USER);
                    });
                },
            (onError) =>
                {
                    Debug.LogError("No se ha podido obtener la informacion del jugador");
                    playfabBusSO.OnErrorPlayFab?.Invoke(ERROR_GRANT_ITEM_TO_USER);
                });
            }
            else
            {
                Debug.Log("El jugador no está conectado a su cuenta");
            }
        }

        [ContextMenu("ShowPlayerInventory")]
        public void ShowPlayerInventory()
        {
            Debug.Log("Se va a mostrar el inventario del jugador");
            playfabManager.GetUserInventoryRequest(
            (onSucess) =>
            {
                foreach (ItemInstance item in onSucess.Inventory)
                {
                    Debug.Log("Player owns " + item.ItemId);
                }
            },
            (onError) =>
            {

            });
        }

        [ContextMenu("Load Inventory")]
        public void LoadInventory()
        {
            if (loginManager.IsAlreadyLogged)
            {
                playfabManager.GetUserInventoryRequest(
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
                    playfabBusSO.OnSucessLoadPlayfabInventory?.Invoke();
                },
                (onError) =>
                {
                    Debug.LogError("No se ha podido cargar el inventario del jugador");
                    playfabBusSO.OnErrorPlayFab?.Invoke(ERROR_SHOW_INVENTORY);
                });
            }
            else
            {
                Debug.Log("El jugador no está conectado a su cuenta");
            }

        }
    }
}
