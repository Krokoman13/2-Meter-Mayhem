using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShirtColorer : MonoBehaviour
{
    public Color shirtColor;

    [SerializeField] private Renderer shirtRenderer;

    // Start is called before the first frame update
    void Start()
    {
        if (shirtRenderer == null) shirtRenderer = gameObject.GetComponent<Renderer>();

        if (shirtColor == new Color(0, 0, 0, 0)) shirtColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
        shirtRenderer.material.color = shirtColor;
    }
}
