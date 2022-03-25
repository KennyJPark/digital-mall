using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GameObject representation of an ItemObject
public class ItemForSale : MonoBehaviour
{
    public int itemID;
    public string itemName;
    public string itemDescription;
    //public float basePrice;
    public int basePrice;
    //public float marketPrice;
    public int marketPrice;

    public ItemDatabaseObject itemDatabaseObj;
    public GameObject prefab;

    public Sprite itemSprite;

    public ItemType itemType;

    public bool isEmpty;

    public ItemObject itemForSale;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        ClearFields();
    }

    public bool IsEmpty()
    {
        return isEmpty;
        //
    }

    public void PopulateFields(int id)
    {
        if(id == -1)
        {
            ClearFields();
        }
        else
        {
            ClearFields();
            FillFields(id);
        }

    }

    public void RemoveItem()
    {
        ClearFields();
    }

    void ClearFields()
    {
        Debug.Log("Clear Fields");

        itemID = -1;
        itemName = null;
        itemDescription = null;
        basePrice = 0;
        marketPrice = 0;
        prefab = null;
        itemSprite = null;
        itemType = ItemType.Null;
        isEmpty = true;
    }

    public void CopyFromDatabase(ItemObject item)
    {
        Debug.Log("CopyFromDatabase");
        Debug.Log(item.itemName + " " + item.itemID);

        itemID = item.itemID;
        itemName = item.itemName;
        itemDescription = item.itemDescription;
        basePrice = item.basePrice;
        prefab = item.prefab;
        itemSprite = item.itemSprite;
        itemType = item.itemType;
        isEmpty = false;
    }

    void FillFields(int id)
    {
        // Get item info using item id

        itemForSale = itemDatabaseObj.getItem[id];
        //itemForSale.PrintItem();
        CopyFromDatabase(itemForSale);
    }

    public void PrintItemForSale()
    {
        Debug.Log(itemName);
    }
}
