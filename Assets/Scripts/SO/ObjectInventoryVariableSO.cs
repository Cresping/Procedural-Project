using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.SO
{
    public abstract class ObjectInventoryVariableSO : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private int _id=0;
        [SerializeField] private string objectName;

        [SerializeField] private Sprite objectSprite;
        [Range(1, 5)]
        [SerializeField]
        protected int objectRarity = 1;
        private int _playerPositionEquipment;
        private bool _isEquiped=false;

        public int ObjectRarity { get => objectRarity; set => objectRarity = value; }
        public Sprite ObjectSprite { get => objectSprite; set => objectSprite = value; }
        public string ObjectName { get => objectName; set => objectName = value; }
        public int Id { get => _id; set => _id = value; }
        public int PlayerPositionEquipment { get => _playerPositionEquipment; set => _playerPositionEquipment = value; }
        public bool IsEquiped { get => _isEquiped; set => _isEquiped = value; }

        public virtual void OnAfterDeserialize()
        {
            if(this._id==0)
            {
                this._id = GetInstanceID();
            }
            _isEquiped = false;
            _playerPositionEquipment = -1;
        }
        public virtual void OnBeforeSerialize()
        {

        }
    }

}

