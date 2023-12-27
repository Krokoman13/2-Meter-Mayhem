using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SettingsHandler))]
public class PlayData : MonoBehaviour
{
    public static PlayData instance = null;
    /*[HideInInspector]*/public int obtainedScore;
    //other data can be stored here, like which groceries and the amount of them.

    void Start()
    {
        instance = this;
    }
}