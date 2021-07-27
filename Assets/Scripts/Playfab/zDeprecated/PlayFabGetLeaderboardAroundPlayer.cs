using System;
using System.Text;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Playfab
{
    public class PlayFabGetLeaderboardAroundPlayer
    {
        public event Action<string> OnSuccess; 
        
        public void GetLeaderboardAroundPlayer(string playerId, int maxResultsCount, string leaderboardName)
        {
            var request = new GetLeaderboardAroundPlayerRequest
            {
                PlayFabId = playerId,
                MaxResultsCount = maxResultsCount,
                StatisticName = leaderboardName
            };
            PlayFabClientAPI.GetLeaderboardAroundPlayer(request,
                OnGetLeaderboardAroundPlayerSuccess,
                GetLeaderboardAroundPlayerFailure);
        }

        private void GetLeaderboardAroundPlayerFailure(PlayFabError error)
            => Debug.LogError($"Here's some debug information: {error.GenerateErrorReport()}");

        private void OnGetLeaderboardAroundPlayerSuccess(GetLeaderboardAroundPlayerResult response)
        {
            var leaderboard = new StringBuilder();
            foreach (var playerLeaderboardEntry in response.Leaderboard)
                leaderboard.AppendLine($"User: {playerLeaderboardEntry.DisplayName}  -----  Score: {playerLeaderboardEntry.StatValue}");

            OnSuccess?.Invoke(leaderboard.ToString());
        }
    }
}