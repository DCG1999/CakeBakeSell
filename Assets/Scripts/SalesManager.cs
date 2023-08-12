using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SalesManager : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField]TextMeshProUGUI cakessoldText;
    [SerializeField]TextMeshProUGUI moneymadeText;
    [SerializeField]TextMeshProUGUI totalMoneyLeftText;
    private void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();

    }

    void Start()
    {
        gameManager.InitializeScene(2);
        CalculateAndDisplayProfit();
    }


    void CalculateAndDisplayProfit()
    {
        int unitsSold;
        switch(GameManager.FinalGrade)
        {
            case "A": 

                if(GameManager.priceSet > 40)
                {
                    if(Random.Range(0,100) > 95)
                    {
                        unitsSold = Random.Range(1, 11);
                    }
                    else
                    {
                        unitsSold = 0;
                    }
                }
                else if(GameManager.priceSet > 35)
                {
                    if(Random.Range(0,100) > 75)
                    {
                        unitsSold = Random.Range(8, 11);
                    }
                    else
                    {
                        unitsSold = Random.Range(4, 11);
                    }
                }
                else if(GameManager.priceSet > 20)
                {
                    if(Random.Range(0,100) > 55)
                    {
                        unitsSold = Random.Range(8, 11);
                    }
                    else
                    {
                        unitsSold = Random.Range(4, 11);
                    }
                }
                else
                {
                    unitsSold = Random.Range(9, 11);
                }
                
                break;


            case "B":
                if (GameManager.priceSet > 30)
                {
                    if (Random.Range(0, 100) > 95)
                    {
                        unitsSold = Random.Range(1, 11);
                    }
                    else
                    {
                        unitsSold = 0;
                    }
                }
                else if (GameManager.priceSet > 20)
                {
                    if (Random.Range(0, 100) > 75)
                    {
                        unitsSold = Random.Range(8, 11);
                    }
                    else
                    {
                        unitsSold = Random.Range(4, 11);
                    }
                }
                else if (GameManager.priceSet > 15)
                {
                    if (Random.Range(0, 100) > 55)
                    {
                        unitsSold = Random.Range(8, 11);
                    }
                    else
                    {
                        unitsSold = Random.Range(4, 11);
                    }
                }
                else
                {
                    unitsSold = Random.Range(9, 11);
                }
                break;


            case "C":
                if (GameManager.priceSet > 10)
                {
                    unitsSold = 0;
                }
                else
                {
                    unitsSold = Random.Range(1, 5);
                }
                break;
            default:
                unitsSold = 0;
                break;
        }

        int moneyMade = GameManager.priceSet * unitsSold;
        cakessoldText.text = "Cakes Sold : " + unitsSold.ToString();
        moneymadeText.text = "Money Made : " + moneyMade.ToString();
        GameManager.Money += moneyMade;
      
        GameManager.Money -= GameManager.DailyExpenses;
        totalMoneyLeftText.text = "Total Money Left : "+GameManager.Money.ToString();
    }

    public void EndDay()
    {
        gameManager.NextScene();
    }
}
