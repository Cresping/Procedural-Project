using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Playfab
{
    public class GameManager : Singleton<GameManager>
    {

        public string gameVersion;
        public EconomyModel serverEconomy;
           

        private void Start()
        {
             ServerLogin();
        }
          
        private void ServerLogin()
        {
            PlayfabManager.Instance.Login(
            onSuccess =>
            {
                Debug.Log("User login: " + onSuccess.PlayFabId);
                Debug.Log("User newly created: " + onSuccess.NewlyCreated);
                LoadServerData();
            },
            onError =>
            {
                Debug.Log("Login failed: " + onError.ErrorMessage);
            });           
        }

        private void LoadServerData()
        {
            PlayfabManager.Instance.GetTitleData(
                titleDataReached
                    => LoadGameSetup(titleDataReached.Data),

                titleDataError
                    => Debug.Log("Title Data Failure: " + titleDataError.ErrorMessage));
        }
       

        private void LoadGameSetup(Dictionary<string, string> gameData)
        {
            SetPlayfabVersion(gameData["ClientVersion"]);
            SetPlayfabEconomyModel(gameData["EconomySetup"]);
        }

        private void SetPlayfabVersion(string version) => gameVersion = version;

        private void SetPlayfabEconomyModel(string economyJson) => JsonUtility.FromJsonOverwrite(economyJson, serverEconomy);
    }
}