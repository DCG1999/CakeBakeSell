using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlScript : MonoBehaviour
{

    public float currentQuantity = 0;
    public bool EnoughBatter;

    public float totalIngredientQuality = 0;

    public List<KeyValuePair<string, string>> bowl1current = new List<KeyValuePair<string, string>>();
    public List<KeyValuePair<string, string>> bowl2current = new List<KeyValuePair<string, string>>();

    public void AddToBowl(GameObject bowl, string _ingredient, string _quantity, string _grade)
    {

        print("bowl " + bowl.name + " ig: " + _ingredient + " quant: " + _quantity);

        switch (bowl.name)
        {
            case "Bowl1":

                CheckAndAddItems(bowl1current, _ingredient, _quantity, _grade);
                break;

            case "Bowl2":
                CheckAndAddItems(bowl2current, _ingredient, _quantity, _grade);
                break;

        }
    }

    void CheckAndAddItems(List<KeyValuePair<string, string>> _bowl, string _ingredient, string _quantity, string _grade)
    {

        bool itemFound = false;

        foreach (KeyValuePair<string, string> ingredient in _bowl) // checks if item has been added previously, if yes then it replaces the entry and adds the new quantity with the old quantity
        {
            if (ingredient.Key == _ingredient)
            {
                itemFound = true;
                int index = _bowl.IndexOf(ingredient);

                if(_ingredient == "Whisk")
                {
                    _bowl[index] = new KeyValuePair<string, string>(_ingredient, _quantity);
                    print("ig added  " + _ingredient + "quant added " + _quantity);
                }
                else
                {
                    float quantity = float.Parse(ingredient.Value);
                    quantity += float.Parse(_quantity);
                    _bowl[index] = new KeyValuePair<string, string>(_ingredient, quantity.ToString());
                    print("ig added  " + _ingredient + " quant added " + quantity);
                }
                break;
            }

        }
        if (!itemFound)
        {
            _bowl.Add(new KeyValuePair<string, string>(_ingredient, _quantity.ToString()));
            switch(_grade)
            {
                case "A": totalIngredientQuality += 50;
                    break;

                case "B": totalIngredientQuality += 25;
                    break;
            }
            
        }
    }

    public bool CheckMinReqForFrostingCake()
    {
        if(currentQuantity > 750) // this value is minimum required batter amount in bowl2 to create a frosting layer
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
