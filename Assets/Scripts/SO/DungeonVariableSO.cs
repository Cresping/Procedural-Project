using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.SO
{
    public abstract class DungeonVariableSO : ScriptableObject
    {
        public Action OnGenerateDungeonEvent;

        [SerializeField] protected GameStartBusSO gameStartBusSO;
        [SerializeField] protected int dungeonLvl;
        [SerializeField] protected int dungeonWidth = 20, dungeonHeight = 20;

        public int DungeonHeight { get => dungeonHeight; set => dungeonHeight = value; }
        public int DungeonWidth { get => dungeonWidth; set => dungeonWidth = value; }
        public int DungeonLvl { get => dungeonLvl; set => dungeonLvl = value; }
        protected virtual void OnEnable()
        {
            gameStartBusSO.OnGameStartEvent += ResetValues;
        }
        protected virtual void OnDisable()
        {
            gameStartBusSO.OnGameStartEvent -= ResetValues;
        }
        protected virtual void ResetValues()
        {
            dungeonLvl = 1;
        }
        public abstract void CalculateDifficulty();
    }
}

