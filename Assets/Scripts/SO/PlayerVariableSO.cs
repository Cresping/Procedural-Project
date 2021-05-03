using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewPlayerVariable", menuName = "Scriptables/Player/PlayerVariable")]
public class PlayerVariableSO : ScriptableObject, ISerializationCallbackReceiver
{
    public Action PlayerHPOnValueChange;
    public Action PlayerPositionOnValueChange;

    public Action PlayerSpeedOnValueChange;

    [SerializeField] private int maxPlayerHP = 100;
    [SerializeField] private float invincibilityTime = 1f;
    private int _playerHP;
    [SerializeField] private int _playerDamage;
    [SerializeField] private int _playerSpeed;
    [SerializeField] private int _playerDef;
    private StatusTypes.StatusType status;
    private Vector2 _playerPosition;
    private Vector2 _playerPreviousPosition;
    private Vector2 _playerStartPosition;
    private bool _isInvencible;
    private bool _isMoving;
    private bool _isAttacking;

    public int PlayerHP
    {
        get => _playerHP;
        set
        {
            if (!_isInvencible)
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
    }
    public int PlayerDamage
    {
        get => _playerDamage;
        set => _playerDamage = value;
    }
    public bool IsInvencible { get => _isInvencible; set => _isInvencible = value; }
    public float InvincibilityTime { get => invincibilityTime; set => invincibilityTime = value; }
    public bool IsMoving { get => _isMoving; set => _isMoving = value; }
    public bool IsAttacking { get => _isAttacking; set => _isAttacking = value; }
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
    public StatusTypes.StatusType Status { get => status; set => status = value; }
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

    public Vector2 PlayerPreviousPosition { get => _playerPreviousPosition; set => _playerPreviousPosition = value; }

    public void OnAfterDeserialize()
    {
        _playerHP = maxPlayerHP;
        _isInvencible = false;
        _isMoving = false;
        _isAttacking = false;
    }

    public void OnBeforeSerialize() { }


    private void OnValidate()
    {
        PlayerHPOnValueChange?.Invoke();
        PlayerSpeedOnValueChange?.Invoke();
    }

    public void ResetValue()
    {
        _playerHP = maxPlayerHP;
        _isInvencible = false;
        _isMoving = false;
        _isAttacking = false;
    }
}

