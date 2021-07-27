using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.SO
{
    [CreateAssetMenu(fileName = "PlayfabBusDataSO", menuName = "Scriptables/Bus/PlayfabBusData")]
    public class PlayfabBusDataSO : ScriptableObject
    {
        public Action OnLogin;
        public Action<List<string>> OnUpdateInventory;
        public Action OnLoadPlayfabInventory;
        public Action OnSucessLogin;
        public Action OnIgnoreLogin;
        public Action OnSucessUpdateInventory;
        public Action OnIgnoreUpdateInventory;
        public Action OnSucessLoadPlayfabInventory;
        public Action OnSucessLoadPlayerLeaderboardRecord;
        public Action<string> OnSucessLoadLeaderboard;
        public Action<string> OnErrorPlayFab;
        public Action<string,bool> OnReplyErrorPlayFab;
    }

}
