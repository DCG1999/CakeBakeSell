using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakingGroceryScript : MonoBehaviour
{
    public BakingItem BakeItem;

    [HideInInspector] public float amountPoured = 0;
    [HideInInspector] public float tempAmountPoured = 0;
    [HideInInspector] public float originalQuantity;
    [HideInInspector] public float maxCapacity;

    public float pourRate;
 

    private void Awake()
    {
        maxCapacity = BakeItem.currentQuantity; // maxCapacity is extracted from BakingItem ScriptableObject
    }

    public void AssignOriginalValues()
    {
        originalQuantity = BakeItem.currentQuantity; // called from GameManager Awake
        print(originalQuantity);
    }
}
