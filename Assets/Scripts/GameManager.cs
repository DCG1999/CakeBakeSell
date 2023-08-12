using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static float Money;
    public static  float DailyExpenses = 25;
    GameObject[] itemsInShop;
    List<BakingGroceryScript> itemsPurchased;

    int currentScene;
    bool firstStartup;

    MobileScript mobileScript;

    public static string FinalGrade;
    public static int priceSet;

    ShopSoundPlayer shopSoundPlayer;

    private Button buyBtn;
    private Button confirmBtn;
    private void Awake()
    {
        // apply singleton

        print("this is called");
        FinalGrade = "";
        Money = 500;
        mobileScript.UpdateMoney();
        firstStartup = true;
        currentScene = 1; // as this script starts executing from scene 1
        DontDestroyOnLoad(this); // to keep the script running throughout the cycles

        itemsPurchased = new List<BakingGroceryScript>();
        itemsInShop = new GameObject[40];

        itemsInShop = GameObject.FindGameObjectsWithTag("Item");


    }

    private void Start()
    {
        foreach (GameObject item in itemsInShop)
        {
            item.GetComponent<BakingGroceryScript>().AssignOriginalValues();
            // initialize items original stats.
        }
    }
    public void BuyItem()
    {
        
        BakingGroceryScript bakingGroceryScript = ShopObjectSelectScript.selectedItem.GetComponent<BakingGroceryScript>();
        if (Money - bakingGroceryScript.BakeItem.Cost > 0)
        {
            shopSoundPlayer.PlaySound("Buy");
            if (ShopObjectSelectScript.selectedItem)
            {
                bakingGroceryScript.BakeItem.currentQuantity = bakingGroceryScript.originalQuantity;
                Money -= bakingGroceryScript.BakeItem.Cost;
                mobileScript.UpdateMoney();
                if (!firstStartup)
                {
                    foreach (BakingGroceryScript item in itemsPurchased)
                    {
                        if (item.BakeItem.name == bakingGroceryScript.BakeItem.name)
                        {
                            itemsPurchased.Remove(item);
                        }
                        itemsPurchased.Add(item);
                    }
                }
                else if (firstStartup)
                {
                    itemsPurchased.Add(bakingGroceryScript);
                }

                ShopObjectSelectScript.selectedItem.SetActive(false);
                ShopObjectSelectScript.selectedItem = null;
            }
        }
    }

    public void NextScene() // to create the game loop
    {
        firstStartup = false;
        if(currentScene < 3)
        { currentScene++; }
        else if(currentScene == 3)
        { currentScene = 1; }

        SceneManager.LoadScene(currentScene);
    }

    public void InitializeScene(int sceneNumber)
    {
        // these scene number are not scene index, they are parts of the game.
        switch(sceneNumber)
        {
            case 0:
                buyBtn = GameObject.Find("BuyBtn").GetComponent<Button>();
                confirmBtn = GameObject.Find("ConfirmBtn").GetComponent<Button>();
                buyBtn.onClick.AddListener(BuyItem);
                confirmBtn.onClick.AddListener(NextScene);
                shopSoundPlayer = GameObject.FindObjectOfType<ShopSoundPlayer>();
                mobileScript = GameObject.FindObjectOfType<MobileScript>();
                mobileScript.UpdateMoney();
                // update money on mobile whenever the scene is loaded
                GameObject.Find("MiddleClass_shop2").gameObject.SetActive(false);
                break;

            case 1:
                InventoryInstantiator inventoryInstantiator = GameObject.FindObjectOfType<InventoryInstantiator>();
                inventoryInstantiator.Spawninventory(itemsPurchased);
                // instantiate the purchased items 
                break;

            case 2:
                
                break;
        }
    }

    public void CollectGrade(string _grade)
    {
        FinalGrade = _grade;
    }
}
