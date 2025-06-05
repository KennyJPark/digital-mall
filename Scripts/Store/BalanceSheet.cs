using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BalanceSheet : MonoBehaviour
{
    [SerializeField]
    GameObject itemSaleController;

    [SerializeField]
    private int _numPatronsVisited;
    [SerializeField]
    private int _patronsServed;
    [SerializeField]
    private int _numItemsSold;
    [SerializeField]
    private int _revenue;
    [SerializeField]
    private int _profit;



    public event Action BalanceSheetChanged;

    //public delegate void CompletedSaleAction();
    //public static event CompletedSaleAction OnCompletedSale;




    /* Example on how to set up delegates:
     * 
     *                      *** DigitField.cs ***
     *     public delegate void PriceChangeAction(int currentVal, int baseVal);
           public static event PriceChangeAction OnPriceChange;
     * 
     *         
     *      if (OnPriceChange != null)
                OnPriceChange(currentPrice, basePrice);


        Example on how to set up listener:

                            *** ItemSaleController.cs ***
                            *
     *  DigitField.OnPriceChange += UpdatePercent;
     *  
     *  DigitField.OnPriceChange -= UpdatePercent;
     * */

    void Awake()
    {


        _numPatronsVisited = 0;
        _patronsServed = 0;
        _numItemsSold = 0;
        _revenue = 0;
        _profit = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (itemSaleController != null)
        {
            itemSaleController.GetComponent<ItemSaleController>().SaleMade += OnSaleMade;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    //public void RecordSale(bool patronMadePurchase, int itemsSold, int rev)
    public void RecordSale(Receipt receipt)
    {
        //Debug.Log("HERE");
        _revenue += receipt.GetRevenue();
        _numItemsSold += receipt.GetNumItemsSold();
        if (receipt.GetNumItemsSold() > 0)
        {
            _patronsServed++;
        }
        _numPatronsVisited++;
        BalanceSheetChanged.Invoke();
    }

    void OnSaleMade()
    {
        Debug.Log("On Sale Made");
        Receipt r = itemSaleController.GetComponent<ItemSaleController>().GetLatestReceipt();
        r.PrintReceipt();
        RecordSale(r);
    }

    public void GenerateBalanceSheet()
    {

    }

    public int GetNumPatronsVisited()
    {
        return _numPatronsVisited;
    }

    public int GetPatronsServed()
    {
        return _patronsServed;
    }

    public int GetNumItemsSold()
    {
        return _numItemsSold;
    }

    public int GetRevenue()
    {
        return _revenue;
    }

    public int GetProfit()
    {
        return _profit;
    }

    void UnsubscribeEvents()
    {
        if (itemSaleController != null)
        {
            itemSaleController.GetComponent<ItemSaleController>().SaleMade -= OnSaleMade;
        }

        
    }

    void OnEnable()
    {
        //ItemSaleController.OnConfirmSale += RecordSale;
    }

    void OnDisable()
    {
        //ItemSaleController.OnConfirmSale -= RecordSale;
        UnsubscribeEvents();
    }

    void OnDestroy()
    {

    }
}
