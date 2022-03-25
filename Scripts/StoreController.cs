using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class StoreController : MonoBehaviour
{
    public static StoreController instance { get; private set; }

    public CashRegister cashRegister;

    public PlayerController player;

    public static bool storeOpen;
    public bool transactionInProgress;
    public static int numPatrons;

    public GameObject storeDialog;

    public GameObject patronQueueObj;
    public GameObject currentPatronObj;

    public PatronGenerator patronGenerator;

    public GameObject itemSaleControllerObj;
    public ItemSaleController itemSaleController;

    public GameObject priceInputPanel;

    PatronQueue patronQueue;
    Patron currentPatron;

    public GameObject storeDisplayController;
    public StoreDisplayCollection storeDisplayCollection;
    //public List<GameObject> storeDisplayList;
    public List<ItemForSale> itemForSaleList;

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
        instance = this;
        storeOpen = false;
        //storeDialog.SetActive(false);
        patronQueue = patronQueueObj.GetComponent<PatronQueue>();
        if(itemSaleController == null && itemSaleControllerObj != null)
        {
            itemSaleController = itemSaleControllerObj.GetComponent<ItemSaleController>();
        }
        if(!storeOpen)
        {
            //priceInputPanel.SetActive(false);
            itemSaleControllerObj.SetActive(false);
        }
    }

    public void ArrangeStore()
    {
        storeDisplayCollection.CompileCollection();
        storeDisplayCollection.PrintCollection();

    }

    void CloseStore()
    {
        Debug.Log("Closing Store");
        PlayerController.isFrozen = false;
        storeOpen = false;
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

        itemSaleControllerObj.SetActive(false);
        currentPatron.offerReceived = true;
        yield return null;
        Debug.Log("End DisplayOfferPage");
    }
        

    ///*
    async Task DisplayOfferPageAsync(Patron p, CancellationToken cancellationToken)
    {
        bool offerMade = false;
        Debug.Log("Displaying offer page ASYNC");

        itemSaleController.SetUpSalePage(p);

        //while (!offerMade)
        while (!itemSaleController.offerSubmitted)
        {
            Debug.Log("offer not yet submitted ASYNC");
            await Task.Delay(1, cancellationToken);
        }

        itemSaleControllerObj.SetActive(false);
        currentPatron.offerReceived = true;
        
        Debug.Log("End DisplayOfferPage");
    }
    //*/

    bool GetNextPatron()
    {
        Debug.Log("GetNextPatron");
        // Assign next patron in queue
        if (patronQueue.GetComponent<PatronQueue>().patronCount > 0)
        {
            currentPatronObj = patronQueue.GetComponent<PatronQueue>().NextPatron();
            //Debug.Log("1");

            currentPatron = currentPatronObj.GetComponent<Patron>();
            //Debug.Log("2");
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnDisable()
    {
        //Patron.OnRequestOffer -= DisplayOfferPage;
        Patron.OnRequestOffer -= DisplayOfferPageAsync;
        cancellationTokenSource.Cancel();
    }
    
    void OnEnable()
    {
        //Patron.OnRequestOffer += DisplayOfferPage;
        Patron.OnRequestOffer += DisplayOfferPageAsync;
    }

    
    public async Task OpenStore()
    //public IEnumerator OpenStore()
    //public void OpenStore()
    {
        var cancellationToken = cancellationTokenSource.Token;

        Debug.Log("OpenStore()");
        // Subscribe to event
        

        storeDisplayCollection.CompileCollection();
        Debug.Log("Count: " + storeDisplayCollection.storeCollection.Count);
        // DO NOT let the player open the store if they have 0 items for sale
        if (storeDisplayCollection.storeCollection.Count > 1)
        {
            //player.StopPlayer();
            Debug.Log("Opening Store..");
            storeDisplayCollection.CompileCollection();
            //storeDisplayList
            itemForSaleList = storeDisplayController.GetComponent<StoreDisplayController>().itemForSaleList;
            numPatrons = 1;
            //numPatrons = Random.Range(1, 6);
            //StartCoroutine(patronGenerator.GeneratePatrons(numPatrons));
            patronGenerator.GeneratePatrons(numPatrons);
            if (storeDisplayCollection != null)
            {
                storeDisplayCollection.CompileCollection();
                //storeDisplayCollection.PrintCollection();
            }
            ///*
            Debug.Log("numPatrons" + numPatrons);
            storeOpen = true;
            //Debug.Log("Start couroutine");

            //yield return StartCoroutine(ProcessPatrons());
            var processPatronTask = ProcessPatronsAsync(cancellationToken);
            await processPatronTask;
            //StartCoroutine(ProcessPatrons());
            //ProcessPatrons();

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


        CloseStore();
        Debug.Log("EXIT");
        //yield return null;
        //player.ReleasePlayer();
    }


    private async Task ProcessPatronsAsync(CancellationToken cancellationToken)
    //IEnumerator ProcessPatrons()
    //void ProcessPatrons()
    {
        Debug.Log("ProcessPatrons");
        //Debug.Log("numPatrons: " + numPatrons);

        while (numPatrons > 0)
        {
            //Debug.Log("while");
            if (GetNextPatron())
            {
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
            
            currentPatron.PrintPatron();
            var processNextPatronTask = ProcessNextPatronAsync(cancellationToken);
            try
            {
                await processNextPatronTask;
            }
            catch
            {

            }
            /*
            while(transactionInProgress)
            {
                yield return null;
            }
            */
            Debug.Log("StoreController.ProcessPatronsAsync -> Purchase complete");
        }
    }

    async Task ProcessNextPatronAsync(CancellationToken cancellationToken)
    //IEnumerator ProcessNextPatron()
    {
        transactionInProgress = true;
        var attemptPurchaseTask = currentPatron.AttemptPurchaseAsync(cancellationToken);
        await attemptPurchaseTask;
        //yield return StartCoroutine(currentPatron.AttemptPurchase());
        transactionInProgress = false;
    }


}
