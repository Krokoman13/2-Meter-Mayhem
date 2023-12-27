using System;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseSceneHandler : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI loseText = null;
    [Header("Should be 101"), SerializeField]
    int maxPercent = 101;

    [Header("Score display")]
    [SerializeField]
    TextMeshProUGUI scoreText = null;
    [SerializeField]
    TextMeshProUGUI highscoreText = null;
    string highscorePrefName = "HighScore";    


    #region
    [Space(10)]
    [TextArea]
    public string importantNote = "Order by high -> low probability";
    #endregion
    [Serializable]
    public struct LoseScenario
    {
        public string scenarioText;
        public int probability;        
    }
    public LoseScenario[] scenarios;

    //-------------------------------------------------------------------
    void Awake()
    {
        //playData = FindObjectOfType<PlayData>();
    }
    void Start()
    {
        //Get a random message and display it.
        LoseScenario chosenScenario = GetScenario();
        loseText.text = chosenScenario.scenarioText;

        DisplayHighscores();
    }


    /// <summary>
    /// Grab a random LoseScenario based on the probability
    /// </summary>
    /// <returns></returns>
    LoseScenario GetScenario()
    {        
        //Grab a random value to represent to compare the probability with.
        int rndm = UnityEngine.Random.Range(1, maxPercent);
        //Start with 0, as it's (or should be) the most likely one
        LoseScenario chosenScenario = scenarios[0];

        //If the random value is within the probability margin, then make that the currently chosen one.
        //This requires on the scenario probabilities being ordered from high probability to low.
        foreach(LoseScenario ls in scenarios)
        {
            if(rndm <= ls.probability)
                chosenScenario = ls;            
        }
        
        return chosenScenario;
    }

    void DisplayHighscores()
    {
        int highscore = PlayerPrefs.GetInt(highscorePrefName, 0);
        int score = PlayData.instance.obtainedScore;
        highscoreText.text = highscore.ToString("D4");
        scoreText.text = score.ToString("D4");

        if(score > highscore)
        {
            PlayerPrefs.SetInt(highscorePrefName, score);
            //Display a text saying "New Highscore!"
        }

    }
}