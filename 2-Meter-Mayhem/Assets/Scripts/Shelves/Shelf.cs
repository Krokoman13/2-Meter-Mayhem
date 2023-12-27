using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : MonoBehaviour
{
    public Grocery.Groceries product;
    private Grocery.Groceries _prevProduct;
    [SerializeField] private GameObject _placementsGameobject;
    private List<GameObject> _placments = new List<GameObject>();

    public int toRemove;

    public void SetProduct(Grocery.Groceries pProduct)
    {
        product = _prevProduct = pProduct;
        clearProducts();
        placeProducts();
    }

    private void clearProducts()
    {
        foreach (GameObject placement in _placments)
        {
            for (int i = 0; i < placement.transform.childCount; i++)
            {
                GameObject child = placement.transform.GetChild(i).gameObject;

                if (child.CompareTag("Grocery"))
                {
                    DestroyImmediate(child);
                }
            }
        }
    }

    private void placeProducts()
    {
        foreach (GameObject placment in _placments) 
        {
            Instantiate(Grocery.GetGameObject(product), placment.transform.position, new Quaternion(0,0,0,0), placment.transform);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        buildPlacements();
        if (product != Grocery.Groceries.UNKNOWN) SetProduct(product);
    }

    private void buildPlacements()
    {
        if (_placementsGameobject == null) return;

        for (int i = 0; i < _placementsGameobject.transform.childCount; i++)
        {
            Transform child = _placementsGameobject.transform.GetChild(i);

            if (_placments.Contains(child.gameObject)) continue;

            GameObject placement = new GameObject("Placemenent");
            placement.transform.position = child.position;
            placement.transform.rotation = child.rotation;
            placement.transform.parent = _placementsGameobject.transform;
            placement.transform.localScale = child.localScale;
            _placments.Add(placement);

            Destroy(child.gameObject);
        }
    }

    private void Update()
    {
        if (_prevProduct != product)
        { 
            SetProduct(product);
        }
        if (toRemove > 0)
        {
            RemoveProduct(toRemove);
            toRemove = 0;
        }
    }

    public Grocery.Groceries GetProduct()
    {
        if (_placments.Count < 1) return Grocery.Groceries.UNKNOWN;
        return product;
    }

    public void RemoveProduct(int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            if (_placments.Count < 1) break;

            GameObject toRemove = _placments[UnityEngine.Random.Range(0, _placments.Count)];
            _placments.Remove(toRemove);
            Destroy(toRemove);
        }
    }
}
