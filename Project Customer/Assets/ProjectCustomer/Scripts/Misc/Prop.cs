using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    public Grocery.Groceries myType = Grocery.Groceries.UNKNOWN;
    [HideInInspector]public Collider col = null;
    [HideInInspector]public Rigidbody rb = null;

    void Awake()
    {
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }
}