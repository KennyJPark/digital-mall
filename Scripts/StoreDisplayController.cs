using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

// Provides functionality for interacting with Player's Store Displays
public class StoreDisplayController : MonoBehaviour
{
    public static StoreDisplayController instance { get; private set; }

    public PlayerController playerInstance;

    bool choiceMade;

    // delegate for when player clicks "Confirm" on a Store Display
    // Listeners for this event include StoreDisplayCollection.cs to update its items for sale
    public delegate void ConfirmAction();
    public static event ConfirmAction OnConfirm;

    // delegate for when any store display is updated
    public delegate void UpdateAction();
    public static event UpdateAction OnUpdate;

    // Item Display Box
    public GameObject storeDisplayDialog;

    //
    public GameObject selectedStoreDisplay;
    public GameObject selectedInventorySlot;

    // ItemSlot/Add Button
    public GameObject itemSlotObject;

    // InventoryCanvas
    public GameObject inventoryWindow;

    // InventoryController
    public GameObject playerInventory;

    // InventoryGridController
    public GameObject inventoryGrid;

    public GameObject itemForSaleObj;
    public ItemForSale itemForSale;
    public Sprite itemForSaleSprite;

    // Currently selected Item in Player's inventory; Unused?
    public GameObject selectedItem;

    public Button confirmButton;
    bool confirmEnabled;
    public Button addItemButton;
    public Button removeItemButton;
    public Button swapItemButton;
    public Button undoChangeButton;
    public Button resetChangesButton;

    Color32 GRAY = new Color32(142, 142, 142, 255);
    Color32 confirmButtonOriginalColor;
    Color32 removeItemButtonOriginalColor;
    Color32 swapItemButtonOriginalColor;
    Color32 undoChangeButtonOriginalColor;
    Color32 resetChangesButtonOriginalColor;

    // Store Display the Player is interacting with
    public StoreDisplay currentInteractingStoreDisplay;
    public GameObject storeDisplayList;
    //public List<GameObject> storeDisplayList;
    public List<ItemForSale> itemForSaleList;
    //public StoreDisplay[] storeDisplays;


    void Start()
    {
        //Debug.Log("Start");
    }

    void Awake()
    {
        //Debug.Log("Awake");

        instance = this;
        choiceMade = false;


        if (confirmButton != null)
            confirmButton.onClick.AddListener(CallConfirm);

        addItemButton.onClick.AddListener(AddItem);
        removeItemButton.onClick.AddListener(RemoveItem);
        swapItemButton.onClick.AddListener(SwapItem);

        if(undoChangeButton != null)
            undoChangeButton.onClick.AddListener(UndoChange);
        
        if (resetChangesButton != null)
            resetChangesButton.onClick.AddListener(ResetChanges);

        confirmButtonOriginalColor = confirmButton.GetComponent<Image>().color;
        removeItemButtonOriginalColor = removeItemButton.GetComponent<Image>().color;
        swapItemButtonOriginalColor = swapItemButton.GetComponent<Image>().color;
        undoChangeButtonOriginalColor = undoChangeButton.GetComponent<Image>().color;
        resetChangesButtonOriginalColor = resetChangesButton.GetComponent<Image>().color;

        undoChangeButton.GetComponent<Image>().color = GRAY;
        resetChangesButton.GetComponent<Image>().color = GRAY;

        confirmEnabled = true;
        if (itemForSale == null)
        {
            removeItemButton.GetComponent<Image>().color = GRAY;
            swapItemButton.GetComponent<Image>().color = GRAY;
        }

        if (storeDisplayDialog == null)
        {

        }
        else
        {
            storeDisplayDialog.SetActive(false);
        }
        UpdateDisplayList();
    }

    void OnEnable()
    {
        Debug.Log("OnEnable");
        choiceMade = false;
        //UpdateDisplayList();
    }

    // Update is called once per frame
    void Update()
    {
        if (storeDisplayDialog.activeInHierarchy)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                CancelMenu();
            }

            ///*
            if (Input.GetButtonDown("Interact"))
            {
                confirmButton.onClick.Invoke();
            }
            //*/

            if (Input.GetKeyDown("k"))
            {
                confirmButton.onClick.Invoke();
            }

        }
    }

    void FixedUpdate()
    {

    }

    public IEnumerator InteractWithStoreDisplay(StoreDisplay display)
    //public void InteractWithStoreDisplay(StoreDisplay display)
    {
        confirmEnabled = true;
        choiceMade = false;
        //Debug.Log("Inside Interact");
        currentInteractingStoreDisplay = display;
        itemForSale = currentInteractingStoreDisplay.GetComponentInChildren<ItemForSale>();

        if (!currentInteractingStoreDisplay.GetComponentInChildren<ItemForSale>().IsEmpty())
        //if (!currentInteractingStoreDisplay.GetComponentInChildren<ItemForSale>().
        {
            LoadDisplay(currentInteractingStoreDisplay);
        }
        // Empty Item For Sale
        else
        {
            //Debug.Log(currentInteractingStoreDisplay.GetComponentInChildren<ItemForSale>().IsEmpty());
        }

        if (storeDisplayDialog.activeInHierarchy)
        {
            storeDisplayDialog.SetActive(false);
            //PlayerController.ReleasePlayer();
        }
        else
        {
            //await PromptItemForDisplayAsync();
            //StartCoroutine(PromptItemForDisplay());
            yield return StartCoroutine(PromptItemForDisplay());
            ///*
            Debug.Log("Start While Loop");
            while (!choiceMade)
            {
                yield return null;
            }
            //*/
            Debug.Log("End While Loop");
            // Position the dialog relative to the Store Display object
            //storeDisplayDialog.transform.position = display.transform.position;
            //storeDisplayDialog.GetComponent<RectTransform>().position = display.transform.position;
        }
        Debug.Log("End InteractWithStoreDisplay");
        yield return null;
    }

    // Display Store Display window and await player input
    public IEnumerator PromptItemForDisplay()
    {
        
        storeDisplayDialog.SetActive(true);
        // Open player's inventory
        inventoryWindow.SetActive(true);
        InventoryController.instance.ShowInventory();
        if (inventoryGrid != null)
            inventoryGrid.GetComponent<InventoryGridController>().AssignStoreDisplay(currentInteractingStoreDisplay);
        InventoryGridController.OnClicked += SetItemForSale;
        //InventoryGridController.OnUnclicked += UndoChange;
        

        Debug.Log("End PromptItemForDisplay");
        yield break;
        //yield return null;
    }

    /*
    public async Task PromptItemForDisplayAsync()
    {
        // Display Store Display window and await player input
        storeDisplayDialog.SetActive(true);
        // Open player's inventory
        inventoryWindow.SetActive(true);
        InventoryController.instance.ShowInventory();
        if(inventoryGrid != null)
            inventoryGrid.GetComponent<InventoryGridController>().AssignStoreDisplay(currentInteractingStoreDisplay);
        InventoryGridController.OnClicked += SetItemForSale;
        //InventoryGridController.OnUnclicked += UndoChange;
    }
    */
    // Tentatively add item for sale
    public void SetItemForSale()
    {
        if (inventoryGrid != null)
        {
            selectedInventorySlot = inventoryGrid.GetComponent<InventoryGridController>().currentSelectedItem;


            // Fill in Empty Slot with Selected Item Sprite
            //currentInteractingStoreDisplay.GetComponentInChildren<ItemForSale>().PopulateFields(selectedInventorySlot.GetComponent<InventorySlotController>().itemID);
            //currentInteractingStoreDisplay.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = itemForSaleSprite;

            itemForSaleSprite = selectedInventorySlot.transform.GetChild(0).GetComponent<Image>().sprite;
            itemSlotObject.transform.GetChild(0).GetComponent<Image>().overrideSprite = selectedInventorySlot.transform.GetChild(0).GetComponent<Image>().sprite;

            itemForSaleObj = currentInteractingStoreDisplay.transform.GetChild(currentInteractingStoreDisplay.transform.childCount - 1).gameObject;
            itemForSale = itemForSaleObj.GetComponent<ItemForSale>();

            confirmButton.GetComponent<Image>().color = confirmButtonOriginalColor;
            removeItemButton.GetComponent<Image>().color = removeItemButtonOriginalColor;

            undoChangeButton.GetComponent<Image>().color = undoChangeButtonOriginalColor;
            resetChangesButton.GetComponent<Image>().color = resetChangesButtonOriginalColor;
        }
       
        if(selectedInventorySlot != null)
        {
         
        }
        confirmEnabled = true;
    }

    void CallConfirm()
    {
        //StartCoroutine(ConfirmChanges());
        //Debug.Log("Call Confirm");
        ConfirmChanges();
    }

    // Confirm Changes
    // Set item in slot and close display and inventory windows
    //IEnumerator ConfirmChanges()
    private void ConfirmChanges()
    {
        //Debug.Log("Confirm Changes");
        if (confirmEnabled)
        {
            //Debug.Log("ConfirmChanges()");
            if(selectedInventorySlot != null)
            {
                // Is there an existing itemForSale?
                //if (itemForSale != null)
                if (!itemForSale.IsEmpty())
                {
                    // Is the original item id different from the selected item's id? Swap if yes
                    // Else do nothing; 
                    if (itemForSale.itemID != selectedInventorySlot.GetComponent<InventorySlotController>().itemID)
                    {
                        SwapItem();
                    }
                    else
                    {

                    }
                }

                // Populate the empty fields
                else
                {
                    // Populate the empty fields of StoreDisplay->ItemForSale using selected inventory item's ID
                    currentInteractingStoreDisplay.FillDisplay(itemForSale);
                    currentInteractingStoreDisplay.GetComponentInChildren<ItemForSale>().PopulateFields(selectedInventorySlot.GetComponent<InventorySlotController>().itemID);
                    currentInteractingStoreDisplay.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = itemForSaleSprite;

                    itemForSaleObj = currentInteractingStoreDisplay.transform.GetChild(currentInteractingStoreDisplay.transform.childCount - 1).gameObject;
                    playerInventory.GetComponent<InventoryController>().DecrementItem(selectedInventorySlot.GetComponent<InventorySlotController>().itemID);
                    removeItemButton.GetComponent<Image>().color = removeItemButtonOriginalColor;
                }


                //itemForSaleList.Add(itemForSaleObj.GetComponent<ItemForSale>());
                itemForSaleList.Add(itemForSale);
            }
            else
            {
                if(itemForSale.isEmpty)
                {
                    itemForSale.PopulateFields(-1);
                    itemForSale.gameObject.GetComponent<SpriteRenderer>().sprite = null;
                    //Debug.Log("Null");
                }

            }
            //close windows via Events? via function?
            //confirmEnabled = false;
            InventoryGridController.OnClicked -= SetItemForSale;
            //InventoryGridController.OnUnclicked -= UndoChange;
            inventoryGrid.GetComponent<InventoryGridController>().UnassignStoreDisplay();
            choiceMade = true;
            ClearFields();
            confirmEnabled = false;
            CloseMenu();
        }
        else
        {
            //Debug.Log("Confirm not enabled");
        }

        if(OnConfirm != null)
        {
            OnConfirm();
        }
        Debug.Log("End Confirm Changes");
        //yield return null;

    }

    void AddItem()
    {
        //Debug.Log("AddItem()");
        //StartCoroutine(PlayerController.instance.OpenInventory());


    }

    // Remove item from display and return to inventory
    void RemoveItem()
    {
        if (!itemForSale.IsEmpty())
        {
            //Debug.Log("Remove: non empty slot ");
            //Debug.Log(itemForSale.itemID);

            removeItemButton.GetComponent<Image>().color = GRAY;
            playerInventory.GetComponent<InventoryController>().IncrementItem(currentInteractingStoreDisplay.GetComponentInChildren<ItemForSale>().itemID);
            itemForSaleSprite = null;

            itemSlotObject.transform.GetChild(0).GetComponent<Image>().overrideSprite = null;
            //inventoryGrid.GetComponent<InventoryGridController>().DeselectItem();
            selectedInventorySlot = null;
            currentInteractingStoreDisplay.GetComponentInChildren<ItemForSale>().RemoveItem();
        }
        else
        {
            Debug.Log("Removing unset item; clearing slot");
            if (selectedInventorySlot != null)
            {
                itemSlotObject.transform.GetChild(0).GetComponent<Image>().overrideSprite = null;
                //inventoryGrid.GetComponent<InventoryGridController>().DeselectItem();
                selectedInventorySlot = null;
            }

        }

    }

    void LoadDisplay(StoreDisplay currentDisplay)
    {
        if(!currentDisplay.GetComponentInChildren<ItemForSale>().IsEmpty())
        {
            //Debug.Log("Load Display");
            itemForSaleObj = currentInteractingStoreDisplay.transform.GetChild(currentInteractingStoreDisplay.transform.childCount - 1).gameObject;
            itemForSale = currentDisplay.GetComponentInChildren<ItemForSale>();
            itemForSaleSprite = itemForSale.itemSprite;
            itemSlotObject.transform.GetChild(0).GetComponent<Image>().overrideSprite = currentInteractingStoreDisplay.GetSprite();
        }
        
        //currentDisplay.GetComponentInChildren<ItemForSale>()
        //itemForSaleSprite = selectedInventorySlot.transform.GetChild(0).GetComponent<Image>().sprite;
        //itemSlotObject.transform.GetChild(0).GetComponent<Image>().overrideSprite = selectedInventorySlot.transform.GetChild(0).GetComponent<Image>().sprite;
    }

    // Swap itemForSale with selectedInventorySlot
    void SwapItem()
    {
        Debug.Log("SwapItem");
        playerInventory.GetComponent<InventoryController>().IncrementItem(itemForSale.itemID);
        playerInventory.GetComponent<InventoryController>().DecrementItem(selectedInventorySlot.GetComponent<InventorySlotController>().itemID);

        itemForSale.PopulateFields(selectedInventorySlot.GetComponent<InventorySlotController>().itemID);
        itemForSale.gameObject.GetComponent<SpriteRenderer>().sprite = itemForSale.itemSprite;
    }

    void UndoChange()
    {
        Debug.Log("UndoChange()");
    }

    void ResetChanges()
    {
        Debug.Log("ResetChanges()");
    }

    void CancelMenu()
    {
        ResetChanges();
        InventoryController.instance.CloseInventory();
        inventoryWindow.SetActive(false);
        Debug.Log(this + "Cancel");
        storeDisplayDialog.SetActive(false);
        InventoryGridController.OnClicked -= SetItemForSale;
        playerInstance.ReleasePlayer();
    }

    void CloseMenu()
    {
        Debug.Log("CloseMenu");
        InventoryController.instance.CloseInventory();
        inventoryWindow.SetActive(false);
        storeDisplayDialog.SetActive(false);
        playerInstance.ReleasePlayer();
    }

    void OnDisable()
    {
        Debug.Log("OnDisable");
        InventoryGridController.OnClicked -= SetItemForSale;
        //InventoryGridController.OnUnclicked -= UndoChange;
    }

    void UpdateDisplayList()
    {
        for(int i = 0; i < storeDisplayList.gameObject.transform.GetChildCount(); ++i)
        {
            StoreDisplay sd = storeDisplayList.gameObject.transform.GetChild(i).GetComponent<StoreDisplay>();
            if(!sd.isEmpty)
            {
                ItemForSale ifs = sd.gameObject.transform.GetChild(0).GetComponent<ItemForSale>();
                if (!itemForSaleList.Contains(ifs))
                {
                    itemForSaleList.Add(sd.gameObject.transform.GetChild(0).GetComponent<ItemForSale>());
                }
                
            }
        }
    }

    void ClearFields()
    {
        Debug.Log("ClearFields");
        itemForSaleSprite = null;
        selectedInventorySlot = null;
        currentInteractingStoreDisplay = null;
        itemForSaleObj = null;
        itemForSale = null;
    }

    /*
    public void InteractWithStoreDisplay(int id)
    {
        storeDisplay.transform
        storeDisplay.SetActive(true);
    }
    */
}
