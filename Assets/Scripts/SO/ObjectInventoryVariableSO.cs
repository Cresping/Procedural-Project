using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.SO
{
    public abstract class ObjectInventoryVariableSO : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private int id;
        [SerializeField] private string objectName;

        [SerializeField] private Sprite objectSprite;
        [Range(1, 5)]
        [SerializeField]
        protected int objectRarity;
        private int _playerPositionEquipment;
        private bool _isEquiped;



        public int ObjectRarity { get => objectRarity; set => objectRarity = value; }
        public Sprite ObjectSprite { get => objectSprite; set => objectSprite = value; }
        public string ObjectName { get => objectName; set => objectName = value; }
        public int Id { get => id; set => id = value; }
        public bool IsEquiped { get => _isEquiped; set => _isEquiped = value; }
        public int PlayerPositionEquipment { get => _playerPositionEquipment; set => _playerPositionEquipment = value; }

        public abstract void OnAfterDeserialize();
        public abstract void OnBeforeSerialize();
    }

}

