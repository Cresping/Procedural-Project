using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace HeroesGames.ProjectProcedural.Utils
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class FlashingText : MonoBehaviour
    {
        [SerializeField] private float timeToAppear = 2f;
        private float timerChangeText;
        private TextMeshProUGUI _flashingText;

        private void Awake()
        {
            _flashingText = GetComponent<TextMeshProUGUI>();
            timerChangeText = timeToAppear;
        }
        private void Update()
        {
            timerChangeText -= Time.deltaTime;
            if (timerChangeText <= 0)
            {
                _flashingText.enabled = !_flashingText.enabled;
                timerChangeText = timeToAppear;
            }
        }
    }
}

