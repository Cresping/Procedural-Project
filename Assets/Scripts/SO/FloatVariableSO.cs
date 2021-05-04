using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.SO
{
    /// <summary>
    ///  Clase encargada de crear un 'ScriptableObject' generico de tipo 'Float'
    /// </summary>
    [CreateAssetMenu(fileName = "NewFloatVariable", menuName = "Scriptables/Variables/FloatVariable")]
    public class FloatVariableSO : ScriptableObject, ISerializationCallbackReceiver
    {
        public Action OnValueChangeCallback;
        [SerializeField] private float initialValue;
        private float _runtimeValue;

        public float InitialValue { get => initialValue; set => initialValue = value; }

        public float RuntimeValue
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

        public void IncreaseValue()
        {
            RuntimeValue++;
        }

        public void DecreaseValue()
        {
            RuntimeValue--;
        }

        public void ResetValue()
        {
            RuntimeValue = initialValue;
        }
    }
}
