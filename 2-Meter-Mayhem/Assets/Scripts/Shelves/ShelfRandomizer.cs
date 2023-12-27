using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfRandomizer : MonoBehaviour
{
    Shelf shelf;
    [SerializeField] List<Grocery.Groceries> possibleGroceries;
    [SerializeField] bool randomize;

    // Start is called before the first frame update
    void Start()
    {
        shelf = GetComponent<Shelf>();
    }

    // Update is called once per frame
    void Update()
    {
        if (randomize)
        {
            shelf.product = possibleGroceries[Random.Range(0, possibleGroceries.Count)];
        }

        Destroy(this);
    }
}
