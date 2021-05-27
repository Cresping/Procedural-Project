using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


namespace HeroesGames.ProjectProcedural.Utils
{
    public static class SequencesTween
    {
        /// <summary>
        /// Mueve el tranform de su posición actual a la una determinada posición, luego vuelve a su posicion original en el tiempo dado.
        /// </summary>
        /// <param name="transform">Transform que se moverá</param>
        /// <param name="position">Posición a la que se moverá</param>
        /// <param name="duration">Tiempo que tardará en realizar la animación</param>
        /// <returns>'La sequencia del DoTween</returns>
        public static Sequence DOMoveAnimation(Transform transform, Vector2 position, float duration)
        {
            Sequence moveAnimation = DOTween.Sequence();
            moveAnimation.Append(transform.DOMove(position, duration / 2, true));
            moveAnimation.Append(transform.DOMove(transform.position, duration / 2, true));
            return moveAnimation;
        }
    }
}

