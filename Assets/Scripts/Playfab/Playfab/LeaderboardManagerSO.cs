using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.SO
{
    [CreateAssetMenu(fileName = "LeaderboardManagerSO", menuName = "Scriptables/Playfab/LeaderboardManagerSO")]
    public class LeaderboardManagerSO : ScriptableObject
    {
        private const string LEADERBOARD_NAME = "Weekly High Scores";
        private const string LOAD_LEADERBOARD_ERROR = "003";
        private const string LOAD_LEADERBOARD_ERROR_USER = "007";
        private const string UPDATE_LEADERBOARD_ERROR = "006";
        [SerializeField] private PlayfabManagerSO playfabManagerSO;
        [SerializeField] private LoginManagerSO loginManagerSO;
        [SerializeField] private PlayfabBusDataSO playfabBusDataSO;
        [SerializeField] private PlayerRecordsVariableSO playerRecordsVariableSO;
        private int _currentScore;
        private void OnEnable()
        {
            playfabBusDataSO.OnReplyErrorPlayFab += OnErrorResponse;
            playfabBusDataSO.OnSucessLogin += GetMaxPlayerLeaderboardValue;
        }
        private void OnDisable()
        {
            playfabBusDataSO.OnReplyErrorPlayFab -= OnErrorResponse;
            playfabBusDataSO.OnSucessLogin -= GetMaxPlayerLeaderboardValue;
        }
        public void OnErrorResponse(string errorCode, bool retry)
        {
            switch (errorCode)
            {
                case LOAD_LEADERBOARD_ERROR:
                    if (retry)
                    {
                        GetLeaderboard();
                    }
                    break;
                case UPDATE_LEADERBOARD_ERROR:
                    if (retry)
                    {
                        UpdatePlayerStatistics(_currentScore);
                    }
                    break;
                case LOAD_LEADERBOARD_ERROR_USER:
                    if (retry)
                    {
                        GetLeaderboardAroundPlayer();
                    }
                    break;
            }
        }
        public void GetLeaderboard()
        {
            if (loginManagerSO.IsAlreadyLogged)
            {
                playfabManagerSO.GetLeaderboard(0, 10, LEADERBOARD_NAME,
                (onSuccessGetLeaderBoard) =>
                {
                    Debug.Log("Se ha obtenido la informacion de la leaderboard");
                    var leaderboard = new StringBuilder();
                    foreach (var playerLeaderboardEntry in onSuccessGetLeaderBoard.Leaderboard)
                    {
                        leaderboard.AppendLine($"User: {playerLeaderboardEntry.DisplayName} ----- Dungeon: {playerLeaderboardEntry.StatValue}");
                    }
                    playfabBusDataSO.OnSucessLoadLeaderboard?.Invoke(leaderboard.ToString());

                },
            (onError) =>
                {
                    Debug.LogError("No se ha podido obtener la informacion de la leaderboard");
                    playfabBusDataSO.OnErrorPlayFab?.Invoke(LOAD_LEADERBOARD_ERROR);
                });
            }
            else
            {
                Debug.LogError("No se ha iniciado sesión");
                playfabBusDataSO.OnErrorPlayFab?.Invoke(LOAD_LEADERBOARD_ERROR);
            }
        }
        public void GetLeaderboardAroundPlayer()
        {
            if (loginManagerSO.IsAlreadyLogged)
            {
                playfabManagerSO.GetAccountInfoRequest(
                (onSuccessAccountInfo) =>
                {
                    Debug.Log("Se ha obtenido la informacion del usuario " + onSuccessAccountInfo.AccountInfo.PlayFabId);
                    playfabManagerSO.GetLeaderboardAroundPlayer(onSuccessAccountInfo.AccountInfo.PlayFabId, 10, LEADERBOARD_NAME,
                    (onSuccessGetLeaderBoard) =>
                    {
                        Debug.Log("Se ha obtenido la informacion de la leaderboard del usuario");
                        var leaderboard = new StringBuilder();
                        foreach (var playerLeaderboardEntry in onSuccessGetLeaderBoard.Leaderboard)
                        {
                            leaderboard.AppendLine($"User: {playerLeaderboardEntry.DisplayName} ----- Dungeon: {playerLeaderboardEntry.StatValue}");
                        }
                        playfabBusDataSO.OnSucessLoadLeaderboard?.Invoke(leaderboard.ToString());
                    },
                    (onError) =>
                    {
                        Debug.LogError("No se ha podido obtener la informacion de la leaderboard");
                        playfabBusDataSO.OnErrorPlayFab?.Invoke(LOAD_LEADERBOARD_ERROR_USER);
                    });
                },
                (onError) =>
                {
                    Debug.LogError("No se ha podido obtener la informacion del jugador");
                    playfabBusDataSO.OnErrorPlayFab?.Invoke(LOAD_LEADERBOARD_ERROR_USER);
                });
            }
            else
            {
                Debug.LogError("No se ha iniciado sesión");
                playfabBusDataSO.OnErrorPlayFab?.Invoke(LOAD_LEADERBOARD_ERROR_USER);
            }
        }
        public void GetMaxPlayerLeaderboardValue()
        {
            if (loginManagerSO.IsAlreadyLogged)
            {
                playfabManagerSO.GetAccountInfoRequest(
               (onSuccessAccountInfo) =>
               {
                   Debug.Log("Se ha obtenido la informacion del usuario " + onSuccessAccountInfo.AccountInfo.PlayFabId);
                   playfabManagerSO.GetLeaderboardAroundPlayer(onSuccessAccountInfo.AccountInfo.PlayFabId, 1, LEADERBOARD_NAME,
                   (onSuccessGetLeaderBoard) =>
                   {
                       Debug.Log("Se ha obtenido la informacion de la leaderboard del usuario, se va a actualizar los valores de los Records");
                       var leaderboard = new StringBuilder();
                       foreach (var playerLeaderboardEntry in onSuccessGetLeaderBoard.Leaderboard)
                       {
                           playerRecordsVariableSO.MaxDungeonLevel = playerLeaderboardEntry.StatValue;
                           break;
                       }
                       playfabBusDataSO.OnSucessLoadPlayerLeaderboardRecord?.Invoke();
                   },
                   (onError) =>
                   {
                       Debug.LogError("No se ha podido obtener la informacion de la leaderboard");
                       playfabBusDataSO.OnErrorPlayFab?.Invoke(LOAD_LEADERBOARD_ERROR_USER);
                   });
               },
               (onError) =>
               {
                   Debug.LogError("No se ha podido obtener la informacion del jugador");
                   playfabBusDataSO.OnErrorPlayFab?.Invoke(LOAD_LEADERBOARD_ERROR_USER);
               });
            }
            else
            {
                Debug.LogError("No se ha iniciado sesión");
            }
        }
        public void UpdatePlayerStatistics(int score)
        {
            if (loginManagerSO.IsAlreadyLogged)
            {
                playfabManagerSO.UpdatePlayerStatistics(LEADERBOARD_NAME, score,
                (onSuccessUpdatePlayerStatistics) =>
                {
                    Debug.Log("Se ha actualizado la información de la leader board");

                },
                (onError) =>
                {
                    Debug.LogError("No se ha podido obtener la informacion de la leaderboard");
                    playfabBusDataSO.OnErrorPlayFab?.Invoke(UPDATE_LEADERBOARD_ERROR);
                });
            }
            else
            {
                Debug.Log("No se ha iniciado sesión, no se actualizará los datos de los records");
            }
        }
    }
}
