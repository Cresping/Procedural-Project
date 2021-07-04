using System.Collections;
using System.Collections.Generic;
using HeroesGames.ProjectProcedural.SO;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.Sound
{
    public class SoundUIController : SoundController
    {
        [SerializeField] private MainMenuBusSO mainMenuBusSO;
        [SerializeField] private SoundVariableSO buttonSound;
        [SerializeField] private SoundVariableSO dragSound;
        [SerializeField] private SoundVariableSO dropSound;

        private void OnEnable()
        {
            mainMenuBusSO.OnDragItem += PlayDragSound;
            mainMenuBusSO.OnDropItem += PlayDropSound;
        }
        private void OnDisable()
        {
            mainMenuBusSO.OnDragItem -= PlayDragSound;
            mainMenuBusSO.OnDropItem -= PlayDropSound;
        }
        public void PlayButtonSound()
        {
            base.PlaySound(buttonSound);
        }
        public void PlayDragSound(ObjectInventoryVariableSO objectDrag)
        {
            base.PlaySound(dragSound);
        }
        public void PlayDropSound(ObjectInventoryVariableSO objectDrop)
        {
            base.PlaySound(dropSound);
        }
    }

}
