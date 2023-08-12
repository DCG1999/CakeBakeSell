using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInstantiator : MonoBehaviour
{
    Transform t_inventoryPlaceholder;
    [SerializeField] Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        t_inventoryPlaceholder = transform;
    }

    public void Spawninventory(List<BakingGroceryScript> items)
    {
        foreach(BakingGroceryScript item in items)
        {
            GameObject inventoryItem = null;

            if (item.BakeItem.itemGrade.ToString() == "A")
            {
                 inventoryItem = Instantiate(Resources.Load<GameObject>("GradeAItems/" + item.BakeItem.name));
            }
            else if (item.BakeItem.itemGrade.ToString() == "B")
            {
                 inventoryItem = Instantiate(Resources.Load<GameObject>("GradeBItems/" + item.BakeItem.name));
            }

            inventoryItem.transform.position  = t_inventoryPlaceholder.position + offset * items.IndexOf(item);
            inventoryItem.name = item.BakeItem.ItemName;
        }
    }
}
