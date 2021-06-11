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
        [SerializeField] private int difficultyParameter=1;
        [SerializeField] private float timeBetweenKeystrokes=0.1f;
        [SerializeField] private float lerpDuration=0.2f;
        [SerializeField] private bool smoothGameplay = true;
        [SerializeField] private int turnSpeedValue = 5;
        [SerializeField] private int maxLevelEasy =5;
        [SerializeField] private int maxLevelNormal = 10;
        [SerializeField] private int maxLevelHard = 15;
        [SerializeField] private int maxLevelVeryHard = 20;

        public int TurnSpeedValue { get => turnSpeedValue; set => turnSpeedValue = value; }
        public bool SmoothGameplay { get => smoothGameplay; set => smoothGameplay = value; }
        public float LerpDuration { get => lerpDuration; set => lerpDuration = value; }
        public float TimeBetweenKeystrokes { get => timeBetweenKeystrokes; set => timeBetweenKeystrokes = value; }
        public int MaxLevelEasy { get => maxLevelEasy; set => maxLevelEasy = value; }
        public int MaxLevelNormal { get => maxLevelNormal; set => maxLevelNormal = value; }
        public int MaxLevelHard { get => maxLevelHard; set => maxLevelHard = value; }
        public int MaxLevelVeryHard { get => maxLevelVeryHard; set => maxLevelVeryHard = value; }
        public int DifficultyParameter { get => difficultyParameter; set => difficultyParameter = value; }
    }
}

