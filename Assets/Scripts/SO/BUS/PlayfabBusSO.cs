using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.SO
{
    [CreateAssetMenu(fileName = "PlayfabErrorBusSO", menuName = "Scriptables/Bus/PlayfabErrorBus")]
    public class PlayfabBusSO : ScriptableObject
    {
        public Action<string> OnFailedUpdateInventory;
        public Action OnSucessUpdateInventory;
        public Action<List<string>> OnUpdateInventory;
        public Action OnLoadPlayfabInventory;
        public Action<List<string>> OnSucessLoadPlayfabInventory;
        public Action OnFailedLoadPlayfabInventory;
    }

}

