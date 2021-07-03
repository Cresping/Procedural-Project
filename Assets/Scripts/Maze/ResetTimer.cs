using System.Collections;
using System.Collections.Generic;
using HeroesGames.ProjectProcedural.SO;
using UnityEngine;

public class ResetTimer : MonoBehaviour
{
    [SerializeField] private TimerVariableSO timerVariableSO;
    void Start()
    {
        timerVariableSO.ResetValue();
    }

}
