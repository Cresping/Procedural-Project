using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyVariable", menuName = "Scriptables/Enemy/EnemyVariable")]
public class EnemyVariableSO : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int maxEnemyHP;
    [SerializeField] private int enemyDefense;
    [SerializeField] private int enemyAttack;
    [SerializeField] private int enemySpeed;
    [SerializeField] private int pursuePlayerDistance;
    [SerializeField] private int attackPlayerDistance;

    private bool _isDead;

    public int EnemyDefense { get => enemyDefense; set => enemyDefense = value; }
    public int EnemyAttack { get => enemyAttack; set => enemyAttack = value; }
    public int EnemySpeed { get => enemySpeed; set => enemySpeed = value; }
    public int PursuePlayerDistance { get => pursuePlayerDistance; set => pursuePlayerDistance = value; }
    public int AttackPlayerDistance { get => attackPlayerDistance; set => attackPlayerDistance = value; }
    public int MaxEnemyHP { get => maxEnemyHP; private set => maxEnemyHP = value; }
    public bool IsDead { get => _isDead; set => _isDead = value; }
    public GameObject EnemyPrefab { get => enemyPrefab; set => enemyPrefab = value; }

    public void OnAfterDeserialize() { }

    public void OnBeforeSerialize() { }

}
