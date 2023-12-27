using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayGrocery : MonoBehaviour
{
    [SerializeField] ShoppingListHandler shoppingListHandler;
    [SerializeField] Grocery.Groceries currentGrocery;
    [SerializeField] Transform groceryHolder;

    private void Update()
    {
        if (shoppingListHandler.currentTargetGrocery != currentGrocery)
        {
            currentGrocery = shoppingListHandler.currentTargetGrocery;

            foreach (Transform child in groceryHolder)
            {
                Destroy(child.gameObject);
            }

            GameObject grocery = Instantiate(Grocery.GetGameObject(currentGrocery), groceryHolder);
            //grocery.layer = 5;
        }
    }
}
