using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OnScreenButton : MonoBehaviour
{
    Canvas myCanvas = null;

    [Header("Grab Button Prompt")]
    [SerializeField] RectTransform grabPrompt = null;
    [SerializeField] Transform grabTarget = null;
    PlayerCollision pCol = null;

    void Awake()
    {
        myCanvas = GetComponent<Canvas>();
        pCol = FindObjectOfType<PlayerCollision>();
    }
    void Start()
    {
        myCanvas.worldCamera = Camera.main;
        pCol.currentShelfChanged += OnCurrentShelfChanged;
        //pCol.foundCorrectShelf += OnCorrectShelfFound;
    }

    void OnCurrentShelfChanged(Shelf newShelf)
    {
        if (newShelf != null)
        {
            if (pCol.IsCorrectGrocery())
            {
                EnableOnscreenButton(newShelf.transform);
                return;
            }
        }

        DisableOnscreenButton();

        //if (newShelf == null)
        //{
        //    grabPrompt.gameObject.SetActive(false);
        //    grabTarget = null;
        //}
        //else
        //{
        //    //grabPrompt.gameObject.SetActive(true);
        //    //grabTarget = newShelf.transform;
        //}        
    }

    public void DisableOnscreenButton()
    {
        grabPrompt.gameObject.SetActive(false);
        grabTarget = null;
    }

    public void EnableOnscreenButton(Transform target)
    {
        grabPrompt.gameObject.SetActive(true);
        grabTarget = target;
    }

    void OnCorrectShelfFound(Shelf newShelf)
    {
        //grabPrompt.gameObject.SetActive(true);
        //grabTarget = newShelf.transform;
    }

    void LateUpdate()
    {
        Display_GrabPrompt();

    }


    void Display_GrabPrompt()
    {
        if (grabTarget == null)
            return;


        Vector3 here = myCanvas.worldCamera.WorldToScreenPoint(grabTarget.position);
        grabPrompt.position = here;
    }
}