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



        public int ObjectRarity { get => objectRarity; set => objectRarity = value; }
        protected Sprite ObjectSprite { get => objectSprite; set => objectSprite = value; }
        protected string ObjectName { get => objectName; set => objectName = value; }
        public int Id { get => id; set => id = value; }

        public abstract void OnAfterDeserialize();
        public abstract void OnBeforeSerialize();
    }

}

