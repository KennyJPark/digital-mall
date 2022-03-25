using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Scriptable Object for Item objects for use in InventoryObject.cs
public enum ItemType
{
    Null,
    Food,
    Drink,
    Equipment,
    Commodity,
    Clothing,
    Electronic,
    Toy,
    Decoration
}

[CreateAssetMenu(fileName = "New Item", menuName = "Scripts/ScriptableObjects/Item")]
public class ItemObject : ScriptableObject
{
    public int itemID;
    public string itemName;
    public string itemDescription;
    public int basePrice;
    public int marketPrice;

    public GameObject prefab;

    public Sprite itemSprite;

    public ItemType itemType;


    public void PrintItem()
    {
        Debug.Log("### Print Item ###");
        Debug.Log("Item ID: " + itemID);
        Debug.Log("Name: " + itemName);
        Debug.Log("BasePrice: " + basePrice);
    }

}
