using System.Collections;
using System.Collections.Generic;
using HeroesGames.ProjectProcedural.SO;
using TMPro;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.UI
{
    public class UIStorageController : MonoBehaviour
    {
        [SerializeField] private MainMenuBusSO mainMenuBusSO;
        [SerializeField] private PlayerVariableSO playerVariableSO;
        [SerializeField] private TextMeshProUGUI textHPValue;
        [SerializeField] private TextMeshProUGUI textAttackValue;
        [SerializeField] private TextMeshProUGUI textDefenseValue;
        [SerializeField] private TextMeshProUGUI textSpeedValue;

        private void Awake()
        {
            UpdateStats();
        }
        private void OnEnable()
        {
            mainMenuBusSO.OnEquipItemEvent += IncreaseStats;
            mainMenuBusSO.OnUnequipItemEvent += DecreaseStats;
        }
        private void OnDisable()
        {
            mainMenuBusSO.OnEquipItemEvent -= IncreaseStats;
            mainMenuBusSO.OnUnequipItemEvent -= DecreaseStats;
        }
        private void IncreaseStats(ObjectInventoryVariableSO objectInventoryVariableSO)
        {
            if (objectInventoryVariableSO is ArmorVariableSO)
            {
                ArmorVariableSO aux = (ArmorVariableSO)objectInventoryVariableSO;
                playerVariableSO.PlayerHP += aux.ArmorHP;
                playerVariableSO.RuntimePlayerDef += aux.ArmorDefense;
                playerVariableSO.EquippedObjects[objectInventoryVariableSO.PlayerPositionEquipment] = aux;
                UpdateStats();
            }
        }
        private void DecreaseStats(ObjectInventoryVariableSO objectInventoryVariableSO)
        {
            if (objectInventoryVariableSO is ArmorVariableSO)
            {
                ArmorVariableSO aux = (ArmorVariableSO)objectInventoryVariableSO;
                playerVariableSO.PlayerHP -= aux.ArmorHP;
                playerVariableSO.RuntimePlayerDef -= aux.ArmorDefense;
                playerVariableSO.EquippedObjects[objectInventoryVariableSO.PlayerPositionEquipment] = null;
                UpdateStats();
            }
        }
        private void UpdateStats()
        {
            textHPValue.text = "HP:\n" + (playerVariableSO.PlayerHP).ToString();
            textDefenseValue.text = "DEF:\n" + (playerVariableSO.RuntimePlayerDef).ToString();
            textAttackValue.text = "ATK:\n" + (playerVariableSO.RuntimePlayerDamage).ToString();
            textSpeedValue.text = "SPD:\n" + (playerVariableSO.RuntimePlayerSpeed).ToString();
        }

    }
}

