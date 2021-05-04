using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.SO
{
    /// <summary>
    ///  Clase encargada de crear un 'ScriptableObject' generico de tipo 'Bool'
    /// </summary>
    [CreateAssetMenu(fileName = "NewBoolVariable", menuName = "Scriptables/Variables/BoolVariable")]
    public class BoolVariableSO : ScriptableObject, ISerializationCallbackReceiver
    {
        public Action OnValueChangeCallback;
        [SerializeField] private bool initialValue;
        private bool _runtimeValue;

        public bool InitialValue { get => initialValue; set => initialValue = value; }

        public bool RuntimeValue
        {
            get => _runtimeValue;
            set
            {
                _runtimeValue = value;
                OnValueChangeCallback?.Invoke();
            }
        }

        public void OnAfterDeserialize()
        {
            _runtimeValue = initialValue;
        }

        public void OnBeforeSerialize() { }


        private void OnValidate()
        {
            OnValueChangeCallback?.Invoke();
        }

        public void ResetValue()
        {
            RuntimeValue = initialValue;
        }
    }
}
