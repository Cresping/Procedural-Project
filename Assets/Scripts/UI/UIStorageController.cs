using System.Collections;
using System.Collections.Generic;
using HeroesGames.ProjectProcedural.SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HeroesGames.ProjectProcedural.UI
{
    public class UIStorageController : MonoBehaviour
    {
        [SerializeField] private MainMenuBusSO mainMenuBusSO;
        [SerializeField] private PlayerVariableSO playerVariableSO;
        [SerializeField] private PlayerInventoryVariableSO playerInventoryVariableSO;
        [SerializeField] private RectTransform content;
        [SerializeField] private RectTransform slot1;
        [SerializeField] private RectTransform slot2;
        [SerializeField] private RectTransform slot3;
        [SerializeField] private RectTransform slot4;
        [SerializeField] private GameObject prefabContentObject;

        [SerializeField] private TextMeshProUGUI textHPValue;
        [SerializeField] private TextMeshProUGUI textAttackValue;
        [SerializeField] private TextMeshProUGUI textDefenseValue;
        [SerializeField] private TextMeshProUGUI textSpeedValue;

        [SerializeField] private GameObject objectInfo;
        [SerializeField] private Image objectImageInfo;
        [SerializeField] private TextMeshProUGUI objectNameInfo;
        [SerializeField] private TextMeshProUGUI objectStatValue1;
        [SerializeField] private TextMeshProUGUI objectStatValue2;
        [SerializeField] private List<Image> objectStars;
        [SerializeField] private Sprite lockedStarSprite;
        [SerializeField] private Sprite unlockedStarSprite;

        private void Awake()
        {
            LoadStorage();
            ResetObjectInfo();
            UpdateStats();
        }
        private void OnEnable()
        {
            mainMenuBusSO.OnEquipItemEvent += DoIncreaseStats;
            mainMenuBusSO.OnUnequipItemEvent += DoDecreaseStats;
            mainMenuBusSO.OnDragItem += DoUpdateObjectInfo;
        }
        private void OnDisable()
        {
            mainMenuBusSO.OnEquipItemEvent -= DoIncreaseStats;
            mainMenuBusSO.OnUnequipItemEvent -= DoDecreaseStats;
            mainMenuBusSO.OnDragItem -= DoUpdateObjectInfo;
        }
        private void DoUpdateObjectInfo(ObjectInventoryVariableSO objectInventoryVariableSO)
        {
            ResetObjectInfo();
            int cont = 0;
            objectImageInfo.sprite = objectInventoryVariableSO.ObjectSprite;
            foreach (var star in objectStars)
            {
                if (cont < objectInventoryVariableSO.ObjectRarity)
                {
                    star.sprite = unlockedStarSprite;
                }
                else
                {
                    star.sprite = lockedStarSprite;
                }
                cont++;
            }
            objectNameInfo.text = objectInventoryVariableSO.ObjectName;
            if (objectInventoryVariableSO is ArmorVariableSO)
            {
                ArmorVariableSO aux = (ArmorVariableSO)objectInventoryVariableSO;
                objectStatValue1.text = "HP:" + aux.ArmorHP;
                objectStatValue2.text = "DEF:" + aux.ArmorDefense;
            }
            else if (objectInventoryVariableSO is WeaponVariableSO)
            {
                WeaponVariableSO aux = (WeaponVariableSO)objectInventoryVariableSO;
                objectStatValue1.text = "ATK:" + aux.WeaponAttack;
                objectStatValue2.text = "SPD:" + aux.WeaponSpeed;
            }
            objectInfo.SetActive(true);
        }
        private void ResetObjectInfo()
        {
            objectInfo.SetActive(false);
            objectImageInfo.sprite = null;
            foreach (var star in objectStars)
            {
                star.sprite = null;
            }
            objectNameInfo.text = null;
            objectStatValue1.text = null;
            objectStatValue2.text = null;
        }
        private void DoIncreaseStats(ObjectInventoryVariableSO objectInventoryVariableSO)
        {
            playerVariableSO.EquipObject(objectInventoryVariableSO.PlayerPositionEquipment, objectInventoryVariableSO);
            UpdateStats();
        }
        private void DoDecreaseStats(ObjectInventoryVariableSO objectInventoryVariableSO)
        {

            playerVariableSO.UnequipObject(objectInventoryVariableSO.PlayerPositionEquipment, objectInventoryVariableSO);
            UpdateStats();
        }
        private void UpdateStats()
        {
            textHPValue.text = "HP:\n" + (playerVariableSO.RuntimePlayerHP).ToString();
            textDefenseValue.text = "DEF:\n" + (playerVariableSO.RuntimePlayerDef).ToString();
            textAttackValue.text = "ATK:\n" + (playerVariableSO.RuntimePlayerAtk).ToString();
            textSpeedValue.text = "SPD:\n" + (playerVariableSO.RuntimePlayerSpd).ToString();
        }
        private void LoadStorage()
        {
            UIDraggable aux;
            if (prefabContentObject.TryGetComponent<UIDraggable>(out aux))
            {
                int childsContent = content.childCount;
                for (int i = childsContent - 1; i >= 0; i--)
                {
                    GameObject.DestroyImmediate(content.GetChild(i).gameObject);
                }
                foreach (ObjectInventoryVariableSO objectInventory in playerInventoryVariableSO.Inventory.Values)
                {
                    if (!objectInventory.IsEquiped)
                    {
                        GameObject newContentObject = new GameObject();
                        newContentObject = prefabContentObject;
                        UIDraggable draggableObject = newContentObject.GetComponent<UIDraggable>();
                        draggableObject.ObjectInventoryVariableSO = objectInventory;
                        Instantiate(draggableObject, content);
                    }
                    else
                    {
                        GameObject newSlotObject = new GameObject();
                        newSlotObject = prefabContentObject;
                        UIDraggable draggableObject = newSlotObject.GetComponent<UIDraggable>();
                        draggableObject.ObjectInventoryVariableSO = objectInventory;
                        switch (objectInventory.PlayerPositionEquipment)
                        {
                            case 0:
                                Instantiate(draggableObject, slot1);
                                mainMenuBusSO.OnEquipItemEvent?.Invoke(objectInventory);
                                slot1.GetComponent<UIDropZone>().IsTaken = true;
                                slot1.GetComponent<UIDropZone>().CurrentObjectInventoryVariableSO = objectInventory;
                                break;
                            case 1:
                                Instantiate(draggableObject, slot2);
                                mainMenuBusSO.OnEquipItemEvent?.Invoke(objectInventory);
                                slot2.GetComponent<UIDropZone>().IsTaken = true;
                                slot2.GetComponent<UIDropZone>().CurrentObjectInventoryVariableSO = objectInventory;
                                break;
                            case 2:
                                Instantiate(draggableObject, slot3);
                                mainMenuBusSO.OnEquipItemEvent?.Invoke(objectInventory);
                                slot3.GetComponent<UIDropZone>().IsTaken = true;
                                slot3.GetComponent<UIDropZone>().CurrentObjectInventoryVariableSO = objectInventory;
                                break;
                            case 3:
                                Instantiate(draggableObject, slot4);
                                mainMenuBusSO.OnEquipItemEvent?.Invoke(objectInventory);
                                slot4.GetComponent<UIDropZone>().IsTaken = true;
                                slot4.GetComponent<UIDropZone>().CurrentObjectInventoryVariableSO = objectInventory;
                                break;
                            default:
                                Debug.LogError("Error al cargar los objetos equipados");
                                break;
                        }
                    }
                }
            }
            else
            {
                Debug.LogError("El prefab de ContentObject no contiene el script para ser selecionado");
            }
        }

    }
}

