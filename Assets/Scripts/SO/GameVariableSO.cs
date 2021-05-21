using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.SO
{
     /// <summary>
    /// Clase encargada de controlar las variables generales del juego
    /// </summary>
     [CreateAssetMenu(fileName = "NewGameVariableSO", menuName = "Scriptables/Game/GameVariable")]
    public class GameVariableSO : ScriptableObject
    {
        [SerializeField] private float lerpDuration=0.2f;
        [SerializeField] private bool smoothGameplay = true;
        [SerializeField] private int turnSpeedValue = 5;

        public int TurnSpeedValue { get => turnSpeedValue; set => turnSpeedValue = value; }
        public bool SmoothGameplay { get => smoothGameplay; set => smoothGameplay = value; }
        public float LerpDuration { get => lerpDuration; set => lerpDuration = value; }

    }
}

