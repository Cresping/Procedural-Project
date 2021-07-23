using HeroesGames.ProjectProcedural.SO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace HeroesGames.ProjectProcedural.UI
{
    public class UIPlayfabErrorController : MonoBehaviour
    {
        [SerializeField] private PlayfabBusDataSO playfabBusSO;
        [SerializeField] private PlayerInventoryVariableSO playerInventoryVariableSO;
        [SerializeField] private TextMeshProUGUI errorText;
        [SerializeField] private GameObject errorPanel;

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
            this.errorText.text = errorText;
            this.errorPanel.SetActive(true);
        }
        public void DisableWindows()
        {
            this.errorPanel.SetActive(false);
        }
        public void TryAgainUpdateInventory()
        {
            this.errorPanel.SetActive(false);
            playerInventoryVariableSO.UpdateInventory();
        }
    }
}

