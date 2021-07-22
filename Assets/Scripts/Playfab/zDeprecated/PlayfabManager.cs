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

        #region LOGIN

        // Select ServerID depends on isTestServer bool
        private void SelectPlayfabServer()
        {
            PlayFabSettings.TitleId = isTestServer ? PLAYFAB_TEST_SERVER : PLAYFAB_PRODUCTION_SERVER;
        } 


        // Create a user request and try to Log in Playfab Server using PlayfabSettings

        //TODO Hacer otro login creando una cuenta con GooglePlay o  Playfab
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

        public void GetUserInventoryRequest(Action<GetUserInventoryResult> onSucess, Action<PlayFabError> onError)
        {
            var request = new GetUserInventoryRequest();
            PlayFabClientAPI.GetUserInventory(request,
            (result) =>
            {
                onSucess(result);
            },
            (error) =>
            {
                onError?.Invoke(error);
            }
            );
        }
        public void GrantItemToUserRequest(string idUser, List<string> objectsId, Action<PlayFab.ServerModels.GrantItemsToUserResult> onSucess, Action<PlayFabError> onError)
        {
            var request = new PlayFab.ServerModels.GrantItemsToUserRequest()
            {
                CatalogVersion = "EquippableObjects",
                ItemIds = objectsId,
                PlayFabId = idUser
            };
            PlayFabServerAPI.GrantItemsToUser(request,
            (result) =>
            {
                 onSucess(result);
             },
            (error) =>
            {
                onError?.Invoke(error);
            }
            );
        }
        public void GetAccountInfoRequest(Action<GetAccountInfoResult> onSucess, Action<PlayFabError> onError)
        {
            GetAccountInfoRequest request = new GetAccountInfoRequest();
            PlayFabClientAPI.GetAccountInfo(request,
           (result) =>
           {
               onSucess(result);
           },
           (error) =>
           {
               onError?.Invoke(error);
           }
           );
        }


    }
}