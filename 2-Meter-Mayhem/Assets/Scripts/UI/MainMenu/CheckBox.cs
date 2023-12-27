using System;
using UnityEngine.UI;
using UnityEngine;

public class CheckBox : MonoBehaviour
{
    [SerializeField] Image checkmarkImage = null;
    bool isChecked = false;
    public bool defaultValue = false;

    [Header("Sprites to use")]
    [SerializeField] Sprite uncheckedSprite = null;
    [SerializeField] Sprite checkedSprite = null;

    //[SerializeField] string settingPrefName = "";    
    [Header("Index in SettingsHandler's list")]
    [SerializeField]int settingsIndex = 0;
    string settingsName = "";

    void Start()
    {
        //The box needs to be checked based on the saved data.
        settingsName = SettingsHandler.instance.allSettings[settingsIndex].name;                        //get the pref name
        isChecked = Convert.ToBoolean(PlayerPrefs.GetInt(settingsName, Convert.ToInt32(defaultValue)));         //apply the taken info
        UpdateSprite();
    }
    public void BUTTON_ToggleCheckmark()
    {
        isChecked = !isChecked;
        UpdateSprite();        
    }

    void UpdateSprite()
    {
        if (isChecked)
            checkmarkImage.sprite = checkedSprite;
        else
            checkmarkImage.sprite = uncheckedSprite;

        PlayerPrefs.SetInt(settingsName, Convert.ToInt32(isChecked));        
    }
}