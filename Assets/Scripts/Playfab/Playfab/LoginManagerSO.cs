using Playfab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HeroesGames.ProjectProcedural.SO
{
    [CreateAssetMenu(fileName = "LoginManagerSO", menuName = "Scriptables/Playfab/LoginManagerSO")]
    public class LoginManagerSO : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private PlayfabManagerSO playfabManager;
        [SerializeField] private PlayfabBusDataSO playfabBus;
        [SerializeField] private string gameVersion;
        [SerializeField] private EconomyModel serverEconomy;
        private bool isAlreadyLogged;

        public bool IsAlreadyLogged { get => isAlreadyLogged; set => isAlreadyLogged = value; }

        private void OnEnable()
        {
            isAlreadyLogged = false;
            playfabBus.OnLogin += ServerLogin;   
        }
        private void OnDisable()
        {
            playfabBus.OnLogin -= ServerLogin;
        }

        [ContextMenu("ServerLogin")]
        private void ServerLogin()
        {
            if (!isAlreadyLogged)
            {
                playfabManager.Login(
                onSuccess =>
                {
                    Debug.Log("User login: " + onSuccess.PlayFabId);
                    Debug.Log("User newly created: " + onSuccess.NewlyCreated);
                    LoadServerData();
                    playfabBus.OnSucessLogin?.Invoke();
                    isAlreadyLogged = true;
                },
                onError =>
                {
                    Debug.LogError("Login failed: " + onError.ErrorMessage);
                    playfabBus.OnErrorPlayFab?.Invoke(onError.GenerateErrorReport().ToString());
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

        public void OnBeforeSerialize()
        {

        }

        public void OnAfterDeserialize()
        {

        }
    }
}
