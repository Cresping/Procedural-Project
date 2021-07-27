using System.Collections;
using System.Collections.Generic;
using HeroesGames.ProjectProcedural.SO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenuController : MonoBehaviour
{
    [SerializeField] private LoginManagerSO loginManagerSO;
    [SerializeField] private PlayfabBusDataSO playfabBusSO;
    [SerializeField] private PlayerRecordsVariableSO playerRecordsVariableSO;
    [SerializeField] private PlayerVariableSO playerVariableSO;
    [SerializeField] private TextMeshProUGUI valueMaxLevelPlayer;
    [SerializeField] private TextMeshProUGUI valueMaxDungeon;
    [SerializeField] private TextMeshProUGUI valueCollectedItems;
    [SerializeField] private GameStartBusSO gameStartBusSO;
    [SerializeField] private GameObject storage;
    [SerializeField] private GameObject mainScreen;
    private void Start()
    {
        UpdateRecords();
        gameStartBusSO.OnGameStartEvent?.Invoke();
        if(loginManagerSO.IsAlreadyLogged)
        {
            mainScreen.SetActive(false);
        }
    }
    private void OnEnable()
    {
        playfabBusSO.OnSucessLoadPlayfabInventory += UpdateRecords;
        playfabBusSO.OnSucessLoadPlayerLeaderboardRecord += UpdateMaxDungeonRecord;
    }
    private void OnDisable()
    {
        playfabBusSO.OnSucessLoadPlayfabInventory -= UpdateRecords;
        playfabBusSO.OnSucessLoadPlayerLeaderboardRecord -= UpdateMaxDungeonRecord;
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
        playerVariableSO.UpdateRecords();
        valueMaxLevelPlayer.text = playerRecordsVariableSO.MaxPlayerLevel.ToString();
        valueMaxDungeon.text = playerRecordsVariableSO.MaxDungeonLevel.ToString();
        valueCollectedItems.text = playerRecordsVariableSO.NumberObjectsUnlocked.ToString() + "/38";
        playerVariableSO.ResetValues();
    }
    public void UpdateMaxDungeonRecord()
    {
         valueMaxDungeon.text = playerRecordsVariableSO.MaxDungeonLevel.ToString();
    }
}
