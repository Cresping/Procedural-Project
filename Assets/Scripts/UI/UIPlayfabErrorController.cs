using HeroesGames.ProjectProcedural.SO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HeroesGames.ProjectProcedural.UI
{
    public class UIPlayfabErrorController : MonoBehaviour
    {
        [SerializeField] private PlayfabBusDataSO playfabBusSO;
        [SerializeField] private PlayerInventoryVariableSO playerInventoryVariableSO;
        [SerializeField] private TextMeshProUGUI errorText;
        [SerializeField] private GameObject errorPanel;
        [SerializeField] private Button tryAgainButton;
        private string _currentError;
        private void Awake()
        {
            errorPanel.SetActive(false);
        }
        private void OnEnable()
        {
            playfabBusSO.OnErrorPlayFab += OnError;
        }
        private void OnDisable()
        {
            playfabBusSO.OnErrorPlayFab -= OnError;
        }
        private void OnError(string errorText)
        {
            tryAgainButton.gameObject.SetActive(true);
            this.errorText.text = "Error " + errorText + ": ";
            _currentError = errorText;
            switch (errorText)
            {
                case "001":
                    this.errorText.text += "An error occurred while trying to connect to the server. You can try to connect again or continue playing but your data will not be stored on the server.";
                    break;
                case "002":
                    this.errorText.text += "An error occurred while trying to update your inventory on the server. You can try again or continue playing but your data will not be updated.";
                    break;
                case "003":
                    this.errorText.text += "An error has occurred while trying to access the table of records. You can try again or exit.";
                    break;
                case "004":
                    this.errorText.text += "An error occurred while trying to load the inventory. You can try again or ignore it but your inventory will not be loaded from the server.";
                    break;
                case "005":
                    this.errorText.text += "An error occurred while trying to update the name. You can try again or ignore it.";
                    break;
                case "006":
                    this.errorText.text += "An error occurred while trying to update the table of records. You can try again or continue playing but these records will be lost.";
                    break;
                case "007":
                    this.errorText.text += "An error occurred while trying to access the records table for the user. You can try again or exit.";
                    break;
                default:
                    this.errorText.text += "An unknown error has occurred.";
                    tryAgainButton.gameObject.SetActive(false);
                    break;
            }
            this.errorPanel.SetActive(true);
        }
        public void DisableWindows()
        {
            this.errorPanel.SetActive(false);
        }
        public void TryAgain()
        {
            this.errorPanel.SetActive(false);
            playfabBusSO.OnReplyErrorPlayFab(_currentError, true);
        }
        public void Ignore()
        {
            this.errorPanel.SetActive(false);
            playfabBusSO.OnReplyErrorPlayFab(_currentError, false);
        }
    }
}

