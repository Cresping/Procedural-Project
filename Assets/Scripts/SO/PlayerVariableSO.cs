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
        public Action PlayerHPOnValueChange;
        public Action PlayerPositionOnValueChange;
        public Action PlayerSpeedOnValueChange;

        [SerializeField] private PlayerController playerPrefab;
        [SerializeField] private int maxPlayerHP = 100;
        [SerializeField] private int playerDamage;
        [SerializeField] private int playerSpeed;
        [SerializeField] private int playerDef;
        private EnumTypes.StatusType status;

        private PlayerController _instancedPlayer;
        private Vector2 _playerPosition;
        private Vector2 _playerPreviousPosition;
        private Vector2 _playerStartPosition;
        private int _playerHP;
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
        public int PlayerDef { get => playerDef; set => playerDef = value; }
        public int PlayerSpeed
        {
            get => playerSpeed;
            set
            {
                playerSpeed = value;
                PlayerSpeedOnValueChange?.Invoke();
            }
        }
        public int PlayerHP
        {
            get => _playerHP;
            set
            {
                if (value > maxPlayerHP)
                {
                    _playerHP = maxPlayerHP;
                }
                else
                {
                    _playerHP = value;
                }
                PlayerHPOnValueChange?.Invoke();
            }
        }
        public int PlayerDamage
        {
            get => playerDamage;
            set => playerDamage = value;
        }
        public void ReceiveDamage(int damage)
        {
            int actualDamage = damage - playerDef;
            Debug.Log("Soy el jugador y he recibido "+actualDamage+" de daño");
            if(actualDamage>0)
            {
                PlayerHP -= actualDamage; 
            }
        }
        public void OnAfterDeserialize()
        {
            _playerHP = maxPlayerHP;
        }

        public void OnBeforeSerialize() { }

        private void OnValidate()
        {
            PlayerHPOnValueChange?.Invoke();
            PlayerSpeedOnValueChange?.Invoke();
            PlayerPositionOnValueChange?.Invoke();
        }
        public void ResetValue()
        {
            _playerHP = maxPlayerHP;
        }
    }
}


