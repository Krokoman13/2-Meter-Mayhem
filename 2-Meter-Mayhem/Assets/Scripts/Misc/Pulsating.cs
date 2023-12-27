using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulsating : MonoBehaviour
{
    Renderer renderer;
    
    float alpha
    {
        set
        {
            Color newColor = renderer.material.color;
            newColor = new Color(newColor.r, newColor.g, newColor.b, value);
            renderer.material.color = newColor;
        }

        get {return renderer.material.color.a;}
    }
    [SerializeField] float tolerance;

    Color startingColor;
    Color clearColor;
    bool ghosting;
    [SerializeField] float speed;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        startingColor = renderer.material.color;
        clearColor = new Color(startingColor.r, startingColor.g, startingColor.b, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (ghosting)
        {
            renderer.material.color = Color.Lerp(renderer.material.color, clearColor, speed*Time.deltaTime);
            if (alpha - tolerance < clearColor.a) ghosting = false;
            return;
        }

        renderer.material.color = Color.Lerp(renderer.material.color, startingColor, speed * Time.deltaTime);
        if (alpha + tolerance > startingColor.a) ghosting = true;
    }
}
