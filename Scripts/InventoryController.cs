using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Inventory Controller for Player
public class InventoryController : MonoBehaviour
{
    public static InventoryController Instance { get; private set; }

    // Inventory object; player inventory, demo inventory, merchant inventory
    // Set to demo/player inventory
    public InventoryObject inventoryObj;
    private GameObject inventoryView;
    private GameObject inventoryGrid;
    public bool isOpen;

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
    List<GameObject> itemsToDisplayList = new List<GameObject>();

    public ItemObject selectedInventoryItem;

    public TMP_Text inventoryText;

    void Awake()
    {
        Instance = this;
        
        isOpen = false;

        Debug.Log("JOOGUH");

        inventoryChanged = false;

        if (itemDatabaseObj == null)
        {
            Debug.Log("itemDatabaseObj is NULL!");
        }

        if(inventoryView == null)
            inventoryView = this.transform.GetChild(0).gameObject;

        if (inventoryGrid == null)
            inventoryGrid = this.transform.GetChild(0).GetChild(2).gameObject;

        if (Instance.IsOpen())
        {
            Debug.Log("INVENTORY VIEW OPEN");
            Instance.CloseInventory();
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
                int prevQty = inventoryObj.inventory.Items[i].quantity;
                //Debug.Log("INCREMENTING: " + inventoryObj.inventory.Items[i].quantity);
                inventoryObj.inventory.Items[i].quantity++;
                //Debug.Log("NOW: " + inventoryObj.inventory.Items[i].quantity);
                // create grid entry
                if(prevQty == 0)
                {
                    Debug.Log("Swobu" + id);
                    CreateGridItem(id);
                }
                inventoryChanged = true;
                UpdateDisplay(id);
                return;
            }
        }
    }

    public void ShowInventory()
    {
        if(isOpen)
        {
            Debug.Log("ALREADY OPEN");
            return;
        }
        Transform parent = inventoryGrid.transform;

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
            }
            // Check if Item exists in inventory but has different qty from previous inventory
            else
            {
                /*
                var obj = Instantiate(inventoryItemPrefab, parent);
                int qty = inventoryObj.inventory.Items[i].quantity;
                int id = inventoryObj.inventory.Items[i].itemID;
                obj.GetComponent<InventorySlotController>().itemID = inventoryObj.inventory.Items[i].itemID;
                obj.GetComponent<InventorySlotController>().itemQuantity = qty;
                //itemsDisplayed.Add(id, obj);
                */
            }
        }

        // Hide item with 0 qty from grid
        for (int i = 0; i < itemsToDisplayList.Count; ++i)
        {
            if (itemsToDisplayList[i].GetComponent<InventorySlotController>().itemQuantity == 0)
            {
                //HideItem(itemsToDisplayList[i]);
            }
        }
        if(!inventoryView.activeInHierarchy)
        {
            inventoryView.SetActive(true);
            isOpen = true;
        }
    }

    void HideItem(GameObject obj)
    {
        obj.SetActive(false);
    }

    void ShowItem(GameObject obj)
    {
        obj.SetActive(true);
    }


    public void CloseInventory()
    {
        inventoryView.SetActive(false);
        isOpen = false;
    }

    void CreateGridItem(int gridItemID)
    {
        Transform parent = inventoryGrid.transform;
        //Transform parent = this.transform.Find("Inventory Grid");

        var obj = Instantiate(inventoryItemPrefab, parent);
        int qty = inventoryObj.inventory.Items.Find(item => item.itemID == gridItemID).quantity;
        obj.GetComponent<InventorySlotController>().itemID = gridItemID;
        obj.GetComponent<InventorySlotController>().itemQuantity = qty;
        obj.transform.Find("ItemSprite").transform.Find("ItemQuantity").GetComponent<TMP_Text>().text = qty.ToString();
        obj.transform.Find("ItemSprite").GetComponent<Image>().sprite = itemDatabaseObj.getItem[inventoryObj.inventory.Items.Find(item => item.itemID == gridItemID).itemID].itemSprite;
        obj.transform.name = itemDatabaseObj.getItem[inventoryObj.inventory.Items.Find(item => item.itemID == gridItemID).itemID].itemName;
        itemsDisplayed.Add(gridItemID, obj);
        Debug.Log("Items displayed count : " + itemsDisplayed.Count);
    }

    public bool IsOpen()
    {
        isOpen = inventoryView.activeInHierarchy;
        return isOpen;

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

    void SortInventory()
    {

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
                //Debug.Log("Depleted Quantity; Destroying: " + temp);
                //HideItem(temp);
                Destroy(temp);
            }
            else
            {
                //Debug.Log("Quantity > 0");

                ShowItem(itemsDisplayed[itemID]);
            }
        }
        else
        {
            
            if(inventoryObj.inventory.Items.Find(item => item.itemID == itemID).quantity >  0)
            {
                Debug.Log("Creating new grid slot");

                CreateGridItem(itemID);
            }
        }
        //Debug.Log("not in itemsDisplayed");
        inventoryChanged = false;
    }
}
