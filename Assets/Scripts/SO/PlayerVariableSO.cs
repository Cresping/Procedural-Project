using HeroesGames.ProjectProcedural.Player;
using HeroesGames.ProjectProcedural.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HeroesGames.ProjectProcedural.SO
{
    /// <summary>
    ///  Clase encargada de controlar los 'ScriptableObject' del jugador
    /// </summary>
    [CreateAssetMenu(fileName = "NewPlayerVariable", menuName = "Scriptables/Player/PlayerVariable")]
    public class PlayerVariableSO : ScriptableObject, ISerializationCallbackReceiver
    {
        public Action PlayerPositionOnValueChange;
        public Action PlayerSpeedOnValueChange;
        public Action PlayerLevelOnValueChange;

        [SerializeField] private DungeonVariableSO BSPDungeonVariableSO;
        [SerializeField] private GameStartBusSO gameStartBusSO;
        [SerializeField] private GameOverBusSO gameOverBusSO;
        [SerializeField] private int playerLevel = 1;
        [SerializeField] private int dungeonLevel = 1;
        [SerializeField] private PlayerController playerPrefab;
        [SerializeField] private int maxTotalPlayerHP = 100;
        [SerializeField] private int maxInitPlayer = 100;
        [SerializeField] private int playerDamage;
        [SerializeField] private int playerSpeed;
        [SerializeField] private int playerDef;

        [SerializeField] private int minExpLevel = 100;
        private int _numberEnemiesKilled;
        private EnumTypes.StatusType status;

        private PlayerController _instancedPlayer;
        private Vector2 _playerPosition;
        private Vector2 _playerPreviousPosition;
        private Vector2 _playerStartPosition;
        private int _runtimeInitialPlayerHP;
        private int _playerHP;
        private int _runtimePlayerDamage;
        private int _runtimePlayerSpeed;
        private int _runtimePlayerDef;
        private int _playerExperience;
        private int _randomRangeStats;
        private bool _isOnEvent;
        public void InstancePlayer(Vector2 position)
        {
            if (!_instancedPlayer)
            {
                _instancedPlayer = Instantiate(playerPrefab);
                PlayerPosition = position;
            }
        }
        public Vector2 PlayerPosition
        {
            get => _playerPosition;
            set
            {
                _playerPosition = value;
                PlayerPositionOnValueChange?.Invoke();
            }
        }
        public Vector2 PlayerStartPosition { get => _playerStartPosition; set => _playerStartPosition = value; }
        public Vector2 PlayerPreviousPosition { get => _playerPreviousPosition; set => _playerPreviousPosition = value; }
        public EnumTypes.StatusType Status { get => status; set => status = value; }

        public int PlayerHP
        {
            get => _playerHP;
            set
            {
                if (value > maxInitPlayer)
                {
                    _playerHP = maxInitPlayer;
                }
                else
                {
                    _playerHP = value;
                }
                if (_playerHP <= 0)
                {
                    _playerHP = 0;
                    gameOverBusSO.OnGameOverEvent?.Invoke();
                }
            }
        }

        public int RuntimePlayerDef { get => _runtimePlayerDef; set => _runtimePlayerDef = value; }
        public int RuntimePlayerSpeed
        {
            get => _runtimePlayerSpeed;
            set
            {
                _runtimePlayerSpeed = value;
                PlayerSpeedOnValueChange?.Invoke();
            }
        }
        public int RuntimePlayerDamage { get => _runtimePlayerDamage; set => _runtimePlayerDamage = value; }
        public bool IsOnEvent { get => _isOnEvent; set => _isOnEvent = value; }
        public int PlayerLevel { get => playerLevel; set => playerLevel = value; }
        public int DungeonLevel
        {
            get => dungeonLevel;
            set
            {
                dungeonLevel = value;
                BSPDungeonVariableSO.DungeonLvl = dungeonLevel;
            }
        }
        public int NumberEnemiesKilled { get => _numberEnemiesKilled; set => _numberEnemiesKilled = value; }

        public void ReceiveExperience(int exp)
        {
            _playerExperience += exp;
            while (_playerExperience > minExpLevel * playerLevel)
            {
                playerLevel += 1;
                _playerExperience -= minExpLevel;
                if (_playerExperience < 0)
                {
                    _playerExperience = 0;
                }
                IncreaseStats();
                PlayerLevelOnValueChange?.Invoke();
            }
        }
        public void IncreaseStats()
        {
            int points = UnityEngine.Random.Range(1, _randomRangeStats);
            while (points > 0)
            {
                int selectedStat = UnityEngine.Random.Range(1, _randomRangeStats);
                switch (selectedStat)
                {
                    case 1:
                        _runtimePlayerDamage++;
                        break;
                    case 2:
                        _runtimePlayerDef++;
                        break;
                    case 3:
                        _runtimePlayerSpeed++;
                        break;
                    case 4:
                        if (_runtimeInitialPlayerHP + 5 >= maxTotalPlayerHP)
                        {
                            _runtimeInitialPlayerHP = maxTotalPlayerHP;
                            PlayerHP += 10;
                            _randomRangeStats--;
                        }
                        else
                        {
                            _runtimeInitialPlayerHP += 5;
                            PlayerHP += 5;
                        }
                        break;
                    default:
                        Debug.LogError("Error al subir de nivel, estadística no controlada");
                        break;
                }
                points--;
            }
        }
        public int ReceiveDamage(int damage)
        {
            int actualDamage = damage - RuntimePlayerDef;
            if (actualDamage <= 0)
            {
                actualDamage = 1;
            }
            Debug.Log("Soy el jugador y he recibido " + actualDamage + " de daño");
            PlayerHP -= actualDamage;
            return actualDamage;
        }
        public void OnAfterDeserialize()
        {
            ResetValues();
        }

        public void OnBeforeSerialize() { }

        private void OnValidate()
        {
            PlayerSpeedOnValueChange?.Invoke();
            PlayerPositionOnValueChange?.Invoke();
        }
        private void OnEnable()
        {
            gameStartBusSO.OnGameStartEvent += ResetValues;
        }
        private void OnDisable()
        {
            gameStartBusSO.OnGameStartEvent -= ResetValues;
        }
        public void ResetValues()
        {
            _runtimeInitialPlayerHP = maxInitPlayer;
            _playerHP = maxInitPlayer;
            _runtimePlayerDamage = playerDamage;
            _runtimePlayerDef = playerDef;
            _runtimePlayerSpeed = playerSpeed;
            _playerExperience = 0;
            _randomRangeStats = 5;
            playerLevel = 1;
            dungeonLevel = 1;
            _isOnEvent = false;
            _numberEnemiesKilled = 0;
        }
    }
}


