using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExposureMeter : MonoBehaviour
{    
    [SerializeField] private float _exposure;
    public Action infectionRaiseAction = null;

    public float recoverTimeSecs = 3f;
    public float regenerationPerSec = 25f;

    SceneHandler sceneHandler = null;
    public enum states {hurting, recovering, regenerating, healthy, dead}
    public states state = states.regenerating;
    private bool _hurting;

    public float exposure
    {
        get 
        {
            return _exposure;
        }
    }
    void Awake()
    {
        sceneHandler = FindObjectOfType<SceneHandler>();
    }
    void Start()
    {
        _exposure = 0f;
    }

    public void Raise(float infectAmount)
    {
        if (infectAmount < 0f)
        {
            Debug.LogError("Cannot raise a negative amount");
            return;
        }

        state = states.hurting;
        _hurting = true;
        StopAllCoroutines();

        _exposure += infectAmount;
        _exposure = Mathf.Clamp(_exposure, 0f, 100f);

        infectionRaiseAction?.Invoke();

        if (_exposure > 99.5f) 
        {
            state = states.dead;            
            if (sceneHandler != null) sceneHandler.LoadDeathScene();
        }
    }

    public void Lower(float healAmount)
    {
        if (healAmount < 0f)
        {
            Debug.LogError("Cannot heal a negative amount");
            return;
        }

        _exposure -= healAmount;
        _exposure = Mathf.Clamp(_exposure, 0f, 100f);
        if (_exposure <= 0)
        {            
            state = states.healthy;
        }
            
        infectionRaiseAction?.Invoke();
    }

    private void Update()
    {
        switch (state)
        {
            case states.hurting:
                if (!_hurting)
                {
                    StartCoroutine(recover());
                }
                _hurting = false;
                break;

            case states.recovering:
                break;

            case states.regenerating:
                Lower(regenerationPerSec * Time.deltaTime);                
                StopAllCoroutines();                
                break;

            case states.dead:
                StopAllCoroutines();
                break;
            case states.healthy:                
                break;
        }
    }


    private IEnumerator recover()
    {
        state = states.recovering;

        yield return new WaitForSeconds(recoverTimeSecs);
        state = states.regenerating;
    }
}
