using System.Collections;
using System.Collections.Generic;
using HeroesGames.ProjectProcedural.SO;
using HeroesGames.ProjectProcedural.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HeroesGames.ProjectProcedural.UI
{
    public class UIButtonAnimationHelper : MonoBehaviour
    {
        [SerializeField] private CombatVariableSO combatVariableSO;
        [SerializeField] private TextMeshProUGUI superAttackBar;
        private UIImageAnimation _imageAnimation;
        private Button _button;

        private void Awake()
        {
            _imageAnimation = GetComponent<UIImageAnimation>();
            _button = GetComponent<Button>();
            _button.interactable = false;
            _imageAnimation.enabled = false;
            superAttackBar.text = "";

        }
        private void OnEnable()
        {
            combatVariableSO.OnCombatPlayerStrongAttackUnlocked += DoUnlockStrongAttack;
            combatVariableSO.OnCombatPlayerAttack += DoIncreaseSuperAttackBar;
        }
        private void OnDisable()
        {
            combatVariableSO.OnCombatPlayerStrongAttackUnlocked -= DoUnlockStrongAttack;
            combatVariableSO.OnCombatPlayerAttack -= DoIncreaseSuperAttackBar;
            DoLockStrongAttack();
        }
        private void DoIncreaseSuperAttackBar(int n)
        {
            superAttackBar.text = "";
            for (int i = 0; i < n; i++)
            {
                superAttackBar.text += "|";
            }
        }
        private void DoUnlockStrongAttack()
        {
            _button.interactable = true;
            _imageAnimation.enabled = true;
        }
        public void DoLockStrongAttack()
        {
            _button.interactable = false;
            _imageAnimation.enabled = false;
        }
    }
}

