using TMPro;
using UnityEngine;

public class MainMenuButton : MonoBehaviour
{
    [SerializeField]Color normalColor = Color.blue;
    [SerializeField] Color pressedColor = Color.black;
    [SerializeField] TextMeshProUGUI buttonText = null;



    public void ANIM_Normal()
    {
        buttonText.color = normalColor;
    }
    public void ANIM_Pressed()
    {
        buttonText.color = pressedColor;
    }   
}