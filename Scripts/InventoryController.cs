using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Inventory Controller for Player
public class InventoryController : MonoBehaviour
{
    public static InventoryController instance { get; private set; }

    // Inventory object; player inventory, demo inventory, merchant inventory
    // Set to demo/player inventory
    public InventoryObject inventoryObj;

    // Inventory slots to populate Inventory Grid
    public GameObject inventoryItemPrefab;
    //public GameObject editingStoreDisplay;

    public static Dictionary<ItemObject, int> inventory = new Dictionary<ItemObject, int>();
    public static Dictionary<int, ItemObject> idInventory = new Dictionary<int, ItemObject>();

    private bool inventoryChanged;

    public ItemDatabaseObject itemDatabaseObj;
    // Might not need InventorySlot as part of itemsDisplayed
    //Dictionary<int, (InventorySlot, GameObject)> itemsDisplayed = new Dictionary<int, (InventorySlot, GameObject)>();
    Dictionary<int, GameObject> itemsDisplayed = new Dictionary<int, GameObject>();
    //Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();


    public ItemObject selectedInventoryItem;

    public TMP_Text inventoryText;

    void Awake()
    {
        instance = this;
        inventoryChanged = false;

        if (itemDatabaseObj == null)
        {
            Debug.Log("itemDatabaseObj is NULL!");
        }
        /*
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        */
    }

    void Update()
    {

    }

    public void DecrementItem(int id)
    {
        for (int i = 0; i < inventoryObj.inventory.Items.Count; ++i)
        {
            if(inventoryObj.inventory.Items[i].itemID == id)
            {
                inventoryObj.inventory.Items[i].quantity--;
                inventoryChanged = true;
                UpdateDisplay(id);
                return;
            }
        }
    }

    public void IncrementItem(int id)
    {
        for (int i = 0; i < inventoryObj.inventory.Items.Count; ++i)
        {
            if (inventoryObj.inventory.Items[i].itemID == id)
            {
                inventoryObj.inventory.Items[i].quantity++;
                inventoryChanged = true;
                UpdateDisplay(id);
                return;
            }
        }
    }

    public void ShowInventory()
    {
        Transform parent = this.transform.Find("Inventory Grid");

        for (int i = 0; i < inventoryObj.inventory.Items.Count; ++i)
        {
            if (!itemsDisplayed.ContainsKey(inventoryObj.inventory.Items[i].itemID) && inventoryObj.inventory.Items[i].quantity > 0)
            //if (!itemsDisplayed.ContainsKey(inventoryObj.inventory.Items[i].itemID) && inventoryObj.inventory.Items[i].quantity > 0)
            {
                // If item in inventory doesn't already exist in itemsDisplayed create new GameObject "obj"
                // Assign values such as itemID, quantity, sprite, itemName to "obj"
                // Add obj to itemsDisplayed
                var obj = Instantiate(inventoryItemPrefab, parent);
                int qty = inventoryObj.inventory.Items[i].quantity;
                int id = inventoryObj.inventory.Items[i].itemID;
                obj.GetComponent<InventorySlotController>().itemID = inventoryObj.inventory.Items[i].itemID;
                obj.GetComponent<InventorySlotController>().itemQuantity = qty;
                obj.transform.Find("ItemSprite").transform.Find("ItemQuantity").GetComponent<TMP_Text>().text = qty.ToString();
                obj.transform.Find("ItemSprite").GetComponent<Image>().sprite = itemDatabaseObj.getItem[inventoryObj.inventory.Items[i].itemID].itemSprite;
                obj.transform.name = itemDatabaseObj.getItem[inventoryObj.inventory.Items[i].itemID].itemName;
                itemsDisplayed.Add(id, obj);
                //itemsDisplayed.Add(id, (inventoryObj.inventory.Items[i], obj));

            }
            // Check if Item exists in inventory but has different qty from previous inventory

        }
        if(!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }
    }

    public void CloseInventory()
    {
        gameObject.SetActive(false);
    }

    void CreateGridItem(int gridItemID)
    {
        Transform parent = this.transform.Find("Inventory Grid");

        var obj = Instantiate(inventoryItemPrefab, parent);
        int qty = inventoryObj.inventory.Items.Find(item => item.itemID == gridItemID).quantity;
        obj.GetComponent<InventorySlotController>().itemID = gridItemID;
        obj.GetComponent<InventorySlotController>().itemQuantity = qty;
        obj.transform.Find("ItemSprite").transform.Find("ItemQuantity").GetComponent<TMP_Text>().text = qty.ToString();
        obj.transform.Find("ItemSprite").GetComponent<Image>().sprite = itemDatabaseObj.getItem[inventoryObj.inventory.Items.Find(item => item.itemID == gridItemID).itemID].itemSprite;
        obj.transform.name = itemDatabaseObj.getItem[inventoryObj.inventory.Items.Find(item => item.itemID == gridItemID).itemID].itemName;
        itemsDisplayed.Add(gridItemID, obj);
    }

    // When something changes in inventory, update display
    public void UpdateDisplay()
    {
        Debug.Log("Updating Display");
        //Debug.Log(itemsDisplayed);
        // PROBLEM HERE
        // Iterate through inventoryObj instead

        inventoryChanged = false;
    }

    public void UpdateDisplay(int itemID)
    {
        if(itemsDisplayed.ContainsKey(itemID))
        {
            // Update quantity in GameObject and for UI
            // Destroy GameObject if quantity is 0
            itemsDisplayed[itemID].GetComponent<InventorySlotController>().itemQuantity = inventoryObj.inventory.Items.Find(item => item.itemID == itemID).quantity;
            itemsDisplayed[itemID].transform.Find("ItemSprite").transform.Find("ItemQuantity").GetComponent<TMP_Text>().text = itemsDisplayed[itemID].GetComponent<InventorySlotController>().itemQuantity.ToString();
            if (itemsDisplayed[itemID].GetComponent<InventorySlotController>().itemQuantity == 0)
            {
                GameObject temp = itemsDisplayed[itemID];
                itemsDisplayed.Remove(itemID);
                Debug.Log("Depleted Quantity; Destroying: " + temp);
                Destroy(temp);
            }
        }
        else
        {
            if(inventoryObj.inventory.Items.Find(item => item.itemID == itemID).quantity >  0)
            {
                CreateGridItem(itemID);
            }
        }
        inventoryChanged = false;
    }
}
