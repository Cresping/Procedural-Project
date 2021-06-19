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

        private int _index = 0;
        private Image _image;
        private int _frame = 0;
        private Sprite _originalSprite;

        void Awake()
        {
            _image = GetComponent<Image>();
            _originalSprite = _image.sprite;
        }

        void Update()
        {
            if (!loop && _index == sprites.Length) return;
            _frame++;
            if (_frame < spritePerFrame) return;
            _image.sprite = sprites[_index];
            _frame = 0;
            _index++;
            if (_index >= sprites.Length)
            {
                if (loop) _index = 0;
                if (destroyOnEnd) Destroy(gameObject);
            }
        }
        private void OnDisable()
        {
            _image.sprite = _originalSprite;
        }
    }
}
