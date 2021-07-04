using System.Collections;
using System.Collections.Generic;
using HeroesGames.ProjectProcedural.SO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenuController : MonoBehaviour
{
    [SerializeField] private PlayerRecordsVariableSO playerRecordsVariableSO;
    [SerializeField] private PlayerVariableSO playerVariableSO;
    [SerializeField] private TextMeshProUGUI valueMaxLevelPlayer;
    [SerializeField] private TextMeshProUGUI valueMaxDungeon;
    [SerializeField] private TextMeshProUGUI valueCollectedItems;
    [SerializeField] private GameStartBusSO gameStartBusSO;
    [SerializeField] private GameObject storage;
    private void Start()
    {
        playerVariableSO.UpdateRecords();
        UpdateRecords();
        playerVariableSO.ResetValues();
        gameStartBusSO.OnGameStartEvent?.Invoke();
    }
    public void DungeonButton()
    {
        SceneManager.LoadScene("Dungeon", LoadSceneMode.Single);
    }
    public void EnableStorageButton()
    {
        storage.SetActive(true);

    }
    public void DisableStorageButton()
    {
        storage.SetActive(false);
    }
    public void UpdateRecords()
    {
        valueMaxLevelPlayer.text = playerRecordsVariableSO.MaxPlayerLevel.ToString();
        valueMaxDungeon.text = playerRecordsVariableSO.MaxDungeonLevel.ToString();
        valueCollectedItems.text = playerRecordsVariableSO.NumberObjectsUnlocked.ToString() + "/38";
    }
}
