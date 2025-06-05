using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CashRegister : Singleton<CashRegister>, IUsable
{
    public static CashRegister instance { get; private set; }
    bool choiceMade;

    public GameObject registerDialog;

    public Button organizeStoreButton;
    public Button openStoreButton;
    public Button cancelButton;
    public Button a;

    [SerializeField]
    PlayerController player;

    private CancellationTokenSource cancellationTokenSource;

    // Start is called before the first frame update
    void Start()
    {
        cancellationTokenSource = new CancellationTokenSource();
    }

    void Awake()
    {
        Debug.Log("CASH REGISTER AWAKE");
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }

        choiceMade = false;
        registerDialog.SetActive(false);
        //organizeStoreButton.onClick.AddListener(OrganizeStore);
        //openStoreButton.onClick.AddListener(PromptStore);
        //cancelButton.onClick.AddListener(CancelMenu);
        //Debug.Log(this.transform.position);
        //Debug.Log(registerDialog.transform.position);
        //Debug.Log(registerDialog.transform.parent);
        //Debug.Log(registerDialog.transform.parent.transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        if(!choiceMade && registerDialog.activeInHierarchy)
        {
            if(Input.GetButtonDown("Cancel"))
            {
                CancelMenu();
            }
        }
    }


    public Vector2 Position
    {
        get
        {
            return transform.position;
        }
    }

        /*
    public void Use(GameObject user)
    {

    }

    public void Highlight()
    {

    }
    */

    public IEnumerator InteractWithRegister()
    {
        Debug.Log("COROUTINE CR");
        choiceMade = false;
        player.StopPlayer();
        registerDialog.SetActive(true);

        while (!choiceMade)
        {
            //Debug.Log("choice not yet made");
            yield return null;
        }
        yield return 0;
    }
    
    // Unused?
    public async Task InteractWithRegisterAsync()
    {
        Debug.Log("ASYNC CR");
        var cancellationToken = cancellationTokenSource.Token;

        choiceMade = false;
        player.StopPlayer();
        registerDialog.SetActive(true);
        //await Task.WhenAny();

        while (!choiceMade)
        {
            await Task.Delay(1, cancellationToken);
        }

    }

    public void OrganizeStore()
    {
        choiceMade = true;
        player.StopPlayer();
        Debug.Log("Organizing Store");
        StoreController.instance.ArrangeStore();
        registerDialog.SetActive(false);
        player.ReleasePlayer();
    }

    public void Swobu()
    {
        PromptStore();
    }

    public async Task PromptStore()
    //public void PromptStore()
    {
        //var cancellationToken = cancellationTokenSource.Token;

        Debug.Log("PromptStore()");
        player.StopPlayer();
        //Debug.Log("Opening Store");
        choiceMade = true;
        registerDialog.SetActive(false);
        //while()

        //yield return StartCoroutine(StoreController.instance.OpenStore());

        //StartCoroutine(StoreController.instance.OpenStore());

        var openStoreTask = StoreController.instance.OpenStoreAsync();
        await openStoreTask;
        Debug.Log("End PromptStore");
    }

    public void CancelMenu()
    {
        Debug.Log(this + "Cancel");
        choiceMade = true;
        registerDialog.SetActive(false);
        player.ReleasePlayer();
    }

    void OnDisable()
    {
        cancellationTokenSource.Cancel();
    }
}
