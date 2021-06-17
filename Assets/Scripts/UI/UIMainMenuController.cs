using System.Collections;
using System.Collections.Generic;
using HeroesGames.ProjectProcedural.SO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenuController : MonoBehaviour
{
    [SerializeField] private GameStartBusSO gameStartBusSO;
    private void Awake()
    {
        gameStartBusSO.OnGameStartEvent?.Invoke();
    }
    public void DungeonButton()
    {
        SceneManager.LoadScene("Dungeon", LoadSceneMode.Single);
    }
}
