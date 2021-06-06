using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace HeroesGames.ProjectProcedural.UI
{
    [RequireComponent(typeof(TextMeshPro))]
    public class UIDamageController : MonoBehaviour
    {
        [SerializeField] private float fontSizeSpeed;
        [SerializeField] private float fontMovementSpeed;
        private TextMeshPro damageText;
        private void Awake()
        {
            damageText = GetComponentInChildren<TextMeshPro>();
        }
        private void Update()
        {
            transform.position = (Vector2) transform.position + Vector2.up * fontMovementSpeed * Time.deltaTime;
            damageText.fontSize = damageText.fontSize - fontSizeSpeed * Time.deltaTime;
            if(damageText.fontSize<=0)
            {
                Destroy(this.gameObject,0);
            }
        }
        private void OnDisable() 
        {
            Destroy(this.gameObject,0);
        }
    }

}
