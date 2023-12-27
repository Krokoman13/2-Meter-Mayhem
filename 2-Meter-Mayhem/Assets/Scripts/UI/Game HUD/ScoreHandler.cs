using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    int currentScore = 0;
    int pointsToAdd = 0;
    [SerializeField] TextMeshProUGUI scoreText = null;
    GroceryTimer groceryTimer = null;
    Animator anim = null;    

    void Awake()
    {
        groceryTimer = GetComponent<GroceryTimer>();        
        anim = GetComponent<Animator>();        
    }
    void Start()
    {        
        ScoreChanged();
    }

    /// <summary>
    /// Player received points, this function will manage the animation and associated methods.
    /// </summary>
    public void PointsGained(int amount)
    {
        pointsToAdd = amount;
        groceryTimer.StopTimer();
        anim.SetTrigger("NewScore");
    }


    public void ANIM_TimerPointsAbsorbed()
    {
        print(1);
        groceryTimer.ResetTimer();        
        currentScore += pointsToAdd;
        ScoreChanged();
    }

    void ScoreChanged()
    {
        scoreText.text = currentScore.ToString("D4");        
        if (PlayData.instance != null) PlayData.instance.obtainedScore = currentScore;
    }
}