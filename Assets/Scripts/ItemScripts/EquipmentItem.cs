using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmItem", menuName = "EquipmentItemSO")]
public class EquipmentItem : ScriptableObject
{
    public string ItemName;
    public int cost;
}
