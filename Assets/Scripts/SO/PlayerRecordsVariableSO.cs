using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.SO
{
    [CreateAssetMenu(fileName = "NewPlayerRecordsVariable", menuName = "Scriptables/Player/PlayerRecordsVariable")]
    public class PlayerRecordsVariableSO : ScriptableObject
    {
        private int _maxDungeonLevel;
        private int _maxPlayerLevel;
        private int _numberObjectsUnlocked;

        public int MaxDungeonLevel { get => _maxDungeonLevel; set => _maxDungeonLevel = value; }
        public int MaxPlayerLevel { get => _maxPlayerLevel; set => _maxPlayerLevel = value; }
        public int NumberObjectsUnlocked { get => _numberObjectsUnlocked; set => _numberObjectsUnlocked = value; }

        private void OnEnable()
        {
            ResetValues();
        }
        public void ResetValues()
        {
            _maxDungeonLevel = 1;
            _maxPlayerLevel = 1;
            _numberObjectsUnlocked = 0;
        }
    }


}
