using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ItemCondition
{
    InStock,
    Sold,
    Unavailable
}

// GameObject representation of an ItemObject
public class ItemForSale : MonoBehaviour
{
    public event Action ItemSold;

    public int itemID;
    //int itemID;
    public string itemName;
    //string itemName;
    public string itemDescription;
    //public float basePrice;
    public int basePrice;
    //public float marketPrice;
    public int marketPrice;

    public ItemDatabaseObject itemDatabaseObj;
    public GameObject prefab;
    public FloatingUI floatingUI;
    public SpriteRenderer spriteRenderer;

    public Sprite itemSprite;
    
    public ItemType itemType;
    public ItemCondition itemCondition;
    [SerializeField]
    bool isEmpty;

    public ItemObject itemForSale;
    [SerializeField] 
    GameObject storeDisplayObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        if(storeDisplayObj == null)
        {
            storeDisplayObj = transform.parent.gameObject;
        }
        if(spriteRenderer == null)
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }
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

    public void RemoveItemFromDisplay()
    {
        ClearFields();
    }

    void ClearFields()
    {
        Debug.Log("Clear ItemForSale Fields");

        itemID = -1;
        itemName = null;
        itemDescription = null;
        basePrice = 0;
        marketPrice = 0;
        prefab = null;
        itemSprite = null;
        itemType = ItemType.Null;
        isEmpty = true;
        itemCondition = ItemCondition.Sold;
        itemForSale = null;
        if(TryGetComponent<SpriteRenderer>(out SpriteRenderer sprite))
        {
            sprite = null;
        }
        //gameObject.TryGetComponent<SpriteRenderer>().sprite = null;
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
        itemCondition = ItemCondition.InStock;
        if(itemSprite != null)
        {
            floatingUI.enabled = true;
            spriteRenderer.sprite = itemSprite;
        }
    }

    void FillFields(int id)
    {
        // Get item info using item id

        itemForSale = itemDatabaseObj.getItem[id];
        //itemForSale.PrintItem();
        CopyFromDatabase(itemForSale);
    }

    public string GetItemName()
    {
        return itemName;
    }

    public int GetItemID()
    {
        return itemID;
    }

    public int GetBasePrice()
    {
        return basePrice;
    }

    public ItemCondition GetItemCondition()
    {
        return itemCondition;
    }

    public void PrintItemForSale()
    {
        Debug.Log(itemName);
    }

    public void SellItem()
    {
        if(ItemSold != null)
        {
            ItemSold.Invoke();
        }
       
        spriteRenderer.sprite = null;
        RemoveItemFromDisplay();
        floatingUI.enabled = false;
        //Destroy(gameObject);
    }

    void OnEnable()
    {

    }
}
