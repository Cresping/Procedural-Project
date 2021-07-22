using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace HeroesGames.ProjectProcedural.SO
{
    [CreateAssetMenu(fileName = nameof(ObjectContainerVariableSO), menuName = "Scriptables/" + nameof(ObjectContainerVariableSO) + "/" + nameof(ObjectContainerVariableSO) + "Variable")]
    public class ObjectContainerVariableSO : ScriptableObject
    {
        private Dictionary<int, ObjectInventoryVariableSO> _dictionaryAllObjects;
        private Dictionary<int, ObjectInventoryVariableSO> _dictionaryOneStartObjects;
        private Dictionary<int, ObjectInventoryVariableSO> _dictionaryTwoStartObjects;
        private Dictionary<int, ObjectInventoryVariableSO> _dictionaryThreeStartObjects;
        private Dictionary<int, ObjectInventoryVariableSO> _dictionaryFourStartObjects;
        private Dictionary<int, ObjectInventoryVariableSO> _dictionaryFiveStartObjects;

        private void OnEnable()
        {
            ObjectInventoryVariableSO[] objectArray;
            ResetDictionaries();
            objectArray = Resources.LoadAll<ObjectInventoryVariableSO>("SO/Objects");
            foreach (ObjectInventoryVariableSO objectInventory in objectArray)
            {
                if (!_dictionaryAllObjects.ContainsKey(objectInventory.Id)) 
                {
                    _dictionaryAllObjects.Add(objectInventory.Id, objectInventory);
                }
                switch (objectInventory.ObjectRarity)
                {
                    case 1:
                        if (!CheckRepeated(_dictionaryOneStartObjects, objectInventory))
                        {
                            _dictionaryOneStartObjects.Add(objectInventory.Id, objectInventory);
                        }                     
                        break;
                    case 2:
                        if (!CheckRepeated(_dictionaryTwoStartObjects, objectInventory))
                        {
                            _dictionaryTwoStartObjects.Add(objectInventory.Id, objectInventory);
                        }
                        break;
                    case 3:
                        if (!CheckRepeated(_dictionaryThreeStartObjects, objectInventory))
                        {
                            _dictionaryThreeStartObjects.Add(objectInventory.Id, objectInventory);
                        }
                        break;
                    case 4:
                        if (!CheckRepeated(_dictionaryFourStartObjects, objectInventory))
                        {
                            _dictionaryFourStartObjects.Add(objectInventory.Id, objectInventory);
                        }
                        break;
                    case 5:
                        if (!CheckRepeated(_dictionaryFiveStartObjects, objectInventory))
                        {
                            _dictionaryFiveStartObjects.Add(objectInventory.Id, objectInventory);
                        }
                        break;
                    default:
                        Debug.LogError("El objeto tiene una rareza no controlada");
                        break;
                }
            }
            Debug.Log("Hay un total de " + _dictionaryAllObjects.Values.Count + " objetos");
        }
        public ObjectInventoryVariableSO PickRandomItem(int rarity)
        {
            ObjectInventoryVariableSO aux = null;
            switch (rarity)
            {
                case 1:
                    if (_dictionaryOneStartObjects.Count > 0)
                    {
                        List<int> keys = _dictionaryOneStartObjects.Keys.ToList<int>();
                        _dictionaryOneStartObjects.TryGetValue(keys[UnityEngine.Random.Range(0, keys.Count)], out aux);
                    }
                    break;
                case 2:
                    if (_dictionaryTwoStartObjects.Count > 0)
                    {
                        List<int> keys = _dictionaryTwoStartObjects.Keys.ToList<int>();
                        _dictionaryTwoStartObjects.TryGetValue(keys[UnityEngine.Random.Range(0, keys.Count)], out aux);
                    }
                    break;
                case 3:
                    if (_dictionaryThreeStartObjects.Count > 0)
                    {
                        List<int> keys = _dictionaryThreeStartObjects.Keys.ToList<int>();
                        _dictionaryThreeStartObjects.TryGetValue(keys[UnityEngine.Random.Range(0, keys.Count)], out aux);
                    }
                    break;
                case 4:
                    if (_dictionaryFourStartObjects.Count > 0)
                    {
                        List<int> keys = _dictionaryFourStartObjects.Keys.ToList<int>();
                        _dictionaryFourStartObjects.TryGetValue(keys[UnityEngine.Random.Range(0, keys.Count)], out aux);
                    }
                    break;
                case 5:
                    if (_dictionaryFiveStartObjects.Count > 0)
                    {
                        List<int> keys = _dictionaryFiveStartObjects.Keys.ToList<int>();
                        _dictionaryFiveStartObjects.TryGetValue(keys[UnityEngine.Random.Range(0, keys.Count)], out aux);
                    }
                    break;
                default:
                    Debug.LogError("La rareza pedida no está controlada");
                    break;
            }
            return aux;
        }
        public ObjectInventoryVariableSO GetObjectInventory(string id)
        {
            try
            {
                int parse = Int32.Parse(id);
                if (_dictionaryAllObjects.ContainsKey(parse))
                {
                    return _dictionaryAllObjects[parse];
                }
                else
                {
                    return null;
                }
            }
            catch (FormatException)
            {
                Debug.LogError("El ID " + id + " no es un entero");
                return null;
            }

        }
        private void ResetDictionaries()
        {
            _dictionaryAllObjects = new Dictionary<int, ObjectInventoryVariableSO>();
            _dictionaryOneStartObjects = new Dictionary<int, ObjectInventoryVariableSO>();
            _dictionaryTwoStartObjects = new Dictionary<int, ObjectInventoryVariableSO>();
            _dictionaryThreeStartObjects = new Dictionary<int, ObjectInventoryVariableSO>();
            _dictionaryFourStartObjects = new Dictionary<int, ObjectInventoryVariableSO>();
            _dictionaryFiveStartObjects = new Dictionary<int, ObjectInventoryVariableSO>();
        }
        private bool CheckRepeated(Dictionary<int, ObjectInventoryVariableSO> dictionaryObjectInventory, ObjectInventoryVariableSO value)
        {
            return dictionaryObjectInventory.ContainsKey(value.Id);
        }
    }
}