using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Required when using Event data

public class InventorySlotController : MonoBehaviour, IPointerDownHandler
{
    public bool selected;
    public static Vector3 mousePos;
    public int itemID;
    public int itemQuantity;

    void Awake()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log(this.gameObject.name + " Was Clicked.");
        //Debug.Log("item ID: " + itemID);
        //Debug.Log("Sibling Index: " + this.transform.GetSiblingIndex());
        //InventoryController.instance.SelectItem(this);
        //transform.parent.GetComponent<InventoryController>().SelectItem(this);
        transform.parent.GetComponent<InventoryGridController>().SelectItem(gameObject);
    }

    void UpdateInventoryDisplay()
    {
        transform.parent.GetComponent<InventoryController>().UpdateDisplay();
    }
}
