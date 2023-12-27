using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Linq;

public class ShoppingListHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentShoppingListText = null;
    [SerializeField] TextMeshProUGUI nextShoppingListText = null;
    [SerializeField] Image currentShoppingListImage = null;

    Animator anim = null;   //The shoppinglist animator. Should be on the same object as this script.
    [SerializeField] GroceryTimer groceryTimer = null;
    [SerializeField] ScoreHandler scoreHandler = null;

    //Add all product renders to this list IN THE GROCERY ENUM'S ORDER -->
    //Meat, Milk, Olive_oil, Orange, Red_wine, Toilet_rolls, Tomato, Watermelon, White_wine, Hotdog, Pizza, Bread
    [SerializeField] Sprite[] allGrocerySprites = new Sprite[12];
    

    public Dictionary<Grocery.Groceries, int> gottenGroceries = new Dictionary<Grocery.Groceries, int>();
    public List<Grocery.Groceries> prevGroceries;
    public Grocery.Groceries currentTargetGrocery
    {
        get { return prevGroceries[3]; }
        set
        {
            if (value == Grocery.Groceries.UNKNOWN) return;

            prevGroceries.RemoveAt(0);
            prevGroceries.Add(value);
        }
    }

    void Awake()
    {
        anim = GetComponent<Animator>();
        
        for (int i = 1; i < Grocery.Groceries.GetNames(typeof(Grocery.Groceries)).Length; i++)
        {
            gottenGroceries.Add((Grocery.Groceries)i, Random.Range(0,2));
        }
    }

    void Start()
    {
        prevGroceries = new List<Grocery.Groceries>() { Grocery.Groceries.UNKNOWN, Grocery.Groceries.UNKNOWN, Grocery.Groceries.UNKNOWN, Grocery.Groceries.UNKNOWN};

        NewGroceryTargetSetup();
        ANIM_UpdateUI();    //To prevent having to play the animation on startup, the function will be called manually here.
        ANIM_UpdateImage();
    }


    public void TargetGroceryFound()
    {
        //Award the player with points.
        scoreHandler.PointsGained(CalculateScore());        
        anim.SetTrigger("NextItem");
    }    

    void ANIM_RequestNewItem()
    {
        NewGroceryTargetSetup();
        
        //groceryTimer.StopTimer();
    }

    /// <summary>
    /// Updates the UI so that the current item represents the actual current target using the 'next item'.
    /// </summary>
    void ANIM_UpdateUI()
    {
        currentShoppingListText.text = nextShoppingListText.text;
        
        groceryTimer.ResetTimer();
    }
    void ANIM_UpdateImage()
    {
        currentShoppingListImage.sprite = allGrocerySprites[(int)currentTargetGrocery - 1];
    }

    void NewGroceryTargetSetup()
    {        
        //Record gotten grocery
        AddGrocery(currentTargetGrocery);

        //Set a new target grocery        
        currentTargetGrocery = GetNewGrocery();

        //Visualize the next target for the player
        SetShoppingListUI();                
    }

    public string AccuiredGroceriesList()
    {
        string outP = "Aqcuired groceries: \n";

        foreach (KeyValuePair<Grocery.Groceries, int> groceryItem in gottenGroceries)
        { 
            outP += groceryItem.Key.ToString().Replace("_", " ") + ": " + groceryItem.Value + '\n';
        }

        return outP;
    }

    /// <summary>
    /// Grab a random new Grocery. The parameter determines if a specific grocery can't be chosen again.
    /// </summary>
    /// <returns></returns>
    void AddGrocery(Grocery.Groceries inp)
    {
        if (inp == Grocery.Groceries.UNKNOWN) return;

        gottenGroceries[inp]++;
        //Debug.Log(inp.ToString() + " picked up " + gottenGroceries[inp] + " times!");
    }

    Grocery.Groceries GetNewGrocery()
    {
        List<Grocery.Groceries> sorted = new List<Grocery.Groceries>(sortedList(gottenGroceries));    //Get a sorted version of the gottenGroceries

        sorted.RemoveRange(sorted.Count / 2, sorted.Count / 2);     //Remove the higher half

        foreach (Grocery.Groceries previousGrocery in prevGroceries)
        {
            sorted.Remove(previousGrocery);
        }

        Grocery.Groceries outP = sorted[UnityEngine.Random.Range(0, sorted.Count)]; //Pick a random grocery of the list
        return outP;
    }

    List<Grocery.Groceries> sortedList(Dictionary<Grocery.Groceries, int> inp)
    {
        System.Linq.IOrderedEnumerable<KeyValuePair<Grocery.Groceries, int>> sorted = from entry in inp orderby entry.Value ascending select entry;

        List<Grocery.Groceries> output = new List<Grocery.Groceries>();

        foreach (KeyValuePair<Grocery.Groceries, int> groceryItem in sorted)
        {
            output.Add(groceryItem.Key);
        }

        return output;
    }


    int CalculateScore()
    {
        float score = 100f;

        score += 150f * (groceryTimer.remainingTimeSeconds / groceryTimer.timeInSeconds);

        return (int)score;
    }

    void SetShoppingListUI()
    {
        nextShoppingListText.text = currentTargetGrocery.ToString().Replace("_", " ");
        //nextShoppingListText.text.Replace("-", " ");
    }    
}