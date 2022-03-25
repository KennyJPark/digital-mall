using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemSaleController : MonoBehaviour
{
    public static ItemSaleController instance { get; private set; }

    public delegate void ConfirmSaleAction();
    public static event ConfirmSaleAction OnConfirmSale;

    [SerializeField]
    Button confirmButton;
    [SerializeField]
    Button resetButton;

    public GameObject digitFieldObj;
    public DigitField digitField;

    public GameObject percentObj;
    Percent percent;

    public GameObject currentItemForSaleObj;
    public ItemForSale currentItemForSale;
    public GameObject itemImage;
    

    public Patron currentPatron;

    public bool offerSubmitted;
    

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
        currentItemForSale = currentPatron.desiredItem;
        currentItemForSaleObj = currentItemForSale.gameObject;
        currentItemForSale.PopulateFields(currentPatron.desiredItemID);
        itemImage.GetComponent<Image>().sprite = currentItemForSale.itemSprite;

        digitField.SetUpDigitField(currentItemForSaleObj, currentItemForSale);
        //digitField.SetPrice(currentItemForSale.basePrice);

    }

    void Awake()
    {
        Debug.Log("ItemSaleController.Awake()");
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

        confirmButton.onClick.AddListener(ConfirmSale);
        resetButton.onClick.AddListener(ResetPrice);

        DigitField.OnPriceChange += UpdatePercent;
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
        Debug.Log("SetupSalePage()");
        offerSubmitted = false;
        currentPatron = p;
        if (!this.gameObject.activeInHierarchy)
        {
            this.gameObject.SetActive(true);
        }

        if (currentPatron.desiredItem != null)
        {
            
            AssignItemForSale();
        }
        Debug.Log("END SetupSalePage()");
        // display sale window
    }


    // Submit the price to customer and check if they accept or reject the price
    void ConfirmSale()
    {
        Debug.Log("Confirm Sale");
        offerSubmitted = true;
        if (OnConfirmSale != null)
            OnConfirmSale();
    }


    void GrabItemDetails()
    {

    }

    void ResetPrice()
    {
        Debug.Log("Reset Price");
    }

    void SetImage()
    {
        //currentItemForSale.
    }

    void OnDisable()
    {
        DigitField.OnPriceChange -= UpdatePercent;
    }
}
