using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneHandler : MonoBehaviour
{
    void OnEnable()
    {
        DetectSceneMusic();
    }

    void DetectSceneMusic()
    {
        if (AudioHandler.instance == null)
            return;

        if (SceneManager.GetActiveScene().buildIndex == 1)
            AudioHandler.instance.GameplayLevelLoaded();
        else
            AudioHandler.instance.StopMusic();
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.R))
    //        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //}

    public void LoadDeathScene()
    {
        SceneManager.LoadScene("LoseScene");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void NextScene()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene + 1);
    }

    public void MasklessScene()
    {
        SceneManager.LoadScene("Maskless");
    }

    public void VictoryScreen()
    {
        SceneManager.LoadScene("Victory");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}