using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;

// To be used by Storefront->PriceInputCanvas->PriceInputPanel-> *ItemSaleController*

public class ItemSaleController : MonoBehaviour
{
    public static ItemSaleController instance { get; private set; }

    //public delegate void ConfirmSaleAction(Receipt r);
    //public static event ConfirmSaleAction OnConfirmSale;

    public delegate void PriceResetAction();
    public static event PriceResetAction OnPriceReset;

    public event Action SaleMade;

    [SerializeField]
    Button confirmButton;
    [SerializeField]
    Button resetButton;

    public GameObject saleWindow;

    public GameObject digitFieldObj;
    public DigitField digitField;

    public GameObject percentObj;
    Percent percent;

    public GameObject currentItemForSaleObj;
    public ItemForSale currentItemForSale;
    public GameObject itemNameObj;
    public GameObject itemPriceObj;
    public GameObject itemImageObj;

    public GameObject patronResponse;

    public Patron currentPatron;
    public int patronChoice;

    List<Receipt> receiptList = new List<Receipt>();

    public bool offerSubmitted;
    [SerializeField]
    int itemOffer;
    [SerializeField]
    int sessionRevenue;
    [SerializeField]
    int dayRevenue;
    [SerializeField]
    int totalRevenue;

    void Start()
    {
        Debug.Log("ItemSaleController.Start()");
        if(digitFieldObj == null)
        {
            Debug.Log("digitFieldObj is NULL");
        }
        if (digitField == null)
        {
            Debug.Log("digitField is NULL");
        }
    }

    void AssignItemForSale()
    {
        Debug.Log("AssignItemForSale()");
        Debug.Log("CURRENT PATRON'S DESIRED ITEM: " + currentPatron.desiredItem.GetItemName());
        currentItemForSale = currentPatron.desiredItem;
        currentItemForSaleObj = currentItemForSale.gameObject;
        //currentItemForSale.PopulateFields(currentPatron.desiredItemID);
        currentItemForSale.PopulateFields(currentPatron.desiredItem.GetItemID());
        if(itemImageObj == null)
        {
            Debug.Log("itemImageObj is null");
        }
        if (itemNameObj == null)
        {
            Debug.Log("itemNameObj is null");
        }
        if (itemPriceObj == null)
        {
            Debug.Log("itemPriceObj is null");
        }
        itemImageObj.GetComponent<Image>().sprite = currentItemForSale.itemSprite;
        itemNameObj.GetComponent<TMP_Text>().text = currentItemForSale.GetItemName();
        itemPriceObj.GetComponent<TMP_Text>().text = "Base Price: $" + currentItemForSale.GetBasePrice();
        Debug.Log("CURRENT ITEM FOR SALE: " + currentItemForSale.GetItemName());
        digitField.SetUpDigitField(currentItemForSaleObj, currentItemForSale);
        //digitField.SetPrice(currentItemForSale.basePrice);

    }

    void Awake()
    {
        Debug.Log("ItemSaleController.Awake()");

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        offerSubmitted = false;
        //currentItemForSale = currentItemForSaleObj.GetComponent<ItemForSale>();
        if (digitField == null)
        {
            digitField = digitFieldObj.GetComponent<DigitField>();
        }
        if(percentObj != null)
        {
            percent = percentObj.GetComponent<Percent>();
        }
        if(saleWindow == null)
        {
            saleWindow = this.gameObject.transform.GetChild(0).gameObject;
        }
        // Must be Set
        //currentItemForSaleObj
        //itemNameObj
        //itemPriceObj
        //itemImageObj

        //confirmButton.onClick.AddListener(SubmitSale);
        confirmButton.onClick.AddListener(async () => await SubmitSaleAsync());
        resetButton.onClick.AddListener(ResetPrice);

        //DigitField.OnPriceChange += UpdatePercent;
    }

    public void ActivateItemSaleController()
    {
        this.gameObject.SetActive(true);
    }

    public void DeactivateItemSaleController()
    {
        this.gameObject.SetActive(false);
    }


    void UpdatePercent(int current, int basePrice)
    {
        //Debug.Log("Update Percent");
        percent.SetPercent(current, basePrice);
    }

    public void RequestSale(Patron p, ItemForSale ifs)
    {

    }

    public void SetUpSalePage(Patron p)
    {
        Debug.Log("SetupSalePage");
        offerSubmitted = false;
        currentPatron = p;
        if (!this.gameObject.activeInHierarchy)
        {
            this.gameObject.SetActive(true);
        }
        if (!saleWindow.gameObject.activeInHierarchy)
        {
            saleWindow.gameObject.SetActive(true);
        }
        if (currentPatron.desiredItem != null)
        {
            AssignItemForSale();
        }
        Debug.Log("END SetupSalePage()");
        // display sale window
    }


    async Task SubmitSaleAsync()
    //void SubmitSale()
    {
        Debug.Log("SubmitSaleAsync");
        var task = ConfirmSaleAsync();
        await task;
        //ConfirmSale();
        Debug.Log("End SubmitSaleAsync");
    }

    // Submit the price to customer and check if they accept or reject the price
    //async Task ConfirmSaleAsync()
    async Task<Receipt> ConfirmSaleAsync()
    {
        Debug.Log("ConfirmSale");
        offerSubmitted = true;
        itemOffer = digitField.GetCurrentPrice();
        patronChoice = currentPatron.ProcessOffer(itemOffer);

        Receipt receipt = new Receipt();
        if (receiptList == null)
        {
            Debug.Log("OH YAYER");
        }
        if (patronChoice == 0)
        {
            Debug.Log("Patron choice: " + patronChoice + " (no)");
        }
        else if(patronChoice == 1)
        {
            Debug.Log("Patron choice: " + patronChoice + " (yes)");
            Debug.Log("Item Sold (SELLING OF ITEM NEEDS TO BE IMPLEMENTED)");
            sessionRevenue += itemOffer;
            receipt.SetRevenue(itemOffer);
            receipt.SetNumItemsSold(receipt.GetNumItemsSold() + 1);
            currentItemForSale.SellItem();
            //currentItemForSale.RemoveItemFromDisplay();

        }
        var handleResponseTask = patronResponse.GetComponent<PatronResponse>().HandleResponseAsync(patronChoice);

        await handleResponseTask;

        receiptList.Add(receipt);
        SaleMade.Invoke();
        Debug.Log("END ConfirmSale");
        return receipt;
        //return sessionRevenue;
    }

    public void CloseSaleWindow()
    {
        if(saleWindow.gameObject.activeInHierarchy)
        {
            saleWindow.gameObject.SetActive(false);
        }
    }

    public int GetItemOffer()
    {
        return itemOffer;
    }

    void GrabItemDetails()
    {

    }

    public Receipt GetLatestReceipt()
    {
        return receiptList[receiptList.Count - 1];
    }

    int GetPatronChoice()
    {
        return patronChoice;
    }



    void ResetPrice()
    {
        Debug.Log("Reset Price");
        digitField.ResetDigitField();
        percent.ResetPercent(digitField.GetBasePrice());
    }

    void SetImage()
    {
        //currentItemForSale.
    }

    void OnEnable()
    {
        DigitField.OnPriceChange += UpdatePercent;
    }

    void OnDisable()
    {
        Debug.Log("DISABLING ITEMSALECONTROLLER");
        DigitField.OnPriceChange -= UpdatePercent;
    }
}
