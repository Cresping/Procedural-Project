using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HeroesGames.ProjectProcedural.UI
{
     [RequireComponent(typeof(Image))]
    public class UIImageAnimation : MonoBehaviour
    {

        [SerializeField] private Sprite[] sprites;
        [SerializeField] private int spritePerFrame = 60;
        [SerializeField] private bool loop = true;
        [SerializeField] private bool destroyOnEnd = false;

        private int index = 0;
        private Image image;
        private int frame = 0;

        void Awake()
        {
            image = GetComponent<Image>();
        }

        void Update()
        {
            if (!loop && index == sprites.Length) return;
            frame++;
            if (frame < spritePerFrame) return;
            image.sprite = sprites[index];
            frame = 0;
            index++;
            if (index >= sprites.Length)
            {
                if (loop) index = 0;
                if (destroyOnEnd) Destroy(gameObject);
            }
        }
    }
}
