using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.SO
{
    [CreateAssetMenu(fileName = "PlayfabErrorBusSO", menuName = "Scriptables/Bus/PlayfabErrorBus")]
    public class PlayfabBusSO : ScriptableObject
    {
        public Action OnLogin;
        public Action<List<string>> OnUpdateInventory;
        public Action OnLoadPlayfabInventory;
        public Action OnSucessLogin;
        public Action OnSucessUpdateInventory;
        public Action OnSucessLoadPlayfabInventory;
        public Action<string> OnErrorPlayFab;
    }

}

