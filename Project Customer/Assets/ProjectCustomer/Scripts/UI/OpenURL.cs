using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour
{
    [SerializeField]string url = "";
    public void Pressed()
    {
        Application.OpenURL(url);
    }
}