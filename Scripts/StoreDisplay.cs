using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Xml;

public class StoreDisplay : MonoBehaviour, IUsable
{
    //public string Key => UniqueID;
    //public string UniqueID = "";

    public static int count;
    [SerializeField]
    int id;



    public int availableItemSlots;
    public int usedItemSlots;

    public ItemForSale itemForSale;

    [SerializeField]
    //public ItemObject heldItem;
    ItemObject heldItem;
    public ItemObject[] heldItems;

    public bool isEmpty;


    public Vector2 Position
    {
        get
        {
            return transform.position;
        }
    }

    /*
    public void Use(GameObject user)
    {

    }

    public void Highlight()
    {

    }
    */

    void Awake()
    {
        //availableItemSlots = size;
        id = count++;
        if(heldItems.Length > 0)
        {
            isEmpty = false;
            heldItem = heldItems[0];
            Debug.Log("StoreDisplay: " + id + "; " + "Init " + heldItem);
            itemForSale.CopyFromDatabase(heldItem);
            itemForSale.gameObject.GetComponent<SpriteRenderer>().sprite = itemForSale.itemSprite;
        }
        else
        {
            if(itemForSale != null)
                isEmpty = itemForSale.IsEmpty();
        }
    }

    public void FillDisplay(ItemForSale ifs)
    {
        isEmpty = itemForSale.IsEmpty();
        //if (ifs == null)
        if (isEmpty)
        {

            return;
        }
        else
        {
            // What if ifs == null?
            heldItem = ifs.itemForSale;

            itemForSale.CopyFromDatabase(heldItem);
            itemForSale.gameObject.GetComponent<SpriteRenderer>().sprite = itemForSale.itemSprite;

        }

    }

    // Checks if this storedisplay represents the customer wanting nothing
    public bool IsNothing()
    {
        return itemForSale.GetItemName() == "nothing" || itemForSale.GetItemID() == -1;
    }

    public void ClearDisplay()
    {
        itemForSale = null;
        heldItem = null;
        isEmpty = itemForSale.IsEmpty();
    }

    public Sprite GetSprite()
    {
        return itemForSale.itemSprite;
    }

    public int GetId()
    {
        return id;
    }

    void OnMouseDown()
    {
        //Debug.Log("Clicked store display");
    }

    void EditDisplay()
    {
        
    }

    void OnEnable()
    {
        if (itemForSale != null)
        {
            itemForSale.itemForSale = heldItem;
        }
    }
}
