using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class ExposureFill : MonoBehaviour
{
    [SerializeField] Image exposureFill = null;
    [SerializeField] TextMeshProUGUI exposureText = null;
    Animator anim = null;
    ExposureMeter exposureMeter = null;

    [SerializeField] RectTransform exposureImageT = null;
    Vector2 startPosition = Vector2.zero;

    [Header("UI Shake")]
    [SerializeField] float shakeAmplitude = 2f;

    void Awake()
    {
        exposureMeter = FindObjectOfType<ExposureMeter>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        exposureMeter.infectionRaiseAction += UpdateExposureVisual;
        UpdateExposureVisual();

        startPosition = exposureImageT.localPosition;
    }

    void FixedUpdate()
    {
        string s = "IsChanging";
        if (exposureMeter.state == ExposureMeter.states.hurting)
        {
            exposureImageT.localPosition = startPosition + Random.insideUnitCircle * shakeAmplitude;
            anim.SetBool(s, true);
        }
        else if(exposureMeter.state == ExposureMeter.states.healthy)
        {
            anim.SetBool(s, false);
        }
    }

    void UpdateExposureVisual()
    {
        exposureFill.fillAmount = (exposureMeter.exposure / 100);

        string newText = exposureMeter.exposure.ToString().Split(',')[0];
        newText = newText.Split('.')[0];
        exposureText.text = newText + "%";
    }
}