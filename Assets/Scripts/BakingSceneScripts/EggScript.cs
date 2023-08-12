using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggScript : MonoBehaviour
{
    [SerializeField] GameObject eggPrefab;

    private GameObject[] egg;
    BakingGroceryScript bakingGroceryScript;

    int eggCounter;
    // Start is called before the first frame update
    void Start()
    {
        bakingGroceryScript = GetComponent<BakingGroceryScript>();
        eggCounter = (int)bakingGroceryScript.maxCapacity;
        egg = new GameObject[eggCounter];
        for(int i=0;i<bakingGroceryScript.BakeItem.currentQuantity;i++)
        {
            egg[i] = GameObject.Instantiate(eggPrefab, this.transform,false);
            egg[i].name = "Egg";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameObject GetEgg()
    {
        if (eggCounter >= 0)
        {
            eggCounter--;
            return egg[eggCounter];
        }
        else return null;
    }
}
