using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.SO
{

    [CreateAssetMenu(fileName = "NewTimerVariable", menuName = "Scriptables/Timer/TimerVariable")]
    public class TimerVariableSO : ScriptableObject
    {
        private const float MAX_TIME_TIMER = 99999999999999.0f;
        [SerializeField] private GameStartBusSO gameStartBusSO;
        [SerializeField] private GameOverBusSO gameOverBusSO;
        private float _timeSeconds;

        public float TimeSeconds
        {
            get => _timeSeconds;
            set
            {
                _timeSeconds = value;
                if (_timeSeconds <= 0)
                {
                    gameOverBusSO.OnGameOverEvent?.Invoke();
                }
            }
        }
        public void AddTime(float time)
        {
            if (_timeSeconds + time > MAX_TIME_TIMER)
            {
                _timeSeconds = MAX_TIME_TIMER;
            }
            else
            {
                _timeSeconds += time;
            }
        }
        private void OnEnable()
        {
            ResetValue();
            gameStartBusSO.OnGameStartEvent += ResetValue;
        }
        private void OnDisable()
        {
            gameStartBusSO.OnGameStartEvent -= ResetValue;
        }
        public void ResetValue()
        {
            _timeSeconds = MAX_TIME_TIMER;
        }
    }

}
