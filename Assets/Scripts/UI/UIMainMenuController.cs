using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenuController : MonoBehaviour
{
    public void DungeonButton()
    {
         SceneManager.LoadScene("Dungeon", LoadSceneMode.Single);
    }
}
