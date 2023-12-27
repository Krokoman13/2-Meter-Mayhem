using System;
using UnityEngine.UI;
using UnityEngine;

public class CheckSlider : MonoBehaviour
{    
    public int defaultValue = 100;

    Slider slider = null;

    [Header("Index in SettingsHandler's list")]
    [SerializeField] int settingsIndex = 0;
    string settingsName = "";


    private void Awake()
    {
        slider = GetComponent<Slider>();
    }
    void Start()
    {
        //The box needs to be checked based on the saved data.
        settingsName = SettingsHandler.instance.allSettings[settingsIndex].name;         //get the pref name
        SetSlider(PlayerPrefs.GetInt(settingsName, defaultValue));
    }

    void SetSlider(int value)
    {
        slider.value = value;
    }

    public void OnSliderChanged()
    {
        PlayerPrefs.SetInt(settingsName, (int)slider.value);
        //print("slider set to: " + (int)slider.value);
    }
}