using System;
using HeroesGames.ProjectProcedural.SO;
using TMPro;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    [SerializeField] private GameObject userNameRequestPanel;
    [SerializeField] private LoginManagerSO loginManager;
    [SerializeField] private TextMeshProUGUI
        userNameBox,
        userNameBoxPlaceholder,
        userName;

    [SerializeField] private EventSO welcomeEvent;

    private string _user;

    private void OnEnable()
        => welcomeEvent.CurrentAction += WelcomeMessage;

    private void OnDisable()
        => welcomeEvent.CurrentAction -= WelcomeMessage;

        private void Update()
    => userNameRequestPanel.SetActive(loginManager.NoUserName);
    

    public void OnPressOKButton()
    {
        _user = userNameBox.text;

        if (!string.IsNullOrWhiteSpace(_user))
            loginManager.UpdateUserName(_user);
        else
            userNameBoxPlaceholder.text = "Write a valid username";
    }

    private void WelcomeMessage(string user)
        => userName.text = "Welcome " + user;

    private void WelcomeMessage()
        => WelcomeMessage(loginManager.NickName);
}
