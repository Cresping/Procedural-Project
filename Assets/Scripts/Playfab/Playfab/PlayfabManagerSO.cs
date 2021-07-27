using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace HeroesGames.ProjectProcedural.SO
{
    [CreateAssetMenu(fileName = "PlayfabManagerSO", menuName = "Scriptables/Playfab/PlayfabManagerSO")]
    public class PlayfabManagerSO : ScriptableObject
    {
        [SerializeField] private bool isTestServer = true;
        [SerializeField] private PlayfabBusDataSO playfabBusSO;
        private const string PLAYFAB_TEST_SERVER = "50267"; // 1 Min Dngon (Test) ID
        private const string PLAYFAB_PRODUCTION_SERVER = "4A8C2"; // 1 Min Dngon (Production) ID


        private void OnEnable()
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
                AndroidDeviceId = SystemInfo.deviceUniqueIdentifier, //the custom ID is the Unique Device Identifier
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                {
                    GetPlayerProfile = true
                }
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

        public void GetPlayerProfile(string playFabId)
        {
            PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest()
            {
                PlayFabId = playFabId,
                ProfileConstraints = new PlayerProfileViewConstraints()
                {
                    ShowDisplayName = true
                }
            },
                result => Debug.Log("The player's DisplayName profile data is: " + result.PlayerProfile.DisplayName),
                error => Debug.LogError(error.GenerateErrorReport()));
        }
        public void GetLeaderboardAroundPlayer(string playerId, int maxResultsCount, string leaderboardName, Action<GetLeaderboardAroundPlayerResult> onSucess, Action<PlayFabError> onError)
        {
            var request = new GetLeaderboardAroundPlayerRequest
            {
                PlayFabId = playerId,
                MaxResultsCount = maxResultsCount,
                StatisticName = leaderboardName
            };
            PlayFabClientAPI.GetLeaderboardAroundPlayer(request,
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
        public void UpdatePlayerStatistics(string leaderboardName, int score, Action<UpdatePlayerStatisticsResult> onSucess, Action<PlayFabError> onError)
        {
            var request = new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>
                {
                    new StatisticUpdate
                    {
                        StatisticName = leaderboardName,
                        Value = score
                    }
                },
            };
            PlayFabClientAPI.UpdatePlayerStatistics(request,
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
        public void GetLeaderboard(int startPosition, int maxResultsCount, string leaderboardName, Action<GetLeaderboardResult> onSucess, Action<PlayFabError> onError)
        {
            var request = new GetLeaderboardRequest
            {
                StartPosition = startPosition,
                MaxResultsCount = maxResultsCount,
                StatisticName = leaderboardName
            };
            PlayFabClientAPI.GetLeaderboard(request,
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

