using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.SO
{
    public abstract class DungeonVariableSO : ScriptableObject
    {
        public Action OnGenerateDungeonEvent;
        [SerializeField] protected int dungeonLvl;
        [SerializeField] protected int dungeonWidth = 20, dungeonHeight=20;

        public int DungeonHeight { get => dungeonHeight; set => dungeonHeight = value; }
        public int DungeonWidth { get => dungeonWidth; set => dungeonWidth = value; }
        public int DungeonLvl { get => dungeonLvl; set => dungeonLvl = value; }

        public abstract void CalculateDifficulty();
    }
}

