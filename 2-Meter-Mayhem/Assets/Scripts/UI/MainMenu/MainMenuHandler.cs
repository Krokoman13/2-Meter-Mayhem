using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    SceneHandler sceneHandler = null;

    void Awake()
    {
        sceneHandler = GetComponent<SceneHandler>();
    }
    public void BUTTON_StartPlaying()
    {
        if (PlayerPrefs.GetInt(SettingsHandler.instance.allSettings[0].name) == 1)
        {
            sceneHandler.NextScene();
        }
        else
        {
            sceneHandler.MasklessScene();
        }
    }
}