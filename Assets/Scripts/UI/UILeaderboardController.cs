using System.Collections;
using System.Collections.Generic;
using HeroesGames.ProjectProcedural.SO;
using TMPro;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.UI
{
    public class UILeaderboardController : MonoBehaviour
    {
        [SerializeField] private LeaderboardManagerSO leaderboardManagerSO;
        [SerializeField] private PlayfabBusDataSO playfabBusDataSO;
        [SerializeField] private GameObject leaderboardPanel;
        [SerializeField] private TextMeshProUGUI leaderboardText;
        private void Awake()
        {
            leaderboardPanel.SetActive(false);
        }
        private void OnEnable()
        {
            playfabBusDataSO.OnSucessLoadLeaderboard += OnSucessLoadLeaderboard;
        }
        private void OnDisable()
        {
            playfabBusDataSO.OnSucessLoadLeaderboard -= OnSucessLoadLeaderboard;
        }
        private void OnSucessLoadLeaderboard(string data)
        {
            leaderboardText.text = data;
            leaderboardPanel.SetActive(true);
        }
        public void CloseLeaderboardPanel()
        {
            leaderboardPanel.SetActive(false);
        }
        public void LoadLeaderboard()
        {
            leaderboardManagerSO.GetLeaderboard();
        }
        public void LoadLeaderboardUser()
        {
            leaderboardManagerSO.GetLeaderboardAroundPlayer();
        }
    }
}

