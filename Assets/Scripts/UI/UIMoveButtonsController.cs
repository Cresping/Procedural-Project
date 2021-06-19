using System;
using System.Collections;
using System.Collections.Generic;
using HeroesGames.ProjectProcedural.SO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HeroesGames.ProjectProcedural.UI
{
    public class UIMoveButtonsController : EventTrigger
    {
        public BoolVariableSO boolVariableSO;
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            boolVariableSO.RuntimeValue = true;
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            boolVariableSO.RuntimeValue = false;
        }
        public override void OnPointerEnter(PointerEventData eventData)
        {
            if(boolVariableSO.RuntimeValue)
            {
                base.OnPointerEnter(eventData);
            }
        }
        public void OnDisable()
        {
            boolVariableSO.RuntimeValue = false;
        }
    }
}

