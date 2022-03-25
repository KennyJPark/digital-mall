using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CashRegister : MonoBehaviour
{
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

        choiceMade = false;
        registerDialog.SetActive(false);
        organizeStoreButton.onClick.AddListener(OrganizeStore);
        openStoreButton.onClick.AddListener(PromptStore);
        cancelButton.onClick.AddListener(CancelMenu);
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

    
    public IEnumerator InteractWithRegister()
    {
        choiceMade = false;
        player.StopPlayer();
        registerDialog.SetActive(true);

        while (!choiceMade)
        {
            Debug.Log("choice not yet made");
            yield return null;
        }
        yield return 0;
    }
    
    // Unused?
    public async Task InteractWithRegisterAsync()
    {
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

    private void OrganizeStore()
    {
        choiceMade = true;
        player.StopPlayer();
        Debug.Log("Organizing Store");
        StoreController.instance.ArrangeStore();
        registerDialog.SetActive(false);
        player.ReleasePlayer();
    }

    private void PromptStore()
    //public IEnumerator PromptStore()
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

        StoreController.instance.OpenStore();

        //player.ReleasePlayer();
    }

    private void CancelMenu()
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
