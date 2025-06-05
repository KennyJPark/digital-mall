using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// Highlights object player is facing
// Attached to standalone GameObject in Scene
public class UsableHighlighter : Singleton<UsableHighlighter>
{
    //public Sprite highlightIndicator;
    public static StoreController store;
    public FloatingUI floater;
    public SpriteRenderer spriteRenderer;
    bool isInit = false;

    public void SetHighlight(GameObject go)
    {
        //Debug.Log("SET HIGHLIGHT");
        if(!isInit)
        {
            Init();
        }
        
        if (!go.TryGetComponent(out BoxCollider2D boxCollider))
        {
            return;
        }

        Color color = spriteRenderer.color;
        color.a = 1;
        spriteRenderer.color = color;

        gameObject.transform.position = Mall.Instance.Player.GetPosition();
        Vector3 posOffset = new Vector3();
        
        //posOffset = go.transform.position;


        posOffset = go.GetComponent<BoxCollider2D>().bounds.size;
        
        //Debug.Log(go.GetComponent<BoxCollider2D>().bounds.size);
        //Debug.Log(posOffset);
        if (go.name == "CashRegister")
        {
            posOffset.y = ((posOffset.y + go.transform.position.y) * go.GetComponent<BoxCollider2D>().offset.y);
        }
        else if(go.name.Contains("StoreDisplay"))
        {
            //posOffset.y = ((posOffset.y + go.GetComponent<BoxCollider2D>().offset.y) * 5.0f);
            posOffset.y = (posOffset.y + (Mathf.Abs(go.GetComponent<BoxCollider2D>().size.y)/2f));
            //posOffset.y = ((posOffset.y + Mathf.Abs(go.transform.position.y) + (Mathf.Abs(go.GetComponent<BoxCollider2D>().size.y * 2f))));
        }
        else
        {
            
        }

        posOffset.x = go.transform.position.x;
        transform.position = posOffset;
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }
    }

    public void RemoveHighlight()
    {
        Vector3 resetPos = new Vector3();
        gameObject.transform.position = resetPos;
        gameObject.SetActive(false);
    }

    void Awake()
    {

        //gameObject.SetActive(false);
        //gameObject.SetActive(true);
    }

    void Start()
    {
         
    }
    
    void PauseHighlight()
    {
        Debug.Log("PAUSE");
        //this.enabled = false;
        gameObject.SetActive(false);
        
    }
    void ResumeHighlight()
    {
        Debug.Log("RESUME");
        //this.enabled = true;
        gameObject.SetActive(true);
    }

    void Init()
    {
        //Color initColor = spriteRenderer.color;
        //initColor.a = 0;
        //spriteRenderer.color = initColor;
        Debug.Log("HIGHLIGHT INIT");
        isInit = true;
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }
        if (Mall.Instance.Player == null)
        {
            Debug.Log("PlayerInstanceNull");
        }

        if (store == null)
        {
            if (StoreController.instance != null)
            {
                store = StoreController.instance;
            }
            else
            {
                Debug.Log("StoreInstanceNull");
            }
            store.StoreOpened += PauseHighlight;
            store.StoreClosed += ResumeHighlight;
        }


        floater.frequency = 3.0f;

    }

    void OnDisable()
    {

    }

    void OnDestroy()
    {
        if(store != null)
        {
            store.StoreOpened -= PauseHighlight;
            store.StoreClosed -= ResumeHighlight;
        }

    }
}
