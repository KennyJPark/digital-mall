using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;


#if ENABLE_INPUT_SYSTEM
// New input system backends are enabled.
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
// Old input backends are enabled.
#endif

// NOTE: Both can be true at the same time as it is possible to select "Both"
//       under "Active Input Handling".


//public class PlayerController : Singleton<PlayerController>
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance { get; private set; }
    
    private PlayerControls playerControls;

    // Currently unused
    public delegate void InteractAction();
    public static event InteractAction OnInteract;

    public GameObject inventory;
    public InventoryObject inventoryObj;

    public string playerName;
    public float playerMoney;

    public float playerExperience;
    public int playerLevel;

    // Movement
    [SerializeField]
    float speed = 4.0f;

    bool isMoving;
    // Whether the player can perform actions/inputs
    public static bool isFrozen;
    public static bool isLocked;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    Vector2 lastPosition;
    Vector2 currentPosition;
    static Vector2 lookDirection = new Vector2(1, 0);
    static Vector2 movement = new Vector2(0, 0);

    // Stats, etc
    public static float money = 1000.0f;

    public GameObject storeDisplayController;
    public GameObject storeDisplayObj;
    public StoreDisplay storeDisplay;

    void Awake()
    {
        Debug.Log("Player Controller - AWAKE");
        

        if (Mall.Instance.Player != null)
        {
            Debug.Log("malphs");
            Destroy(gameObject);
            return;
        }
        else
        {
            Debug.Log("NOT malphs");
        }

        gameObject.transform.SetParent(null);

        Mall.Instance.Player = this;
        DontDestroyOnLoad(gameObject);
        
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
        }


        Debug.Log("pre goal");
        if (Mall.Instance.MainCamera == null)
        {
            Debug.Log("CAMERA NULL");
        }
        else
        {
            Debug.Log("goal");
            Mall.Instance.MainCamera.GetComponent<PlayerCamera>().LookAtPlayer();
            Mall.Instance.MainCamera.GetComponent<PlayerCamera>().FollowPlayer();
        }



    }

    // Start is called before the first frame update
    void Start()
    {
        if (playerName == "")
            playerName = "Maru";

        //Debug.Log("My name is " + playerName);

        playerControls.Floor.Interact.performed += ctx => CheckInteraction();

        rigidbody2d = GetComponent<Rigidbody2D>();
        //ventoryObj.CloseInventory();
        CheckHighlight();
        if (Mall.Instance.MainCamera == null)
        {
            // Check CM vcam1 has PlayerCamera.cs
            Debug.Log("CAMERA NULL");
            
        }
        Mall.Instance.MainCamera.GetComponent<PlayerCamera>().LookAtPlayer();
        Mall.Instance.MainCamera.GetComponent<PlayerCamera>().FollowPlayer();
    }

    void OnEnable()
    {
        if (playerControls != null)
        {
            playerControls.Enable();
        }
            
    }

    void OnDisable()
    {
        if(playerControls != null)
        {
            playerControls.Disable();
        }
        
    }

    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }

    public void SetLookDirection(Vector2 facingDirection)
    {
        lookDirection = facingDirection;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        float movementInput = playerControls.Floor.Move.ReadValue<float>();

        Vector3 currentPosition = transform.position;
        currentPosition.x += movementInput * speed * Time.deltaTime;
        transform.position = currentPosition;
        */

        if (!isFrozen)
        {
            
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            movement = new Vector2(horizontal, vertical);
            


            if (!Mathf.Approximately(movement.x, 0.0f) || !Mathf.Approximately(movement.y, 0.0f))
            {
                lookDirection.Set(movement.x, movement.y);
                lookDirection.Normalize();
                //Debug.Log("Walking");
            }

            if (Input.GetButtonDown("Interact"))
            {
                StartCoroutine(CheckInteractionRoutine());
            }

            if (Input.GetButtonDown("Inventory"))
            {
                StartCoroutine(OpenInventory());
            }


        }
        else if (isFrozen)
        {
            if (Input.GetButtonDown("Inventory"))
            {
                StartCoroutine(OpenInventory());
            }
            /*
            if (Input.GetButtonDown("Interact"))
            {
                // Empty
                CancelInteraction();
            }
            */
        }
    }

    public void ToggleControl(bool canControl)
    {
        /*
        m_CanControl = canControl;
        if (canControl)
        {
            m_MoveAction.Enable();
            m_NextItemAction.Enable();
            m_PrevItemAction.Enable();
            m_UseItemAction.Enable();
        }
        else
        {
            m_MoveAction.Disable();
            m_NextItemAction.Disable();
            m_PrevItemAction.Disable();
            m_UseItemAction.Disable();
        }
        */
    }

    void FixedUpdate()
    {
        lastPosition = currentPosition;
        if (!isFrozen)
        {
            
            Vector2 position = rigidbody2d.position;
            position.x = position.x + speed * horizontal * Time.deltaTime;
            position.y = position.y + speed * vertical * Time.deltaTime;
            //position.x = position.x + 3.0f * horizontal * Time.deltaTime;
            //position.y = position.y + 3.0f * vertical * Time.deltaTime;

            rigidbody2d.MovePosition(position);
            
            currentPosition = position;
            UpdateMovementStatus();
            CheckHighlight();

            /*
            float movementInput = playerControls.Floor.Movement.ReadValue<float>();

            Vector3 currentPosition = transform.position;
            currentPosition.x += movementInput * speed * Time.deltaTime;
            transform.position = currentPosition;
            */
        }
        // Testing save/load
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Saving inventory..");
            inventoryObj.Save();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Loading inventory..");
            inventoryObj.Load();
        }
        */
    }

    void CheckHighlight()
    {
        LayerMask mask = ~LayerMask.GetMask("Player");

        RaycastHit2D hitUsable = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.0f, mask);
        if (hitUsable.collider != null)
        {
            if (hitUsable != null)
            {
                //Debug.Log("OBJECT HIT");
                //Debug.Log(hitUsable.collider.gameObject.name);

                //if (hitUsable.collider.gameObject is MonoBehaviour)
                if (hitUsable.collider.gameObject.GetComponent<MonoBehaviour>() != null)
                {
                    //Debug.Log("object IS MONOBEHAVIOR");
                    if (hitUsable.collider.GetComponent<IUsable>() != null)
                    {
                        //Debug.Log("object IS USABLE");
                        UsableHighlighter.Instance.SetHighlight(hitUsable.collider.gameObject);
                    }
                }
            }

        }
        else
        {
            //Debug.Log("REMOVING HIGHLIGHT");
            UsableHighlighter.Instance.RemoveHighlight();
        }
    }

    void CheckInteraction()
    {
        RaycastHit2D hitDoor = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("Door"));
        if (hitDoor.collider != null)
        {
            DoorController door = hitDoor.collider.GetComponent<DoorController>();
            if (door != null)
            {
                //character.DisplayDialog();
                Debug.Log("Enter Door");
                door.RequestChangeScene();

            }
        }
        RaycastHit2D hitRegister = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("Register"));
        if (hitRegister.collider != null)
        {
            CashRegister register = hitRegister.collider.GetComponent<CashRegister>();
            if (register != null)
            {

                StopPlayer();

                StartCoroutine(register.InteractWithRegister());
             
                //Debug.Log("This runs before you finish editing (REGISTER)");
                //await Task.Run(() => register.InteractWithRegisterAsync());
            }
        }

        RaycastHit2D hitDisplay = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("ShopDisplay"));
        if (hitDisplay.collider != null)
        {
            //StoreDisplay storeDisplay = hitDisplay.collider.GetComponent<StoreDisplay>();
            storeDisplayObj = hitDisplay.collider.gameObject;
            storeDisplay = storeDisplayObj.GetComponent<StoreDisplay>();
            //storeDisplayController = storeDisplay.transform.parent.GetComponent<StoreDisplayController>();
            if (storeDisplay != null)
            {
                StopPlayer();
                //storeDisplayController.GetComponent<StoreDisplayController>().InteractWithStoreDisplay(storeDisplay);
                StartCoroutine(storeDisplayController.GetComponent<StoreDisplayController>().InteractWithStoreDisplay(storeDisplay));
                //Debug.Log("This runs before you finish editing");
                //ReleasePlayer();
                //await storeDisplayController.InteractWithStoreDisplay(storeDisplay);

            }
            else
            {
                //Debug.Log("No furniture");
            }
        }
        //*/
    }

    IEnumerator CheckInteractionRoutine()
    {
        /*
        RaycastHit2D hitDoor = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("Door"));
        if (hitDoor.collider != null)
        {
            DoorController door = hitDoor.collider.GetComponent<DoorController>();
            if (door != null)
            {
                //character.DisplayDialog();
                Debug.Log("Enter Door");
                door.RequestChangeScene();

            }
        }
        */
        RaycastHit2D hitRegister = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("Register"));
        if (hitRegister.collider != null)
        {
            CashRegister register = hitRegister.collider.GetComponent<CashRegister>();
            if (register != null)
            {

                StopPlayer();

                enabled = false;
                yield return StartCoroutine(register.InteractWithRegister());
                //await register.InteractWithRegisterAsync();
                enabled = true;
                Debug.Log("This runs before you finish editing (REGISTER)");
                
            }
        }

        RaycastHit2D hitDisplay = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("ShopDisplay"));
        if (hitDisplay.collider != null)
        {
            //StoreDisplay storeDisplay = hitDisplay.collider.GetComponent<StoreDisplay>();
            storeDisplayObj = hitDisplay.collider.gameObject;
            storeDisplay = storeDisplayObj.GetComponent<StoreDisplay>();
            //storeDisplayController = storeDisplay.transform.parent.GetComponent<StoreDisplayController>();
            if (storeDisplay != null)
            {
                StopPlayer();
                enabled = false;
                yield return storeDisplayController.GetComponent<StoreDisplayController>().InteractWithStoreDisplay(storeDisplay);
                enabled = true;
                Debug.Log("This runs before you finish editing");
                //ReleasePlayer();
                //await storeDisplayController.InteractWithStoreDisplay(storeDisplay);

            }
            else
            {
                Debug.Log("No furniture");
            }
        }
        //*/
        yield return null;
    }

    /*
    async Task CheckInteractionRoutine()
    {
        RaycastHit2D hitDoor = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("Door"));
        if (hitDoor.collider != null)
        {
            DoorController door = hitDoor.collider.GetComponent<DoorController>();
            if (door != null)
            {
                //character.DisplayDialog();
                Debug.Log("Enter Door");
                door.RequestChangeScene();

            }
        }
        RaycastHit2D hitRegister = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("Register"));
        if (hitRegister.collider != null)
        {
            CashRegister register = hitRegister.collider.GetComponent<CashRegister>();
            if (register != null)
            {

                StopPlayer();

                enabled = false;
                yield return StartCoroutine(register.InteractWithRegister());
                //await register.InteractWithRegisterAsync();
                enabled = true;
                Debug.Log("This runs before you finish editing (REGISTER)");

            }
        }

        RaycastHit2D hitDisplay = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("ShopDisplay"));
        if (hitDisplay.collider != null)
        {
            //StoreDisplay storeDisplay = hitDisplay.collider.GetComponent<StoreDisplay>();
            storeDisplayObj = hitDisplay.collider.gameObject;
            storeDisplay = storeDisplayObj.GetComponent<StoreDisplay>();
            //storeDisplayController = storeDisplay.transform.parent.GetComponent<StoreDisplayController>();
            if (storeDisplay != null)
            {
                StopPlayer();
                enabled = false;
                yield return storeDisplayController.GetComponent<StoreDisplayController>().InteractWithStoreDisplay(storeDisplay);
                enabled = true;
                Debug.Log("This runs before you finish editing");
                //ReleasePlayer();
                //await storeDisplayController.InteractWithStoreDisplay(storeDisplay);

            }
            else
            {
                Debug.Log("No furniture");
            }
        }
    }
    */

    void CancelInteraction()
    {

    }

    public bool IsPlayerMoving()
    {
        return isMoving;
    }

    void UpdateMovementStatus()
    {
        if (lastPosition != currentPosition)
            isMoving = true;
        else
            isMoving = false;
    }

    public void StopPlayer()
    {
        Debug.Log("Player: StopPlayer");
        isFrozen = true;
        playerControls.Disable();
        //movement = new Vector2(0.0f, 0.0f);
    }

    public void FreezePlayer()
    {
        isFrozen = true;
        playerControls.Disable();
        //movement = new Vector2(0.0f, 0.0f);
    }

    public void ReleasePlayer()
    {
        Debug.Log("Player: ReleasePlayer");
        isFrozen = false;
        playerControls.Enable();
    }

    /*
    public static void ToggleFrozen()
    {
        if (isFrozen)
        {
            isFrozen = false;
            Debug.Log("NOT Frozen");
        }
        else
        {
            isFrozen = true;
            movement = new Vector2(0, 0);
            Debug.Log("Frozen");
        }
        return;
    }
    */


   // void OpenInventory()
    public IEnumerator OpenInventory()
    {
        if(InventoryController.Instance.isOpen)
        {
            Debug.Log("Close Inventory");
            ReleasePlayer();
            InventoryController.Instance.CloseInventory();
        }
        else
        {
            Debug.Log("Open Inventory");
            StopPlayer();
            InventoryController.Instance.ShowInventory();
            //inventoryWindow.ShowInventory();
            InventoryController.Instance.ShowInventory();
            
        }

        yield break;
    }

    void ChangeMoney(float num)
    {
        money += num;
        UIMoney.instance.SetMoney(money);
    }

    /*
    [System.Serializable]
    public struct PlayerSaveData
    {
        public Vector3 Position;

        public int Coins;
        public List<InventorySaveData> Inventory;
    }
    */
}
