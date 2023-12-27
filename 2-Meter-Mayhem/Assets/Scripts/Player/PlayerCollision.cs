using System;
using UnityEngine;
using System.Collections.Generic;

public class PlayerCollision : MonoBehaviour
{    
    ShoppingListHandler shoplist = null;

    [Header("Detecting Shelves")]
    [SerializeField]
    Transform shelfDetectBox = null;
    [SerializeField]
    LayerMask lmask = 0;
    


    public Shelf currentShelf = null;
    public Action<Shelf> currentShelfChanged = null;

    //----------------------------------------------------

    void Awake()
    {        
        shoplist = FindObjectOfType<ShoppingListHandler>();
    }

    private void Start()
    {
        //pInputs.fire2InputAction += Fire2Input;
    }


    public float distance = 1;
    void Update()
    {
        Vector3 halfExtends = new Vector3(shelfDetectBox.localScale.x / 2, shelfDetectBox.localScale.y / 2, shelfDetectBox.localScale.z / 2);
        Vector3 startPos = shelfDetectBox.position - (transform.forward * halfExtends.z);
        //Debug.DrawRay(startPos, transform.forward * shelfDetectBox.localScale.z, Color.magenta);
        //Debug.DrawRay(startPos - new Vector3(halfExtends.x, 0, 0), transform.right * shelfDetectBox.localScale.x, Color.magenta);        

        RaycastHit[] hits = Physics.BoxCastAll(startPos, halfExtends, transform.forward, shelfDetectBox.rotation, distance, lmask, QueryTriggerInteraction.Collide);
        //HIT
        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                //Grab the closest shelf-component
                if (hit.transform.gameObject.layer == 6)
                {
                    //Ensure if this hit is a SHELF. Ignore if it isn't
                    Shelf shelf = hit.transform.GetComponent<ShelfHitbox>().parentShelf;
                    if (shelf == null)
                        continue;

                    //If there's no shelf, use this one.
                    if (currentShelf == null)
                    {
                        currentShelf = shelf;
                        if (currentShelfChanged != null)
                            currentShelfChanged(shelf);
                    }


                    //Use the closest shelf
                    float newShelfDist = Vector3.Distance(hit.transform.position, shelfDetectBox.position);
                    float currentShelfDist = Vector3.Distance(currentShelf.transform.position, shelfDetectBox.position);
                    if (newShelfDist < currentShelfDist)
                    {
                        currentShelf = shelf;
                        if (currentShelfChanged != null)
                            currentShelfChanged(shelf);
                    }
                }
            }
        }
        //NO HIT
        else
        {
            if (currentShelfChanged != null && currentShelf != null)
                currentShelfChanged(null);
            currentShelf = null;
        }
    }

    [Header("Cart Grocery Spawning")]
    [SerializeField] ShoppingCartHandler cartHandler = null;
    //[SerializeField] Prop obtainedGroceryPrefab = null;
    [SerializeField] int maxItemsInCart = 35;
    public void InteractWithShelf()
    {
        if (currentShelf == null)
            return;


        if (IsCorrectGrocery())
        {            
            currentShelf.RemoveProduct();
            shoplist.TargetGroceryFound();

            //Make sure there's not too many props
            if (cartHandler.obtainedGroceryItems.Count >= maxItemsInCart)
            {
                Destroy(cartHandler.obtainedGroceryItems[0].gameObject);
                cartHandler.obtainedGroceryItems.RemoveAt(0);
            }

            //Spawn new prop
            Prop prefabToSpawn = FindPropToSpawn(currentShelf.product);
            Prop newProp = Instantiate(prefabToSpawn, null);            
            //GameObject newPropMesh = Instantiate(Grocery.GetGameObject(currentShelf.product), newProp.transform);
            cartHandler.obtainedGroceryItems.Add(newProp);

            AudioHandler.instance.PlaySFX(AudioHandler.instance.sfx_pickup);

            //Refresh the shelf-detection to ensure any of it's systems don't get stuck.
            shoplist.currentTargetGrocery = Grocery.Groceries.UNKNOWN;          //Make sure there's no double detections                                 
        }
    }

    [SerializeField]
    List<Prop> allPropTypes = new List<Prop>();
    Prop FindPropToSpawn(Grocery.Groceries targetGrocery)
    { 
        foreach(Prop p in allPropTypes)
        {
            if (p.myType == targetGrocery)            
                return p;           
        }

        return null;
    }

    public bool IsCorrectGrocery()
    {
        bool result = false;

        if (currentShelf == null)
            return false;

        Grocery.Groceries newGrocery = (Grocery.Groceries)(currentShelf.GetProduct());
        if (shoplist.currentTargetGrocery == newGrocery)
            result = true;

        return result;
    }
}