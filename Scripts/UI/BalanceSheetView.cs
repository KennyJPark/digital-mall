using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TMPro;
using System;

public class BalanceSheetView : MonoBehaviour
{
    public event Action BalanceSheetClosed;

    [SerializeField]
    private BalanceSheet balanceSheet;

    [SerializeField]
    GameObject storeFront;

    public GameObject balanceSheetWindow;
    public GameObject balanceSheetBar;

    public TextMeshProUGUI customersVisitedText;
    public TextMeshProUGUI customersServedText;
    public TextMeshProUGUI salesMadeText;
    public TextMeshProUGUI revenueText;

    //public TextMeshProUGUI patronsVisitedText;
    //public TMP_InputField digitField;

    void Awake()
    {
        SubscribeEvents();
        if (gameObject.activeInHierarchy)
        {
            //gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetUpBalanceSheet();
    }

    void OnBalanceSheetChanged()
    {
        Debug.Log("OnBalanceSheetChanged");
        UpdateView();
    }

    void SetUpBalanceSheet()
    {
        Debug.Log("SETTING UP BALANCE SHEET");
        gameObject.SetActive(true);

        if (balanceSheet == null)
        {
            balanceSheet = gameObject.GetComponent<BalanceSheet>();
        }
        if (balanceSheetWindow == null)
        {
            balanceSheetWindow = gameObject.transform.GetChild(0).gameObject;
            
        }
        if (balanceSheetBar == null)
        {
            balanceSheetBar = gameObject.transform.GetChild(1).gameObject;
            
        }

        balanceSheetWindow.SetActive(false);
        balanceSheetBar.SetActive(false);


    }

    void ShowBalanceSheet()
    {
        Debug.Log("SHOW BALANCE SHEET");
        balanceSheetWindow.SetActive(true);
    }

    public void CloseBalanceSheet()
    {
        Debug.Log("CLOSE BALANCE SHEET");
        BalanceSheetClosed.Invoke();
        //balanceSheetWindow.SetActive(false);
    }

    void OnDisable()
    {
        UnsubscribeEvents();
    }

    public void Reset()
    {

    }

    public void UpdateView()
    {
        Assert.AreNotEqual(null, customersVisitedText);
        Assert.AreNotEqual(null, customersServedText);
        Assert.AreNotEqual(null, salesMadeText);
        Assert.AreNotEqual(null, revenueText);

        customersVisitedText.text = balanceSheet.GetNumPatronsVisited().ToString();
        customersServedText.text = balanceSheet.GetPatronsServed().ToString();
        salesMadeText.text = balanceSheet.GetNumItemsSold().ToString();
        revenueText.text = balanceSheet.GetRevenue().ToString();
    }

    void SubscribeEvents()
    {
        Debug.Log("BalanceSheetView.SubscribeEvents()");
        if (storeFront != null)
        {
            storeFront.GetComponent<StoreController>().StoreOpened += SetUpBalanceSheet;
            storeFront.GetComponent<StoreController>().StoreSessionEnded += ShowBalanceSheet;
        }

        if (balanceSheet != null)
        {
            balanceSheet.BalanceSheetChanged += OnBalanceSheetChanged;
        }
        else
        {
            balanceSheet = gameObject.GetComponent<BalanceSheet>();
            balanceSheet.BalanceSheetChanged += OnBalanceSheetChanged;
        }
    }

    void UnsubscribeEvents()
    {
        if (storeFront != null)
        {
            storeFront.GetComponent<StoreController>().StoreOpened -= SetUpBalanceSheet;
            storeFront.GetComponent<StoreController>().StoreSessionEnded -= ShowBalanceSheet;
        }
        if (balanceSheet != null)
        {
            balanceSheet.BalanceSheetChanged -= OnBalanceSheetChanged;
        }
        
    }

    void OnDestroy()
    {

    }

}
