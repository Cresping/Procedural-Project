using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Playfab
{
    public class PlayfabManager : Singleton<PlayfabManager>
    {
        [SerializeField] private bool isTestServer = true;

        private const string PLAYFAB_TEST_SERVER = "50267"; // 1 Min Dngon (Test) ID
        private const string PLAYFAB_PRODUCTION_SERVER = "4A8C2"; // 1 Min Dngon (Production) ID
        private string idUser = null;
        private void Awake() => SelectPlayfabServer();
        
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
        public void GrantItemToUserRequest(List<string> idObjects)
        {
            if(idUser!=null)
            {
                var request = new PlayFab.ServerModels.GrantItemsToUserRequest()
                {
                    CatalogVersion = "EquippableObjects",
                    ItemIds = idObjects,
                    PlayFabId = SystemInfo.deviceUniqueIdentifier
                };
            }
        }
        [ContextMenu("AddInventory")]
        public void GrantItemToUserRequest()
        {
            PlayFabServerAPI.GrantItemsToUser(new PlayFab.ServerModels.GrantItemsToUserRequest
            {
                CatalogVersion = "EquippableObjects",
                ItemIds = new List<string> { "6852" },
                PlayFabId = idUser
            }, LogSuccess, LogFailure); 
        }
        [ContextMenu("AccountInformation")]
        public void GetAccountInformation()
        {
            GetAccountInfoRequest request = new GetAccountInfoRequest();
            PlayFabClientAPI.GetAccountInfo(request, LogSuccess, LogFailure);
        }

        private void LogSuccess(GetAccountInfoResult obj)
        {
            Debug.Log("Se ha obtenido la informacion de la cuenta con exito");
            Debug.Log("ID de la cuenta: "+obj.AccountInfo.PlayFabId);
            idUser = obj.AccountInfo.PlayFabId;
        }

        private void LogFailure(PlayFabError obj)
        {
            Debug.LogError("Pos no funciona " + obj.GenerateErrorReport());
        }

        private void LogSuccess(PlayFab.ServerModels.GrantItemsToUserResult obj)
        {
            Debug.Log("Pos si funciona");
        }
    }
}