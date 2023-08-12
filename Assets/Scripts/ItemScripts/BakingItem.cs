using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BakeItem", menuName = "ItemSO")]
public class BakingItem : ScriptableObject
{
    public Sprite icon;
    public string ItemName;
    public enum ItemGrades { A, B, C };
    public ItemGrades itemGrade;

    public int Cost;
    public int ExpiryDuration;

    public float currentQuantity;
    public float reqQuantity;

}
