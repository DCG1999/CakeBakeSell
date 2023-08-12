using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public GameObject CartItemPrefab;
    public Transform CartContainer_UI;
   
    private List<GameObject> CartList;


    [SerializeField] TextMeshProUGUI ItemNameText;
    [SerializeField] TextMeshProUGUI ItemGradeText;
    [SerializeField] TextMeshProUGUI ItemCostText;
    [SerializeField] TextMeshProUGUI ItemExpiryText;
    [SerializeField] Image icon;


    [SerializeField] TextMeshProUGUI MoneyText;

    GameManager gameManager;
    void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        gameManager.InitializeScene(0);
        CartList = new List<GameObject>();

    }

     void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void DisplayInfo()
    {
        BakingItem selectedItemProperties = ShopObjectSelectScript.selectedItem.GetComponent<BakingGroceryScript>().BakeItem;

        icon.sprite = selectedItemProperties.icon;
        ItemNameText.text = selectedItemProperties.name.ToString();
        ItemCostText.text = "Price: $" + selectedItemProperties.Cost.ToString();
        ItemGradeText.text = "Grade: " + selectedItemProperties.itemGrade.ToString();
        ItemExpiryText.text = "Expiry: " + selectedItemProperties.ExpiryDuration.ToString();
    }

    public void RemoveDisplayInfo()
    {
        icon.sprite = null;
        ItemNameText.text = "";
        ItemCostText.text = "";
        ItemGradeText.text = "";
        ItemExpiryText.text = "";
    }

    public void DisplayCart()
    {

    }

}
