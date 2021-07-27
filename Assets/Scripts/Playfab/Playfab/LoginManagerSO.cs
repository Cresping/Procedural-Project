using Playfab;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;


namespace HeroesGames.ProjectProcedural.SO
{
    [CreateAssetMenu(fileName = "LoginManagerSO", menuName = "Scriptables/Playfab/LoginManagerSO")]
    public class LoginManagerSO : ScriptableObject
    {
        private const string LOGIN_ERROR_CODE = "001";
        private const string UPDATE_NAME_ERROR_CODE = "005";
        [SerializeField] private PlayfabManagerSO playfabManager;
        [SerializeField] private PlayfabBusDataSO playfabBus;
        [SerializeField] private string gameVersion;
        [SerializeField] private EconomyModel serverEconomy;
        [SerializeField] private EventSO welcomeEvent;

        private bool isAlreadyLogged;
        private string _userName = null;

        public bool NoUserName { get; set; }
        public string NickName { get; set; }

        public bool IsAlreadyLogged { get => isAlreadyLogged; set => isAlreadyLogged = value; }

        private void OnEnable()
        {
            isAlreadyLogged = false;
            NoUserName = false;
            playfabBus.OnLogin += ServerLogin;
            playfabBus.OnReplyErrorPlayFab += OnErrorResponse;

        }
        private void OnDisable()
        {
            playfabBus.OnLogin -= ServerLogin;
            playfabBus.OnReplyErrorPlayFab -= OnErrorResponse;
        }
        private void OnErrorResponse(string errorCode, bool retry)
        {
            switch (errorCode)
            {
                case LOGIN_ERROR_CODE:
                    if (retry)
                    {
                        ServerLogin();
                    }
                    else
                    {
                        playfabBus.OnIgnoreLogin?.Invoke();
                    }
                    break;
                case UPDATE_NAME_ERROR_CODE:
                    if(!retry)
                    {
                        NoUserName = false;
                    }
                    break;
            }
        }
        [ContextMenu("ServerLogin")]
        private void ServerLogin()
        {
            if (!isAlreadyLogged)
            {
                playfabManager.Login(
                onSuccess =>
                {
                    isAlreadyLogged = true;
                    NickName = null;
                    if (!onSuccess.NewlyCreated &&
                        !string.IsNullOrWhiteSpace(onSuccess.InfoResultPayload.PlayerProfile.DisplayName))
                    {
                        NickName = onSuccess.InfoResultPayload.PlayerProfile.DisplayName;
                        welcomeEvent.CurrentAction?.Invoke();
                        playfabBus.OnSucessLogin?.Invoke();
                    }

                    if (string.IsNullOrWhiteSpace(NickName))
                        NoUserName = true;

                    Debug.Log("User login: " + onSuccess.PlayFabId);
                    Debug.Log("User newly created: " + onSuccess.NewlyCreated);
                    LoadServerData();
                },
                onError =>
                {
                    Debug.LogError("Login failed: " + onError.ErrorMessage);
                    playfabBus.OnErrorPlayFab?.Invoke(LOGIN_ERROR_CODE);
                });
            }
        }

        private void LoadServerData()
        {
            playfabManager.GetTitleData(
                titleDataReached
                    => LoadGameSetup(titleDataReached.Data),

                titleDataError
                    => Debug.LogError("Title Data Failure: " + titleDataError.ErrorMessage));
        }


        private void LoadGameSetup(Dictionary<string, string> gameData)
        {
            SetPlayfabVersion(gameData["ClientVersion"]);
            SetPlayfabEconomyModel(gameData["EconomySetup"]);
        }

        private void SetPlayfabVersion(string version) => gameVersion = version;

        private void SetPlayfabEconomyModel(string economyJson) => JsonUtility.FromJsonOverwrite(economyJson, serverEconomy);

        public void UpdateUserName(string playerUserName)
        {
            var request = new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = playerUserName
            };

            PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnUserNameUpdated, OnUserNameUpdateError);
        }

        private void OnUserNameUpdated(UpdateUserTitleDisplayNameResult result)
        {
            Debug.Log("UserName Updated");
            NoUserName = false;
            NickName = result.DisplayName;
            playfabBus.OnSucessLogin?.Invoke();
            welcomeEvent.CurrentAction?.Invoke();
        }

        private void OnUserNameUpdateError(PlayFabError error)
        {
            Debug.Log(error.ErrorMessage);
            NoUserName = true;
            playfabBus.OnErrorPlayFab?.Invoke(UPDATE_NAME_ERROR_CODE);
        }

        public void GetPlayerProfile(string playFabId)
        {
            PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest
            {
                PlayFabId = playFabId,
                ProfileConstraints = new PlayerProfileViewConstraints
                {
                    ShowDisplayName = true
                }
            },
                result => Debug.Log("The player's DisplayName profile data is: " + result.PlayerProfile.DisplayName),
                error =>
                {
                    Debug.LogError(error.GenerateErrorReport());
                    playfabBus.OnErrorPlayFab?.Invoke(LOGIN_ERROR_CODE);

                });
        }


    }
}
