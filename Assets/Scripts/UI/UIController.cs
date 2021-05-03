using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] GameObject pauseMenu;
    private void Awake()
    {
        pauseMenu.SetActive(false);
    }
    public void ButtonUP()
    {
        playerController.TryMoveButton(Vector2.up);
    }
    public void ButtonDOWN()
    {
        playerController.TryMoveButton(Vector2.down);
    }
    public void ButtonLEFT()
    {
        playerController.TryMoveButton(Vector2.left);
    }
    public void ButtonRIGHT()
    {
        playerController.TryMoveButton(Vector2.right);
    }
    public void ButtonPauseMenu(bool value)
    {
        pauseMenu.SetActive(value);
    }

}
