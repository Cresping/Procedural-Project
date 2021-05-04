using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.SO
{
    /// <summary>
    ///  Clase encargada de crear un 'ScriptableObject' generico de tipo 'String'
    /// </summary>
    [CreateAssetMenu(fileName = "NewStringVariable", menuName = "Scriptables/Variables/StringVariable")]
    public class StringVariableSO : ScriptableObject, ISerializationCallbackReceiver
    {
        public Action OnValueChangeCallback;
        [SerializeField] private string initialValue;
        private string _runtimeValue;

        public string InitialValue { get => initialValue; set => initialValue = value; }

        public string RuntimeValue
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
