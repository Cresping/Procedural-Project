using HeroesGames.ProjectProcedural.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.UI
{
    /// <summary>
    /// Clase encargada de controlar la UI dentro de la mazmorra
    /// </summary>
    public class UIDungeonController : MonoBehaviour
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

}
