using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using System;

using Random = UnityEngine.Random;

public class StoreController : MonoBehaviour
{
    public static StoreController instance { get; private set; }

    public event Action StoreOpened;
    public event Action StoreSessionEnded;
    public event Action StoreClosed;

    public GameObject balanceSheet;

    public CashRegister cashRegister;

    public PlayerController player;

    [SerializeField]
    bool isTest;
    //public static bool storeOpen;
    static bool storeOpen;
    public bool transactionInProgress;
    bool didBalanceSheetClose;
    public static int numPatrons;

    public GameObject storeDialog;

    public GameObject patronQueueObj;
    public GameObject currentPatronObj;

    [SerializeField]
    GameObject generalBackgroundMusic;
    [SerializeField]
    GameObject openStoreMusic;

    public PatronGenerator patronGenerator;

    public GameObject itemSaleControllerObj;
    public ItemSaleController itemSaleController;
    public GameObject saleWindow;


    public GameObject priceInputPanel;

    PatronQueue patronQueue;
    Patron currentPatron;

    public GameObject patronResponse;

    public GameObject storeDisplayController;
    public StoreDisplayCollection storeDisplayCollection;
    //public List<GameObject> storeDisplayList;
    public List<ItemForSale> itemForSaleList;

    public GameObject storeLogObj;
    public StoreLog storeLog;

    int timeBetweenSales;

    private CancellationTokenSource cancellationTokenSource;

    // Update is called once per frame
    void Update()
    {
        
    }

    void Start()
    {

        cancellationTokenSource = new CancellationTokenSource();
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        storeOpen = false;
        //storeDialog.SetActive(false);
        patronQueue = patronQueueObj.GetComponent<PatronQueue>();
        if(itemSaleController == null && itemSaleControllerObj != null)
        {
            itemSaleController = itemSaleControllerObj.GetComponent<ItemSaleController>();
        }
        if(saleWindow == null)
        {
            saleWindow = itemSaleControllerObj.transform.GetChild(0).gameObject;
        }
        if(patronResponse == null)
        {
            Debug.Log("patronResponse == null");
            
        }
        if(storeLog == null)
        {
            Debug.Log("storeLog == null");
            
        }
        if(balanceSheet == null)
        {
            Debug.Log("balanceSheet == null");
        }
        //balanceSheet.GetComponent<BalanceSheetView>().BalanceSheetClosed += (async () => await OnBalanceSheetClosedAsync());
        balanceSheet.GetComponent<BalanceSheetView>().BalanceSheetClosed += OnBalanceSheetClosedListener;
        if (!storeOpen)
        {
            priceInputPanel.SetActive(false);
            //itemSaleControllerObj.SetActive(false);
            itemSaleController.DeactivateItemSaleController();
        }
        //DontDestroyOnLoad(this.gameObject);
    }

    public void ArrangeStore()
    {
        storeDisplayCollection.CompileCollection();
        storeDisplayCollection.PrintCollection();

    }


    void OnDisable()
    {
        //Patron.OnRequestOffer -= DisplayOfferPage;
        Patron.OnRequestOffer -= DisplayOfferPageAsync;
        if (balanceSheet != null)
        {
            //balanceSheet.GetComponent<BalanceSheetView>().BalanceSheetClosed -= (async () => await OnBalanceSheetClosedListener());
            balanceSheet.GetComponent<BalanceSheetView>().BalanceSheetClosed -= OnBalanceSheetClosedListener;
        }

        cancellationTokenSource.Cancel();
    }

    void OnEnable()
    {
        //Patron.OnRequestOffer += DisplayOfferPage;
        Patron.OnRequestOffer += DisplayOfferPageAsync;
    }


    void OnBalanceSheetClosedListener()
    {
        if(!didBalanceSheetClose)
            didBalanceSheetClose = true;

    }

    async Task OnBalanceSheetClosedAsync(CancellationToken cancellationToken)
    {
        Debug.Log("HERE");
        
        while(!didBalanceSheetClose)
        {
            await Task.Delay(1, cancellationToken);
        }
        return;
    }

    //async Task CloseStore()
    void CloseStore()
    {
        Debug.Log("Closing Store");
        // Log sale stats
        // Destroy patrons

        //StoreClosed.Invoke();
        PlayerController.isFrozen = false;
        storeOpen = false;
        Debug.Log("END CLOSE STORE");
    }
    
    async Task ReviewCompletedStoreSessionAsync(CancellationToken cancellationToken)
    {
        StoreSessionEnded.Invoke();
        var balanceSheetClosedTask = OnBalanceSheetClosedAsync(cancellationToken);
        await balanceSheetClosedTask;
    }

    //void DisplayOfferPage(Patron p)
    // async void DisplayOfferPageAsync(Patron p)
    
    IEnumerator DisplayOfferPage(Patron p)
    {
        bool offerMade = false;
        Debug.Log("Displaying offer page");

        itemSaleController.SetUpSalePage(p);
        /*
        if(!itemSaleControllerObj.activeInHierarchy)
        {
            itemSaleControllerObj.SetActive(true);
        }
        */

        //while (!offerMade)
        while (!itemSaleController.offerSubmitted)
        {
            Debug.Log("offer not yet submitted");
            yield return null;
        }

        //itemSaleControllerObj.SetActive(false);


        currentPatron.offerReceived = true;
        yield return null;
        Debug.Log("End DisplayOfferPage");
    }
        

    ///*
    async Task<int> DisplayOfferPageAsync(Patron p, CancellationToken cancellationToken)
    {
        bool offerMade = false;
        priceInputPanel.SetActive(true);
        Debug.Log("Displaying offer page ASYNC");
        if (!itemSaleControllerObj.activeInHierarchy)
        {
            itemSaleController.ActivateItemSaleController();
        }
        itemSaleController.SetUpSalePage(p);

        //while (!offerMade)
        while (!itemSaleController.offerSubmitted)
        {
            await Task.Delay(1, cancellationToken);
        }
        itemSaleController.CloseSaleWindow();
        currentPatron.offerReceived = true;
        
        Debug.Log("End DisplayOfferPage");
        Debug.Log("Item Offer: " + itemSaleController.GetItemOffer());
        return itemSaleController.GetItemOffer();
    }
    //*/

    bool GetNextPatron()
    {
        Debug.Log("GetNextPatron");
        // Assign next patron in queue
        if (patronQueue.GetComponent<PatronQueue>().patronCount > 0)
        {
            currentPatronObj = patronQueue.GetComponent<PatronQueue>().NextPatron();

            currentPatron = currentPatronObj.GetComponent<Patron>();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsStoreOpen()
    {
        return storeOpen;
    }


    void SwitchMusic()
    {
        if(generalBackgroundMusic.activeInHierarchy)
        {
            generalBackgroundMusic.SetActive(false);
            openStoreMusic.SetActive(true);
        }
        else
        {
            openStoreMusic.SetActive(false);
            generalBackgroundMusic.SetActive(true);
        }

    }
    
    public async Task OpenStoreAsync()
    {
        var cancellationToken = cancellationTokenSource.Token;

        Debug.Log("OpenStore() async");
        didBalanceSheetClose = false;
        Debug.Log("DABU");
        StoreOpened.Invoke();
        // Subscribe to event
        Debug.Log("YOSH");

        storeDisplayCollection.CompileCollection();
        Debug.Log("Count: " + storeDisplayCollection.storeCollection.Count);
        // DO NOT let the player open the store if they have 0 items for sale
        if (storeDisplayCollection.storeCollection.Count > 1)
        {
            //player.StopPlayer();
            Debug.Log("Opening Store..");
            //SwitchMusic();

            //storeDisplayCollection.CompileCollection();
            //storeDisplayList
            itemForSaleList = storeDisplayController.GetComponent<StoreDisplayController>().itemForSaleList;
            if(isTest)
            {
                numPatrons = 2;
            }
            else
            {
                numPatrons = Random.Range(1, 6);
                
            }

            patronGenerator.GeneratePatrons(numPatrons);
            //StartCoroutine(patronGenerator.GeneratePatrons(numPatrons));
            if (storeDisplayCollection != null)
            {
                storeDisplayCollection.CompileCollection();
                //storeDisplayCollection.PrintCollection();
            }
            ///*
            Debug.Log("numPatrons " + numPatrons);
            storeOpen = true;
            var processPatronTask = ProcessPatronsAsync(cancellationToken);
            await processPatronTask;

            /*
            while(numPatrons > 0)
            {

            }
            */
            //StartCoroutine(ProcessPatrons());
            //Debug.Log("End couroutine");

            //storeDialog.SetActive(false);

            //yield return null;
            //*/
        }
        else
        {
            Debug.Log("Attempted to open store with 0 items for sale");
            // Nothing for sale
        }

        var ReviewCompletedStoreSessionTask = ReviewCompletedStoreSessionAsync(cancellationToken);
        await ReviewCompletedStoreSessionTask;


        CloseStore();
        
        Debug.Log("END OpenStoreAsync");
        //SwitchMusic();
        //yield return null;
        //player.ReleasePlayer();
    }


    private async Task ProcessPatronsAsync(CancellationToken cancellationToken)
    //IEnumerator ProcessPatrons()
    //void ProcessPatrons()
    {
        Debug.Log("ProcessPatrons");
        //Debug.Log("numPatrons: " + numPatrons);
        int count = 1;
        while (numPatrons > 0)
        {
            //Debug.Log("while");
            if (GetNextPatron())
            {
                Debug.Log("Retreiving next patron...");
                --numPatrons;

            }
            else
            {

            }
            // AttemptPurchase -> RequestOffer -> StartCoroutine(DisplayOfferPage)
            //yield return StartCoroutine(currentPatron.AttemptPurchase());
            //StartCoroutine(currentPatron.AttemptPurchase());

            //StartCoroutine(ProcessNextPatron());
            //yield return StartCoroutine(ProcessNextPatron());
            Debug.Log("### Patron " + count);
            currentPatron.PrintPatron();
            if(currentPatron.desiredItemID != -1)
            {
                
                var processNextPatronTask = ProcessNextPatronAsync(cancellationToken);
                //var patronResponseTask = patronResponse.GetComponent<PatronResponse>().EndTransactionAsync();
                try
                {
                    
                    await processNextPatronTask;
                    
                    //await ProcessNextPatronAsync(cancellationToken);
                    //await patronResponseTask;

                    //var completedTransaction = await Task.WhenAll(processNextPatronTask, patronResponseTask);
                    //await completedTransaction;
                    //patronResponseTask.Wait();
                    /*
                    while(!patronResponse.GetComponent<PatronResponse>().transactionEnded)
                    {
                        await Task.Delay(1);
                    }
                    */
                    Debug.Log("StoreController.ProcessPatronsAsync -> Purchase complete");

                    // Destroy current patron? or set inactive
                    Destroy(currentPatron.gameObject);
                }
                catch
                {

                }
            }
            else
            {
                Debug.Log("desired item ID = -1; patron wants nothing");
            }
            /*
            while(transactionInProgress)
            {
                yield return null;
            }
            */
            count++;
        }

        Debug.Log("No patrons remaining");
        Debug.Log("END ProcessPatronsAsync");
    }

    async Task ProcessNextPatronAsync(CancellationToken cancellationToken)
    //IEnumerator ProcessNextPatron()
    {
        Debug.Log("START ProcessNextPatronAsync");
        //Debug.Log("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
        Debug.Log("Patron ID: " + currentPatron.GetID());
        transactionInProgress = true;
        //Debug.Log("starting attemptPurchaseTask");
        var attemptPurchaseTask = currentPatron.AttemptPurchaseAsync(cancellationToken);
        //Debug.Log("waiting attemptPurchaseTask");
        await attemptPurchaseTask;
        //Debug.Log("finished attemptPurchaseTask");
        
        ///*
        while (!patronResponse.GetComponent<PatronResponse>().confirmClicked)
        {
            //Debug.Log("WAITING");
            await Task.Delay(1, cancellationToken);
            
        }
        //*/
        transactionInProgress = false;

        /*
        while (transactionInProgress)
        {
            await Task.Delay(1);
        }
        */
        Debug.Log("End ProcessNextPatronAsync");
        //Debug.Log("#################################################");
    }


}
