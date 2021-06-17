using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.SO
{

    [CreateAssetMenu(fileName = "GameStartBusSO", menuName = "Scriptables/Bus/GameStartBus")]
    public class GameStartBusSO : ScriptableObject
    {
        public Action OnGameStartEvent;
    }
}

