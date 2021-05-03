using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRoomVariable", menuName = "Scriptables/Room/RoomVariable")]
public class RoomVariableSO : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] RoomTypes.RoomType roomType;
    [SerializeField] List<EnemyVariableSO> enemyTypeList;

    private List<GameObject> enemyList;
    public RoomTypes.RoomType RoomType { get => roomType; private set => roomType = value; }



    public void InstantiateAllEnemies(Transform parent)
    {
        enemyList = new List<GameObject>();
        int index = 0;
        foreach (var enemy in enemyTypeList)
        {
            enemyList.Add(Instantiate(enemy.EnemyPrefab, parent));
            enemyList[index].SetActive(false);
            index++;
        }
    }
    public void EnableAllEnemies(List<Vector2Int> positions)
    {
        int index = 0;
        try
        {
            foreach (var enemy in enemyList)
            {
                enemy.transform.position = (Vector3Int)positions[index];
                enemy.SetActive(true);
                index++;
            }
        }
        catch (Exception) { }
    }
    public void DisableAllEnemies()
    {
        foreach (var enemy in enemyList)
        {
            enemy.SetActive(false);
        }
    }
    public int NumberOfEnemies()
    {
        return enemyTypeList.Count;
    }
    public void OnAfterDeserialize() { }

    public void OnBeforeSerialize() { }
}
