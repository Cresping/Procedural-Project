using System;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Playfab
{
    public class PlayFabLogin
    {
        [SerializeField] private bool isTestServer = true;
        public event Action<string> OnSuccess;
        
        private const string PLAYFAB_TEST_SERVER = "50267"; // 1 Min Dngon (Test) ID
        private const string PLAYFAB_PRODUCTION_SERVER = "4A8C2"; // 1 Min Dngon (Production) ID
        
        public void Login()
        {
            if (string.IsNullOrEmpty(PlayFabSettings.TitleId)) 
               PlayFabSettings.TitleId = isTestServer ? PLAYFAB_TEST_SERVER : PLAYFAB_PRODUCTION_SERVER;

            var request = new LoginWithCustomIDRequest
            {
                CustomId = "GettingStartedGuide3",
                CreateAccount = true
            };
            
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
        }

        private void OnLoginSuccess(LoginResult result)
        {
            Debug.Log("Login");
            OnSuccess?.Invoke(result.PlayFabId);
        }

        private void OnLoginFailure(PlayFabError error)
            => Debug.LogError($"Here's some debug information: {error.GenerateErrorReport()}");
        
    }
}
