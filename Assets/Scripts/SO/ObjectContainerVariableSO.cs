using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.SO
{
    [CreateAssetMenu(fileName = nameof(ObjectContainerVariableSO), menuName = "Scriptables/" + nameof(ObjectContainerVariableSO) + "/" + nameof(ObjectContainerVariableSO) + "Variable")]
    public class ObjectContainerVariableSO : ScriptableObject
    {
        
        private Object[] _objectArray;

        private void OnEnable()
        {
            _objectArray = Resources.LoadAll("SO/Objects", typeof(ObjectContainerVariableSO));
            Debug.Log(_objectArray.Length);
        }
    }
}