using System;
using System.Collections.Generic;
using UnityEngine;

public class SettingsHandler : MonoBehaviour
{
    public static SettingsHandler instance = null;    

    [Serializable]
    public struct AppSetting
    {
        public string name;
        //public int value;
    }
    public AppSetting[] allSettings;

    void Awake()
    {
        //Make sure only one game handler exists
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void SetInt(string keyName, int value)
    {
        PlayerPrefs.SetInt(keyName, value);
    }

    public int GetInt(string keyName)
    {
        return PlayerPrefs.GetInt(keyName);
    }
}