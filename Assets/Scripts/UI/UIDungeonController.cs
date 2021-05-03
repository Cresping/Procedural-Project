using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDungeonController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] GameObject pauseMenu;

    public void ChangePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }
}
