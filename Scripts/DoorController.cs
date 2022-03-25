using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    static int[] doorTypes;

    public int doorType;

    public GameObject storeFront;

    void Awake()
    {
        if(storeFront == null)
        {
            storeFront = transform.parent.gameObject;
        }
    }

    public void RequestChangeScene()
    {
        Mall.instance.ChangeScene(doorType);
        //StartCoroutine(LoadDoorAsyncScene());
    }

    /*
    IEnumerator LoadDoorAsyncScene()
    {
        string destinationScene = "";
        //  If player is using door from their store
        if (doorType == 0)
        {
            destinationScene = "Scenes/MallFloor";
        }
        //  If player is using door to their store
        else if (doorType == 1)
        {
            destinationScene = "Scenes/PlayerStore";
        }
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(destinationScene);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    */

    private void OnDoorwayOpen()
    {

    }

    void OnDestroy()
    {

    }
    /*
    */

}
