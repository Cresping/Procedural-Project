using System;
using System.Collections.Generic;
using HeroesGames.ProjectProcedural.SO;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Playfab
{
    public class PlayfabManager : Singleton<PlayfabManager>
    {
        [SerializeField] private bool isTestServer = true;
        [SerializeField] private PlayfabBusSO playfabBusSO;
        private const string PLAYFAB_TEST_SERVER = "50267"; // 1 Min Dngon (Test) ID
        private const string PLAYFAB_PRODUCTION_SERVER = "4A8C2"; // 1 Min Dngon (Production) ID
        private string idUser = null;
        private List<string> _tempObjectsId;
        
        
        private void Awake()
        {
            SelectPlayfabServer();
        }
        private void OnEnable()
        {
            playfabBusSO.OnUpdateInventory += GrantItemToUserRequest;
        }
        private void OnDisable()
        {
            playfabBusSO.OnUpdateInventory -= GrantItemToUserRequest;
        }


        #region LOGIN

        // Select ServerID depends on isTestServer bool
        private void SelectPlayfabServer() => PlayFabSettings.TitleId =
            isTestServer ? PLAYFAB_TEST_SERVER : PLAYFAB_PRODUCTION_SERVER;


        // Create a user request and try to Log in Playfab Server using PlayfabSettings

        //TODO Hacer otro login creando una cuenta con GooglePlay
        public void Login(Action<LoginResult> onSuccess, Action<PlayFabError> onError)
        {
#if UNITY_ANDROID
            var requestWithAndroid = new LoginWithAndroidDeviceIDRequest()
            {
                CreateAccount = true, //If account doesn't exist, create it!
                AndroidDeviceId = SystemInfo.deviceUniqueIdentifier //the custom ID is the Unique Device Identifier
            };

            PlayFabClientAPI.LoginWithAndroidDeviceID(requestWithAndroid, onSuccess, onError);
#else
            var request = new LoginWithCustomIDRequest
            {
                CreateAccount = true, //If account doesn't exist, create it!
                CustomId = SystemInfo.deviceUniqueIdentifier //the custom ID is the Unique Device Identifier
            };

            PlayFabClientAPI.LoginWithCustomID(request, onSuccess, onError);
#endif
        }

        #endregion

        #region TITLE DATA

        public void GetTitleData(Action<GetTitleDataResult> onSuccess, Action<PlayFabError> onError)
            => PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), onSuccess, onError);

        #endregion

        public void GetInventory(Action<GetUserInventoryResult> onSucess, Action onError = null)
        {
            var request = new GetUserInventoryRequest()
            {

            };
            PlayFabClientAPI.GetUserInventory(request,
            (result) =>
            {
                onSucess(result);
            },
            (error) =>
            {
                onError?.Invoke();
            }
            );
        }
        [ContextMenu("AddInventory")]
        public void GrantItemToUser(string idUser, List<string> objectsId)
        {
            PlayFabServerAPI.GrantItemsToUser(new PlayFab.ServerModels.GrantItemsToUserRequest
            {
                CatalogVersion = "EquippableObjects",
                ItemIds = objectsId,
                PlayFabId = idUser
            }, LogSuccesGrantItemToUser, LogFailureGrantItemToUser); 
        }
        [ContextMenu("AccountInformation")]
        public void GrantItemToUserRequest(List<string> objectsId)
        {
            if(idUser==null)
            {
                _tempObjectsId = objectsId;
                GetAccountInfoRequest request = new GetAccountInfoRequest();
                PlayFabClientAPI.GetAccountInfo(request, LogSuccessAccountInformationGrantItemToUser, LogFailureGrantItemToUser);
            }
            else
            {
                GrantItemToUser(idUser, objectsId);
            }
        }

        private void LogSuccessAccountInformationGrantItemToUser(GetAccountInfoResult obj)
        {
            Debug.Log("Se ha obtenido la informacion de la cuenta con exito");
            Debug.Log("ID de la cuenta: "+obj.AccountInfo.PlayFabId);
            idUser = obj.AccountInfo.PlayFabId;
            GrantItemToUser(idUser, _tempObjectsId);
        }
        private void LogSuccesGrantItemToUser(PlayFab.ServerModels.GrantItemsToUserResult obj)
        {
            Debug.Log("Se han agregado objetos al usuario " + idUser + " de forma satisfactoria");
            _tempObjectsId = new List<string>();
            playfabBusSO.OnSucessUpdateInventory?.Invoke();
        }
        private void LogFailureGrantItemToUser(PlayFabError obj)
        {
            Debug.LogError("Ha ocurrido un error con Playfab:" + obj.GenerateErrorReport());
            playfabBusSO.OnFailedUpdateInventory?.Invoke(obj.ErrorMessage);

        }


    }
}