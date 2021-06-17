using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InvertColorController : MonoBehaviour
{
    private Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    private void OnEnable()
    {
        OriginalColor();
    }

    [ContextMenu("InvertColor")]
    public void InvertColor()
    {
        Material mat = Instantiate(image.material);
        mat.SetFloat("_InvertColors", 1);
        image.material = mat;
    }
    [ContextMenu("OriginalColor")]
    public void OriginalColor()
    {
        Material mat = Instantiate(image.material);
        mat.SetFloat("_InvertColors", 0);
        image.material = mat;
    }
}
