using HeroesGames.ProjectProcedural.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.UI
{
    public class UILoginController : MonoBehaviour
    {
        [SerializeField] private PlayfabBusSO playfabBusSO;
        private void OnEnable()
        {
            playfabBusSO.OnSucessLogin += OnLoginSucess;
        }
        private void OnDisable()
        {
            playfabBusSO.OnSucessLogin -= OnLoginSucess;
        }
        public void Login()
        {
            playfabBusSO.OnLogin.Invoke();
        }
        private void OnLoginSucess()
        {
            this.gameObject.SetActive(false);
        }
    }

}
