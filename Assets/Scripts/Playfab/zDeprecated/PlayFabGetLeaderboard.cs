using System;
using System.Text;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Playfab
{
    public class PlayFabGetLeaderboard
    {
        public event Action<string> OnSuccess; 
        
        public void GetLeaderboardEntries(int startPosition, int maxResultsCount, string leaderboardName)
        {
            var request = new GetLeaderboardRequest
            {
                StartPosition = startPosition,
                MaxResultsCount = maxResultsCount,
                StatisticName = leaderboardName
            };
            PlayFabClientAPI.GetLeaderboard(request,
                OnGetLeaderboardSuccess,
                OnGetLeaderboardFailure);
        }

        private void OnGetLeaderboardFailure(PlayFabError error)
            => Debug.LogError($"Here's some debug information: {error.GenerateErrorReport()}");

        private void OnGetLeaderboardSuccess(GetLeaderboardResult response)
        {
            var leaderboard = new StringBuilder();
            foreach (var playerLeaderboardEntry in response.Leaderboard)
                leaderboard.AppendLine($"User: {playerLeaderboardEntry.DisplayName}  -----  Score: {playerLeaderboardEntry.StatValue}");

            OnSuccess?.Invoke(leaderboard.ToString());
        }
    }
}