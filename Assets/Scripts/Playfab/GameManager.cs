using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Playfab
{
    public class GameManager : Singleton<GameManager>
    {
        private void Start() => ServerLogin();

        #region LOGIN

// Establish connection with Playfab Server through PlayfabManager
        private void ServerLogin() => PlayfabManager.Instance.Login(OnLoginSuccess, OnLoginError);

        private void OnLoginSuccess(LoginResult loginResult)
        {
            Debug.Log("User login: " + loginResult.PlayFabId);
            Debug.Log("User newly created: " + loginResult.NewlyCreated);
        }

        private void OnLoginError(PlayFabError playFabError) => Debug.Log("Login failed: " + playFabError.ErrorMessage);

        #endregion
    }
}