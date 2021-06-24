using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.SO
{
    [CreateAssetMenu(fileName = "MainMenuBusSO", menuName = "Scriptables/Bus/MainMenuBus")]
    public class MainMenuBusSO : ScriptableObject
    {
        public Action<ObjectInventoryVariableSO> OnDragItem;
        public Action<ObjectInventoryVariableSO> OnEquipItemEvent;
        public Action<ObjectInventoryVariableSO> OnUnequipItemEvent;
    }
}
