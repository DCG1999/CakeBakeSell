using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopObjectSelectScript : MonoBehaviour
{
    private ShopManager shopManager;
    public Vector3 selectedItemPosOffset;
    [HideInInspector]public static GameObject selectedItem;


   // bool itemSelected;
    Vector3 itemOriginalLocalPos;
    Quaternion itemOriginalLocalRot;

    [SerializeField] ShopSoundPlayer shopSoundPlayer;
    void Awake()
    {
        shopManager = GameObject.FindObjectOfType<ShopManager>();
       // itemSelected = false;
        itemOriginalLocalPos = Vector3.zero;
    }


    void Update()
    {

       if(Input.touchCount > 0)
        { 
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
               // Debug.Log("Touch recorded");
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(touch.position),out hit))
                {              
                    if (!selectedItem)
                    {
                        if(hit.collider.tag == "Mobile")
                        {
                            //open mobile
                        }
                        else if (hit.collider.tag == "Item")
                        {
                           // itemSelected = true;
                            //Debug.Log(hit.collider.name);
                            selectedItem = hit.collider.gameObject;
                            shopManager.DisplayInfo();
                            itemOriginalLocalPos = selectedItem.transform.localPosition;
                            itemOriginalLocalRot = selectedItem.transform.rotation;

                            SelectItem();
                                                    
                        }
                    }
                    else if(hit.collider.tag != "Computer" && hit.collider.tag!= "Mobile" && hit.collider.tag != "Item" && selectedItem)
                    {
                        DeSelectItem();
                        shopManager.RemoveDisplayInfo();
                    }
                }
            }
        }
    }

    void SelectItem()
    {
        shopSoundPlayer.PlaySound("Select");
        selectedItem.transform.position += selectedItemPosOffset;
    }


    void DeSelectItem()
    {
        if (itemOriginalLocalPos != null)
        {
            shopSoundPlayer.PlaySound("Deselect");
            selectedItem.transform.localPosition = itemOriginalLocalPos;
            selectedItem.transform.localRotation = itemOriginalLocalRot;
         //   itemSelected = false;
            selectedItem = null;
        }
    }


}
