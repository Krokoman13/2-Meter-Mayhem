using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    OnScreenButton onScreenButton;
    SceneHandler sceneHandler;
    PlayerInputs playerInputs;
    Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        onScreenButton = FindObjectOfType<OnScreenButton>();
        sceneHandler = FindObjectOfType<SceneHandler>();
        playerInputs = FindObjectOfType<PlayerInputs>();
        renderer = GetComponent<Renderer>();
        renderer.enabled = false;
    }

    private void Update()
    {
        if (!renderer.enabled)
        {
            if (PlayData.instance.obtainedScore > 1000)
            {
                renderer.enabled = true;
            }
        }
    }

    void Interact(PlayerInputs.InputTypes inputTypes)
    {
        if (inputTypes == PlayerInputs.InputTypes.Down) sceneHandler.VictoryScreen(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("I am supposed to do smth");
        if (!other.gameObject.CompareTag("Player")) return;
        playerInputs.fire1InputAction += Interact;
        onScreenButton.EnableOnscreenButton(transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        playerInputs.fire1InputAction -= Interact;
        onScreenButton.DisableOnscreenButton();
    }
}
