using HeroesGames.ProjectProcedural.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.UI
{
    public class UILoginController : MonoBehaviour
    {
        [SerializeField] private PlayfabBusDataSO playfabBusSO;
        private void OnEnable()
        {
            playfabBusSO.OnSucessLogin += OnLoginSucess;
            playfabBusSO.OnIgnoreLogin += OnIgnoreLogin;
        }
        private void OnDisable()
        {
            playfabBusSO.OnSucessLogin -= OnLoginSucess;
            playfabBusSO.OnIgnoreLogin -= OnIgnoreLogin;
        }
        public void Login()
        {
            playfabBusSO.OnLogin.Invoke();
        }
        private void OnLoginSucess()
        {
            this.gameObject.SetActive(false);
        }
        private void OnIgnoreLogin()
        {
            this.gameObject.SetActive(false);
        }
    }

}
