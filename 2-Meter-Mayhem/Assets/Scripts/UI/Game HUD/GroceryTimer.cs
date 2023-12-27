using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class GroceryTimer : MonoBehaviour
{    
    bool timerActive = true;

    Animator anim = null;
    [SerializeField]Animator shopListAnim = null;
    //SLIDER VALUES
    [SerializeField] RectTransform sliderRT = null;
    [SerializeField] Slider timerSlider = null;
    [SerializeField] Image fillImage = null;

    [Header("Timer Values")]
    public float timeInSeconds = 15f;
    public float remainingTimeSeconds
    {
        get { return timerSlider.value; }
    }

    //[SerializeField] int[] fillColorPercentages = new int[3];
    [SerializeField] int fillColorPercentageBlack = 80;
    [SerializeField] int fillColorPercentageRed = 30;


    Vector2 startPos = Vector2.zero;
    float sliderShakeAmplitude = 2.7f;

    public void ResetTimer()
    {
        if (timerSlider.value == 0)
        {
            anim.ResetTrigger("NewScore");
            //anim.SetBool("TimerGone", false);
        }
        timerSlider.value = timeInSeconds;
        timerActive = true;
    }
    public void StopTimer()
    {
        timerActive = false;
    }


    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        timerSlider.maxValue = timeInSeconds;
        timerSlider.value = timeInSeconds;

        startPos = sliderRT.anchoredPosition;
    }


    void FixedUpdate()
    {
        //UpdateTimeText();
        if(timerActive)
            UpdateTimerSlider();

    }


    void UpdateTimerSlider()
    {
        if (timerSlider.value > 0)
        {
            timerSlider.value -= Time.fixedDeltaTime;

            float percentage = timerSlider.value / timerSlider.maxValue * 100;
            Color targetColor = Color.green;
            if (percentage < fillColorPercentageBlack)
            {
                targetColor = Color.black;
                if (percentage < fillColorPercentageRed)
                {
                    targetColor = Color.red;
                    ShakeSlider();
                }

            }

            fillImage.color = targetColor;
        }
        else
        {
            //make slider invisible
            //don't play animation when player obtains new grocery
            //anim.SetBool("TimerGone", true);
            shopListAnim.SetTrigger("NextItem");
            timerActive = false;
        }
    }

    void ANIM_TimerRestarts()
    {
        //ResetTimer();        
    }

    void ShakeSlider()
    {
        sliderRT.anchoredPosition = startPos + Random.insideUnitCircle * sliderShakeAmplitude;
    }

    //void UpdateTimeText()
    //{
        //if (timerActive)
        //    currentTimer += Time.deltaTime;

        //int minutes = Mathf.FloorToInt(currentTimer / 60f);
        //int seconds = Mathf.FloorToInt(currentTimer - minutes * 60);
        //string newText = string.Format("{0:0}:{1:00}", minutes, seconds);

        //timerText.text = newText;
    //}
}