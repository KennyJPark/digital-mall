using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreDisplay : Furniture
{
    public static int count;
    public int id;

    public int availableItemSlots;
    public int usedItemSlots;

    public ItemForSale itemForSale;

    public ItemObject heldItem;
    public ItemObject[] heldItems;

    public bool isEmpty;

    void Awake()
    {
        availableItemSlots = size;
        id = count++;
        if(heldItems.Length > 0)
        {
            isEmpty = false;
            heldItem = heldItems[0];
            Debug.Log("Init " + heldItem);
            itemForSale.CopyFromDatabase(heldItem);
            itemForSale.gameObject.GetComponent<SpriteRenderer>().sprite = itemForSale.itemSprite;
        }
        else
        {
            isEmpty = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FillDisplay(ItemForSale ifs)
    {
        if(ifs == null)
        {
            isEmpty = true;
            return;
        }
        else
        {
            heldItem = ifs.itemForSale;
            itemForSale.CopyFromDatabase(heldItem);
            itemForSale.gameObject.GetComponent<SpriteRenderer>().sprite = itemForSale.itemSprite;
            isEmpty = false;
        }

    }

    public Sprite GetSprite()
    {
        return itemForSale.itemSprite;
    }

    void OnMouseDown()
    {
        //Debug.Log("Clicked store display");
    }

    void EditDisplay()
    {
        
    }
}
