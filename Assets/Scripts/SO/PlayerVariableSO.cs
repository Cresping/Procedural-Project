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
        private const int MAX_EQUIPPED_OBJECTS = 4;
        public Action PlayerPositionOnValueChange;
        public Action PlayerSpeedOnValueChange;
        public Action PlayerLevelOnValueChange;

        [SerializeField] private PlayerInventoryVariableSO playerInventoryVariableSO;
        [SerializeField] private PlayerRecordsVariableSO playerRecordsVariableSO;
        [SerializeField] private DungeonVariableSO BSPDungeonVariableSO;
        [SerializeField] private MazeVariableSO mazeVariableSO;
        [SerializeField] private GameStartBusSO gameStartBusSO;
        [SerializeField] private GameOverBusSO gameOverBusSO;
        [SerializeField] private int playerLevel = 1;
        [SerializeField] private int dungeonLevel = 1;
        [SerializeField] private PlayerController playerPrefab;
        [SerializeField] private int originalPlayerHP = 100;
        [SerializeField] private int playerAttack;
        [SerializeField] private int playerSpeed;
        [SerializeField] private int playerDef;
        [SerializeField] private int minExpLevel = 100;
        private ObjectInventoryVariableSO[] _equippedObjects = new ObjectInventoryVariableSO[MAX_EQUIPPED_OBJECTS];
        private int _numberEnemiesKilled;
        private EnumTypes.StatusType status;

        private PlayerController _instancedPlayer;
        private Vector2 _playerPosition;
        private Vector2 _playerPreviousPosition;
        private Vector2 _playerStartPosition;

        private int _runtimeMaxPlayerHP;
        private int _runtimePlayerHP;
        private int _runtimePlayerAtk;
        private int _runtimePlayerSpd;
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

        public int RuntimePlayerHP
        {
            get => _runtimePlayerHP;
            set
            {
                if (_runtimePlayerHP + value > _runtimeMaxPlayerHP)
                {
                    _runtimePlayerHP = _runtimeMaxPlayerHP;
                }
                else
                {
                    _runtimePlayerHP += value;
                }

                if (_runtimePlayerHP <= 0)
                {
                    _runtimePlayerHP = 0;
                    gameOverBusSO.OnGameOverEvent?.Invoke();
                }
            }
        }

        public int RuntimePlayerDef { get => _runtimePlayerDef; set => _runtimePlayerDef = value; }
        public int RuntimePlayerSpd
        {
            get => _runtimePlayerSpd;
            set
            {
                _runtimePlayerSpd = value;
                PlayerSpeedOnValueChange?.Invoke();
            }
        }
        public int RuntimePlayerAtk { get => _runtimePlayerAtk; set => _runtimePlayerAtk = value; }
        public bool IsOnEvent { get => _isOnEvent; set => _isOnEvent = value; }
        public int PlayerLevel { get => playerLevel; set => playerLevel = value; }
        public int DungeonLevel
        {
            get => dungeonLevel;
            set
            {
                dungeonLevel = value;
                BSPDungeonVariableSO.DungeonLvl = dungeonLevel;
                mazeVariableSO.DungeonLvl = dungeonLevel;
            }
        }
        public int NumberEnemiesKilled { get => _numberEnemiesKilled; set => _numberEnemiesKilled = value; }
        public void EquipObject(int slot, ObjectInventoryVariableSO objectInventoryVariableSO)
        {
            if (slot < MAX_EQUIPPED_OBJECTS && slot >= 0)
            {
                if (objectInventoryVariableSO is ArmorVariableSO)
                {
                    ArmorVariableSO aux = (ArmorVariableSO)objectInventoryVariableSO;
                    _equippedObjects[slot] = aux;
                    _runtimeMaxPlayerHP += aux.ArmorHP;
                    _runtimePlayerHP += aux.ArmorHP;
                    _runtimePlayerDef += aux.ArmorDefense;
                }
                else if (objectInventoryVariableSO is WeaponVariableSO)
                {
                    WeaponVariableSO aux = (WeaponVariableSO)objectInventoryVariableSO;
                    _equippedObjects[slot] = aux;
                    _runtimePlayerAtk += aux.WeaponAttack;
                    _runtimePlayerSpd += aux.WeaponSpeed;
                }
                objectInventoryVariableSO.IsEquiped = true;
            }
        }
        public void UnequipObject(int slot, ObjectInventoryVariableSO objectInventoryVariableSO)
        {
            if (slot < MAX_EQUIPPED_OBJECTS && slot >= 0)
            {
                if (objectInventoryVariableSO is ArmorVariableSO)
                {
                    ArmorVariableSO aux = (ArmorVariableSO)objectInventoryVariableSO;
                    _equippedObjects[slot] = null;
                    _runtimeMaxPlayerHP -= aux.ArmorHP;
                    _runtimePlayerHP -= aux.ArmorHP;
                    _runtimePlayerDef -= aux.ArmorDefense;
                }
                else if (objectInventoryVariableSO is WeaponVariableSO)
                {
                    WeaponVariableSO aux = (WeaponVariableSO)objectInventoryVariableSO;
                    _equippedObjects[slot] = null;
                    _runtimePlayerAtk -= aux.WeaponAttack;
                    _runtimePlayerSpd -= aux.WeaponSpeed;
                }
                objectInventoryVariableSO.PlayerPositionEquipment = -1;
                objectInventoryVariableSO.IsEquiped = false;
            }
        }
        public void ResetEquipObjects()
        {
            foreach (ObjectInventoryVariableSO objectInventory in _equippedObjects)
            {
                if (objectInventory is ArmorVariableSO)
                {
                    ArmorVariableSO aux = (ArmorVariableSO)objectInventory;
                    _runtimeMaxPlayerHP += aux.ArmorHP;
                    _runtimePlayerHP += aux.ArmorHP;
                    _runtimePlayerDef += aux.ArmorDefense;
                }
                else if (objectInventory is WeaponVariableSO)
                {
                    WeaponVariableSO aux = (WeaponVariableSO)objectInventory;
                    _runtimePlayerAtk += aux.WeaponAttack;
                    _runtimePlayerSpd += aux.WeaponSpeed;
                }
            }
        }
        public ObjectInventoryVariableSO[] EquippedObjects { get => _equippedObjects; set => _equippedObjects = value; }

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
                        _runtimePlayerAtk++;
                        break;
                    case 2:
                        _runtimePlayerDef++;
                        break;
                    case 3:
                        _runtimePlayerSpd++;
                        break;
                    case 4:
                        _runtimeMaxPlayerHP += 10;
                        RuntimePlayerHP += 10;
                        _randomRangeStats--;
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
            RuntimePlayerHP = -actualDamage;
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
            _runtimeMaxPlayerHP = originalPlayerHP;
            _runtimePlayerHP = originalPlayerHP;
            _runtimePlayerAtk = playerAttack;
            _runtimePlayerDef = playerDef;
            _runtimePlayerSpd = playerSpeed;
            _playerExperience = 0;
            _randomRangeStats = 5;
            playerLevel = 1;
            dungeonLevel = 1;
            _isOnEvent = false;
            _numberEnemiesKilled = 0;
            ResetEquipObjects();
        }
        public void UpdateRecords()
        {
            if (playerLevel > playerRecordsVariableSO.MaxPlayerLevel)
            {
                playerRecordsVariableSO.MaxPlayerLevel = playerLevel;
            }
            if (dungeonLevel > playerRecordsVariableSO.MaxDungeonLevel)
            {
                playerRecordsVariableSO.MaxDungeonLevel = dungeonLevel;
            }
            if (playerInventoryVariableSO.Inventory.Count > playerRecordsVariableSO.NumberObjectsUnlocked)
            {
                playerRecordsVariableSO.NumberObjectsUnlocked = playerInventoryVariableSO.Inventory.Count;
            }

        }
    }
}


