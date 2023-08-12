using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public TextMeshProUGUI quantity_text;
    public TextMeshProUGUI itemName_text;
    public TextMeshProUGUI cakeGrade_text;

    public TMP_InputField price_IF;
    public GameObject CakeGradingCanvas;


    float reqQuantity;

    bool isBowl;

    // Start is called before the first frame update
    void Start()
    {
        isBowl = false;
        quantity_text.gameObject.SetActive(false);
        itemName_text.gameObject.SetActive(false);
    }

    public void ShowDisplays(GameObject item, string itemType, bool displayMode)
    {
      
        switch(itemType)
        {
            case "grocery":
                isBowl = false;
                BakingGroceryScript itemInfo = item.GetComponent<BakingGroceryScript>();
             
                if (itemInfo) { itemName_text.text = itemInfo.BakeItem.name; }

                quantity_text.text = Mathf.Round(itemInfo.BakeItem.currentQuantity).ToString();
                reqQuantity = itemInfo.BakeItem.reqQuantity;

                break;

            case "bowl":
                isBowl = true;
                BowlScript bowlScript = item.GetComponent<BowlScript>();
                itemName_text.text = item.name;
                quantity_text.text = Mathf.Round(bowlScript.currentQuantity).ToString();
                break;
        }

        itemName_text.gameObject.SetActive(displayMode);
        quantity_text.gameObject.SetActive(displayMode);
    }

    public void UpdateQuantity(float quantity)
    {
        if (!isBowl)
        {
            quantity_text.text = Mathf.Round(quantity).ToString() + " / " + reqQuantity;
        }
        else
        {
            quantity_text.text = Mathf.Round(quantity).ToString();
        }

    }
    
    public void GetPrice()
    {
        GameManager.priceSet = int.Parse(price_IF.text);
    }

    public void DisplayGradeScreen()
    {
        CakeGradingCanvas.SetActive(true);
        cakeGrade_text.text = "Grade : " + GameManager.FinalGrade;
    }
}
