using System.Collections;
using System.Collections.Generic;
using HeroesGames.ProjectProcedural.SO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenuController : MonoBehaviour
{
    [SerializeField] private GameStartBusSO gameStartBusSO;
    [SerializeField] private GameObject storage;
    private void Awake()
    {
        gameStartBusSO.OnGameStartEvent?.Invoke();
    }
    private void Start()
    {
        storage.SetActive(false);
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
}
