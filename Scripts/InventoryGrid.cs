using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Threading.Tasks;

/*
// To be used with Inventory Grid ... or Inventory?
public class InventoryGridController : MonoBehaviour
{
    //public InventorySlotController currentSelectedItem;

    public delegate void ClickAction();
    public static event ClickAction OnClicked;

    public delegate void UnclickAction();
    public static event UnclickAction OnUnclicked;

    CancellationTokenSource tokenSource;

    public GameObject currentSelectedItem;
    public Sprite selectedSprite;
    public GameObject[] inventorySlots;

    //public GameObject interactingStoreDisplay;
    public StoreDisplay interactingStoreDisplay;

    public bool promptItemForSale;

    enum InventoryOwner
    {
        player,
        merchant,
        npc
    }

    void Start()
    {
        tokenSource = new CancellationTokenSource();
    }

    void Awake()
    {
        currentSelectedItem = null;
        promptItemForSale = false;


    }

    public void AssignStoreDisplay(StoreDisplay display)
    {
        Debug.Log("Assigning display");
        interactingStoreDisplay = display;
    }

    public void UnassignStoreDisplay()
    {
        Debug.Log("Unassigning display");
        interactingStoreDisplay = null;
    }

    public void SelectItem(GameObject item)
    {
        if (currentSelectedItem == null)
        {
            currentSelectedItem = item;
            Debug.Log("currentSelectedItem: " + currentSelectedItem);
            currentSelectedItem.GetComponent<Image>().overrideSprite = selectedSprite;

        }
        else if (currentSelectedItem == item || item == null)
        {
            DeselectItem(item);
        }
        else if (currentSelectedItem != item)
        {
            currentSelectedItem.GetComponent<Image>().overrideSprite = null;
            currentSelectedItem = item;
            Debug.Log("currentSelectedItem: " + currentSelectedItem);
            currentSelectedItem.GetComponent<Image>().overrideSprite = selectedSprite;

            //SwapItems(item);
        }
        
        if(currentSelectedItem != null && interactingStoreDisplay != null)
        {
            if (OnClicked != null)
                OnClicked();
        }

    }

    public void DeselectItem(GameObject item)
    {
        Debug.Log("Deselecting: " + currentSelectedItem);
        currentSelectedItem.GetComponent<Image>().overrideSprite = null;
        currentSelectedItem = null;
        if(OnUnclicked != null)
            OnUnclicked();
    }

    // Use when dragging mouse?
    //void SwapItems(InventorySlotController item)
    void SwapItems(GameObject item)
    {
        // Swap position
        Vector3 temp = currentSelectedItem.transform.position;
        currentSelectedItem.transform.position = item.transform.position;
        item.transform.position = temp;

        // Swap Sibling Index
        int tempIndex = currentSelectedItem.transform.GetSiblingIndex();
        currentSelectedItem.transform.SetSiblingIndex(item.transform.GetSiblingIndex());
        item.transform.SetSiblingIndex(tempIndex);

        Debug.Log("swapping " + currentSelectedItem + " with " + item);
        currentSelectedItem.GetComponent<Image>().overrideSprite = null;
        currentSelectedItem = null;

    }

}
*/