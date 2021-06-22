using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.SO
{
    public abstract class ObjectInventoryVariableSO : ScriptableObject, ISerializationCallbackReceiver
    {
        private int _id;
        [SerializeField] private string objectName;

        [SerializeField] private Sprite objectSprite;
        [Range(1, 5)]
        [SerializeField]
        protected int objectRarity = 1;
        private int _playerPositionEquipment;
        private bool _isEquiped;



        public int ObjectRarity { get => objectRarity; set => objectRarity = value; }
        public Sprite ObjectSprite { get => objectSprite; set => objectSprite = value; }
        public string ObjectName { get => objectName; set => objectName = value; }
        public int Id { get => _id; set => _id = value; }
        public bool IsEquiped { get => _isEquiped; set => _isEquiped = value; }
        public int PlayerPositionEquipment { get => _playerPositionEquipment; set => _playerPositionEquipment = value; }

        public virtual void OnAfterDeserialize()
        {
            this._id = GetInstanceID();
        }
        public virtual void OnBeforeSerialize()
        {

        }
    }

}

