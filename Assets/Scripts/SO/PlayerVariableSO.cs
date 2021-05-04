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

        [SerializeField] private int maxPlayerHP = 100;
        [SerializeField] private int _playerDamage;
        [SerializeField] private int _playerSpeed;
        [SerializeField] private int _playerDef;
        private EnumTypes.StatusType status;
        private Vector2 _playerPosition;
        private Vector2 _playerPreviousPosition;
        private Vector2 _playerStartPosition;
        private int _playerHP;

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
        public int PlayerDef { get => _playerDef; set => _playerDef = value; }
        public int PlayerSpeed
        {
            get => _playerSpeed;
            set
            {
                _playerSpeed = value;
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
            get => _playerDamage;
            set => _playerDamage = value;
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
 

