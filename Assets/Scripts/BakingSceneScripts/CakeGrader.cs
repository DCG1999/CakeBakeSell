using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CakeGrader : MonoBehaviour
{

    public static float points;
    List<KeyValuePair<string, string>> bowl1recipe = new List<KeyValuePair<string, string>>();
    List<KeyValuePair<string, string>> bowl2recipe = new List<KeyValuePair<string, string>>();


    List<KeyValuePair<string, string>> bowl1current = new List<KeyValuePair<string, string>>();
    List<KeyValuePair<string, string>> bowl2current = new List<KeyValuePair<string, string>>();

    float averageQualityPoints;
    bool PerfectRun;

    string FinalGrade;

    GameManager gameManager;
    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();   
        bowl1recipe.Add(new KeyValuePair<string, string>("Flour", "290"));
        bowl1recipe.Add(new KeyValuePair<string, string>("Baking Powder", "8"));
        bowl1recipe.Add(new KeyValuePair<string, string>("Baking Soda", "2"));
        bowl1recipe.Add(new KeyValuePair<string, string>("Salt", "2"));
        bowl1recipe.Add(new KeyValuePair<string, string>("Sugar", "300"));
        bowl1recipe.Add(new KeyValuePair<string, string>("Egg", "3"));
        bowl1recipe.Add(new KeyValuePair<string, string>("Oil", "120"));
        bowl1recipe.Add(new KeyValuePair<string, string>("Buttermilk", "120"));
        bowl1recipe.Add(new KeyValuePair<string, string>("Whisk", "Mix"));  // total item name point 45 // total amount point 45 // total 90

        // divide in to cake pans
        // add to oven

        bowl2recipe.Add(new KeyValuePair<string, string>("Butter", "150"));
        bowl2recipe.Add(new KeyValuePair<string, string>("Cream Cheese", "400"));
        bowl2recipe.Add(new KeyValuePair<string, string>("Icing Sugar", "160"));
        bowl2recipe.Add(new KeyValuePair<string, string>("Vanilla Extract", "8"));
        bowl2recipe.Add(new KeyValuePair<string, string>("Heavy Cream", "120"));
        bowl2recipe.Add(new KeyValuePair<string, string>("Whisk", "Mix")); // total item name point  30 // 30 // 60


        // total perfect run with all A grade items should be 90+60+50 = 200 
        // total perfect run with all B grade items should be 90+60+25 = 175
    }

    private void Start()
    {
        //    GradeCake(false, true);
    }


    public void GradeCake()
    {
        BowlScript bowl1Script = GameObject.Find("Bowl1").GetComponent<BowlScript>();
        BowlScript bowl2Script = GameObject.Find("Bowl2").GetComponent<BowlScript>();

        bowl1current = bowl1Script.bowl1current;
        bowl2current = bowl2Script.bowl2current;

        //bowl1current = bowl1recipe;
        //bowl2current = bowl1recipe;

        PerfectRun = CheckForPerfectRecipe(bowl1current, bowl1recipe) && CheckForPerfectRecipe(bowl2current, bowl2recipe);

        if (!PerfectRun)
        {
            NonPerfectRunGrader(bowl1current, bowl1recipe);
            NonPerfectRunGrader(bowl2current, bowl2recipe);
        }

        averageQualityPoints = (bowl1Script.totalIngredientQuality + bowl1Script.totalIngredientQuality) / (bowl1current.Count + bowl2current.Count);
        points += averageQualityPoints;

        print(points);
        print(PerfectRun);

        gameManager.CollectGrade(CalculateFinalGrade());
    }

    bool CheckForPerfectRecipe(List<KeyValuePair<string, string>> currentRecipe, List<KeyValuePair<string, string>> perfectRecipe)
    {
        print(currentRecipe.Count);
        bool perfectRun = false;

        for (int i = 0; i < currentRecipe.Count; i++)
        {
            perfectRun = false;
            if (perfectRecipe[i].Key == currentRecipe[i].Key)
            {
                perfectRun = true;
                points += 5;
            }
            else
            {
                perfectRun = false;
                break;
            }

            if (perfectRecipe[i].Value == currentRecipe[i].Value)
            {
                points += 5;
            }

            if (currentRecipe[i].Key != "Whisk")
            {
                if (int.Parse(perfectRecipe[i].Value) > int.Parse(currentRecipe[i].Value) + 2 || int.Parse(perfectRecipe[i].Value) < int.Parse(currentRecipe[i].Value) - 2)
                {
                    points += 3;
                }
            }
        }

        return perfectRun;
    }

    void NonPerfectRunGrader(List<KeyValuePair<string, string>> currentRecipe, List<KeyValuePair<string, string>> perfectRecipe)
    {
        foreach (KeyValuePair<string, string> itemAdded in currentRecipe)
        {
            if (perfectRecipe.Contains(itemAdded))
            {
                points += 3;
            }

            foreach (KeyValuePair<string, string> itemInRecipe in perfectRecipe)
            {
                if (itemInRecipe.Key == itemAdded.Key && itemInRecipe.Key != "Whisk")
                {
                    if (int.Parse(itemInRecipe.Value) > int.Parse(itemAdded.Value) + 2 || int.Parse(itemInRecipe.Value) < int.Parse(itemAdded.Value) - 2)
                    {
                        points += 1;
                    }
                }
            }
        }
    }

    public string CalculateFinalGrade()
    {
        if (points <= 200 && points > 175)
        {
            return "A";
        }
        else if (points <= 175 && points > 150)
        {
            return "B";
        }
        else
        {
            return "C";
        }
    }
}




 
