using System;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictorySceen : MonoBehaviour
{
    [Header("Score display")]
    [SerializeField]
    TextMeshProUGUI scoreText = null;
    [SerializeField]
    TextMeshProUGUI highscoreText = null;
    string highscorePrefName = "HighScore";
    PlayData playData = null;

    //-------------------------------------------------------------------
    void Awake()
    {
        playData = PlayData.instance;
    }
    void Start()
    {
        DisplayHighscores();
    }

    void DisplayHighscores()
    {
        int highscore = PlayerPrefs.GetInt(highscorePrefName, 0);
        int score = PlayData.instance.obtainedScore * 2;
        highscoreText.text = highscore.ToString("D4");
        scoreText.text = score.ToString("D4");

        if (score > highscore)
        {
            PlayerPrefs.SetInt(highscorePrefName, score);
            //Display a text saying "New Highscore!"
        }

    }
}