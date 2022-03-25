using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public enum PatronType
{
    Man,
    Woman,
    Child,
}

public class Patron : MonoBehaviour
{
    public Patron instance { get; private set; }

    // delegate for when patron requests offer
    public delegate Task OfferAction(Patron p, CancellationToken cancellationToken);
    //public delegate IEnumerator OfferAction(Patron p);
    public static event OfferAction OnRequestOffer;

    [SerializeField]
    int id;
    public static int count;

    [SerializeField]
    string name;
    public PatronType patronType;

    public long budget;
    public int desiredItemIndex;
    public int desiredItemID;
    public ItemForSale desiredItem;

    public bool offerReceived;
    public bool includeEmptyItem;

    public string dialogue;

    float satisfactionPoints;

    [SerializeField]
    PreferencesCollection preferenceCollection;

    //public StoreDisplayCollection storeDisplayCollection;
    public GameObject storeDisplayCollection;

    /*
    [SerializeField]
    PreferencesDatabaseObject preferenceDatabase;
    */

    int preferenceID;
    public NPCPreference patronPreference;

    private CancellationTokenSource cancellationTokenSource;

    public async Task AttemptPurchaseAsync(CancellationToken cancellationToken)
    //public IEnumerator AttemptPurchase()
    {
        Debug.Log("AttemptPurchase");

        // item already sold to earlier patron
        if (desiredItem == null)
        {

        }
        else
        {
            //Debug.Log("I want " + desiredItem.itemName);

            // Open sale display
            //yield return StartCoroutine(RequestOffer());
            var RequestOfferTask = RequestOfferAsync(cancellationToken);
            await RequestOfferTask;
            //StartCoroutine(RequestOffer());
            //RequestOffer();

        }

        /*
        Debug.Log(gameObject.name);
        dialogue = "I want to buy this!";
        ScrollingText scrollingText = gameObject.GetComponentInChildren<ScrollingText>();
        scrollingText.PrintText(dialogue);
        */
        //yield break;

    }

    // Start is called before the first frame update
    void Start()
    {
        cancellationTokenSource = new CancellationTokenSource();
    }



    // Update is called once per frame
    void Update()
    {

    }
    void Awake()
    {
        //Debug.Log("PATRON AWAKE: " + this.id);
        instance = this;
        this.id = count++;
        includeEmptyItem = false;
        gameObject.name = "Patron " + this.id.ToString();
        preferenceID = Random.Range(1, 1);

        RollPreferences(patronType);
        //Debug.Log("Patron " + this.id + " created");
        PrintPatron();

        if (storeDisplayCollection == null)
        {
            //Debug.Log("storeDisplayCollection == Null");
            //Debug.Log(transform.parent.parent);
            //Debug.Log(transform.parent.parent.Find("StoreDisplays"));
            //storeDisplayCollection = transform.parent.parent.Find("StoreDisplays").gameObject;
            storeDisplayCollection = transform.parent.parent.GetComponent<StoreDisplayController>().storeDisplayList;
            //storeDisplayCollection = transform.Find("StoreDisplays");
        }
        ChooseItem();

    }

    // How should ChooseItem choose an item?
    // If 2+ patrons choose the same item a conflict could occur
    // ChooseItem can have patrons choose an item on a first-come-first-served basis
    // If a patron rejects the player's offer could another patron who wanted that item select that item?
    // 
    public void ChooseItem()
    {
        //Debug.Log("Count: " + (storeDisplayCollection.GetComponent<StoreDisplayCollection>().storeCollection.Count - 1).ToString());

        Debug.Log("Choosing Random Item..");
        Debug.Log("-1, " + (storeDisplayCollection.GetComponent<StoreDisplayCollection>().storeCollection.Count - 1));

        if(includeEmptyItem)
        {
            desiredItemIndex = Random.Range(-1, storeDisplayCollection.GetComponent<StoreDisplayCollection>().storeCollection.Count - 1);
        }
        else
        {
            desiredItemIndex = Random.Range(0, storeDisplayCollection.GetComponent<StoreDisplayCollection>().storeCollection.Count - 1);
        }
        
        if (desiredItemIndex != -1)
        {
            desiredItem = storeDisplayCollection.GetComponent<StoreDisplayCollection>().storeCollection[desiredItemIndex].Item2;
        }
        else
        {
            desiredItem = storeDisplayCollection.GetComponent<StoreDisplayCollection>().GetItemForSale(0);
        }
        Debug.Log("DESIRED ITEM INDEX:" + desiredItemIndex);
        desiredItem.PrintItemForSale();
        //Debug.Log("Desired item ID: " + Random.Range(-1, storeDisplayCollection.GetComponent<StoreDisplayCollection>().storeCollection.Count - 1));

        // patron budget = 90% - 150%
        budget = (long)Random.Range((float)desiredItem.basePrice * 0.9f, (float)desiredItem.basePrice * 1.5f);
    }

    private void OnDisable()
    {
        cancellationTokenSource.Cancel();
    }

    // Prompt StoreController to display ItemSaleDisplay and for the player to enter an offer\
    async Task RequestOfferAsync(CancellationToken cancellationToken)
    //IEnumerator RequestOffer()
    //void RequestOffer()
    {
        offerReceived = false;
        Debug.Log("Request Offer");

        // ## Calls StoreController.DisplayOfferPage()
        if (OnRequestOffer != null)
        {
            //StartCoroutine(OnRequestOffer(this));
            await (OnRequestOffer(this, cancellationToken));
            
            /*
            while(!offerReceived)
            {
                yield return null;
            }
            */
        }
        Debug.Log("END REQUEST OFFER");
    }

    //NPCPreference RollPreferences(PatronType patronType)
    void RollPreferences(PatronType patronType)
    {
        int random = Random.Range(1, 3);

        if(random == 1)
        {
            patronType = PatronType.Man;
        }
        else if (random == 2)
        {
            patronType = PatronType.Woman;
        }
        else if (random == 3)
        {
            patronType = PatronType.Child;
        }

        if (patronType == PatronType.Man)
        {
            //patronPreference = 
        }
        if(patronType == PatronType.Woman)
        {

        }
        if (patronType == PatronType.Child)
        {

        }
        //return null;
    }




    // This function processes the price the player is willing to sell the item for
    // Based on the patron's budget, which is a % of the base price like 120%, the patron will either accept or reject the offer.
    // If the patron rejects there is a chance the patron will walk out the store immediately, or give the player a chance to make another offer.
    int ProcessOffer(int offer)
    {
        int leave = 0;
        // choice is the patron's response to the offer
        // 0 = no
        // 1 = yes
        // 2 = maybe, player can remake offer
        int choice = -1;


        // if budget < offer

        if (offer > budget)
        {
            // 50/50 chance the patron leaves
            leave = Random.Range(0, 2);
            if(leave == 1)
            {
                // Patron leaves
                choice = 0;
                Debug.Log("Customer is anger and storms out.");
                return choice;
            }
            else
            {
                // Patron rejects offer, but player has opportunity to make another offer
                choice = 2;
                return choice;
            }
        }
        else
        {
            // MAYBE IMPLEMENT LATER:
            // even if the budget is greater than the offer,
            // some patrons may reject the offer to see if the player is willing to lower the price further

            // patron accepts offer
            choice = 1;
            return choice;
        }
    }


    public void PrintPatron()
    {
        //Debug.Log("Patron ID: " + this.id + "; patronType: " + patronType + "; Max Budget: " + budget);
    }




}
