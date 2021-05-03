using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monitor : MonoBehaviour
{
    private bool isBusy;
    public bool IsBusy { get => isBusy; set => isBusy = value; }
    private void Awake()
    {
        this.isBusy = false;
    }   
}
