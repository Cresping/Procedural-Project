using System;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Playfab
{
    public class PlayfabManager : Singleton<PlayfabManager>
    {
    
    #region LOGIN

    public void Login(Action<LoginResult> onSuccess, Action<PlayFabError> onError)
    {
        var request= new LoginWithCustomIDRequest
        {
            CreateAccount = true,//If account doesn't exist, create it!
            CustomId =SystemInfo.deviceUniqueIdentifier//the custom ID is the Unique Device Identifier
        };

        PlayFabClientAPI.LoginWithCustomID(request, onSuccess, onError);
    }

    #endregion
    }
}
