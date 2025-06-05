using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    static int[] doorTypes;

    public int doorType;
    public string doorDestination;

    //public GameObject storeFront;

    void Awake()
    {

    }

    void Start()
    {

    }

    void OnEnable()
    {
        /*
        print("DOOR ENABLE");
        if (storeFront == null)
        {
            print("Null");
            print(transform);
            print(transform.parent);
            print(transform.parent.gameObject);
            storeFront = transform.parent.gameObject;
        }
        */
    }

    public void RequestChangeScene()
    {
        Mall.Instance.ChangeScene(doorType);
        //StartCoroutine(LoadDoorAsyncScene());
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        RequestChangeScene();
    }


    

    private void OnDoorwayOpen()
    {

    }

    void OnDestroy()
    {

    }
    /*
    */

}
