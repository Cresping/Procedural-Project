using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesGames.ProjectProcedural.Utils
{

    [Serializable]
    /// <summary>
    /// Clase encargada de cambiar las animaciones del Animator
    /// </summary>
    public class AnimatorHelper
    {
        [SerializeField] private String nameWalkUpParameter = "WalkUp";
        [SerializeField] private String nameWalkDownParameter = "WalkDown";
        private Animator _animator;

        ///// <summary>
        ///// Constructor parametrizado
        ///// </summary>
        ///// <param name="animator">Animator del modelo</param>
        public AnimatorHelper(Animator animator)
        {
            this._animator = animator;
        }
        /// <summary>
        /// Cambia el estado de  la animaci�n 'Walk'
        /// </summary>
        /// <param name="value">Estado</param>
        public void SetWalkUp(bool value)
        {
            _animator.SetBool(nameWalkUpParameter, value);
        }
        /// <summary>
        /// Cambia el estado de  la animaci�n 'Walk'
        /// </summary>
        /// <param name="value">Estado</param>
        public void SetWalkDown(bool value)
        {
            _animator.SetBool(nameWalkDownParameter, value);
        }
        /// <summary>
        /// Cambia el animator del modelo
        /// </summary>
        /// <param name="animator">Nombre del animator que se cambiar�</param>
        public void ChangeAnimator(String animator)
        {
            _animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animators/Animator" + animator);
        }
        /// <summary>
        /// Desactiva el animator del modelo
        /// </summary>
        public void DisableAnimator()
        {
            _animator.enabled = false;
        }
    }
}