using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HeroesGames.ProjectProcedural.SO
{
    [CreateAssetMenu(fileName = "GameOverBusSO", menuName = "Scriptables/Bus/GameOverBus")]
    public class GameOverBusSO : ScriptableObject
    {
        public Action OnGameOverEvent;
    }

}
