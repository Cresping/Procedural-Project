using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Playfab
{
    public class Leaderboard : MonoBehaviour
    {
        [SerializeField] private Button _getHighScoresdButton;
        [SerializeField] private Button _addPlayerScoreButton;
        [SerializeField] private Button _getPlayerScoreButton;

        [SerializeField] private TextMeshProUGUI _resultsText;

        private const string LeaderboardName = "Weekly High Scores";

        private string _playerId;
        private int _score;

        private PlayFabLogin _playFabLogin;
        private PlayFabUpdatePlayerStatistics _playFabUpdatePlayerStatistics;
        private PlayFabGetLeaderboardAroundPlayer _playFabGetLeaderboardAroundPlayer;
        private PlayFabGetLeaderboard _playFabGetLeaderboard;

        private void Start()
        {
            AddListeners();
            CreatePlayFabServices();
            Login();
        }

        private void CreatePlayFabServices()
        {
            _playFabLogin = new PlayFabLogin();
            _playFabLogin.OnSuccess += playerId => _playerId = playerId;

            _playFabUpdatePlayerStatistics = new PlayFabUpdatePlayerStatistics();

            _playFabGetLeaderboardAroundPlayer = new PlayFabGetLeaderboardAroundPlayer();
            _playFabGetLeaderboardAroundPlayer.OnSuccess += result => _resultsText.SetText(result);

            _playFabGetLeaderboard = new PlayFabGetLeaderboard();
            _playFabGetLeaderboard.OnSuccess += result => _resultsText.SetText(result);
        }
        
        // This is a test method
        private int GetRandomScore()
            => Random.Range(100, 100000);

        private void Login()
            => _playFabLogin.Login();

        private void AddListeners()
        {
            _getHighScoresdButton.onClick.AddListener(OnGetLeaderboardButtonPressed);
            _addPlayerScoreButton.onClick.AddListener(OnAddPlayerScoreButtonPressed);
            _getPlayerScoreButton.onClick.AddListener(OnGetPlayerScoreButtonPressed);
        }

        public void OnAddPlayerScoreButtonPressed()
        {
            _score = GetRandomScore();
            _playFabUpdatePlayerStatistics.UpdatePlayerStatistics(LeaderboardName, _score);
            Debug.Log($"new Score is: {_score}");
        }

        public void OnGetPlayerScoreButtonPressed()
            => _playFabGetLeaderboardAroundPlayer.GetLeaderboardAroundPlayer(_playerId, 1, LeaderboardName);

        public void OnGetLeaderboardButtonPressed()
            => _playFabGetLeaderboard.GetLeaderboardEntries(0, 10, LeaderboardName);
    }
}