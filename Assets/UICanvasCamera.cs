using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.UI
{
    public class UICanvasCamera : MonoBehaviour
    {
        
        void Start()
        {
            GetComponent<Canvas>().worldCamera = Camera.main;
        }

    }

}
