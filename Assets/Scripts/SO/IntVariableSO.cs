using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.SO
{
    /// <summary>
    ///  Clase encargada de crear un 'ScriptableObject' generico de tipo 'Int'
    /// </summary>
    [CreateAssetMenu(fileName = "NewIntVariable", menuName = "Scriptables/Variables/IntVariable")]
    public class IntVariableSO : ScriptableObject, ISerializationCallbackReceiver
    {
        public Action OnValueChangeCallback;
        [SerializeField] private int initialValue;
        private int _runtimeValue;

        public int InitialValue { get => initialValue; set => initialValue = value; }

        public int RuntimeValue
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
            if (RuntimeValue == 6)
            {
                ResetValue();
            }
            else
            {
                RuntimeValue++;
            }
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
