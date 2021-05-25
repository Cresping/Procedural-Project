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
        [SerializeField] CombatVariableSO combatVariableSO;
        [SerializeField] PlayerController playerController;
        [SerializeField] PlayerCombatController playerCombatController;
        [SerializeField] GameObject pauseMenu;

        [SerializeField] GameObject movementButtons;

        [SerializeField] GameObject combatButtons;

        [SerializeField] GameObject combatUI;

        private void Awake()
        {
            pauseMenu.SetActive(false);
            combatVariableSO.OnCombatActivation += SwitchCombatInterface;
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
        public void ButtonAttack()
        {
            playerCombatController.DoDamageEnemy();
        }
        public void SwitchCombatInterface()
        {
            if (combatVariableSO.IsActive)
            {
                movementButtons.SetActive(false);
                combatButtons.SetActive(true);
                combatUI.SetActive(true);
            }
            else
            {
                combatButtons.SetActive(false);
                combatUI.SetActive(false);
                movementButtons.SetActive(true);
            }
        }
    }

}
