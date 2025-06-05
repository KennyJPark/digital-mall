using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Prologue : MonoBehaviour
{
    public GameObject uiController;
    public GameObject sfxController;
    public GameObject sceneChanger;

    public static PlayerController player;
    public PositionController positionController;

    public GameObject storeController;

    // need array of events/

    void Awake()
    {
        Debug.Log("PROLOGUE AWAKE");
        // Mandatory Objects
        Assert.AreNotEqual(null, uiController);
        Assert.AreNotEqual(null, sfxController);
        Assert.AreNotEqual(null, sceneChanger);

    }

    void Start()
    {
        Debug.Log("PROLOGUE START");

        if (player == null)
        {
            player = Mall.Instance.Player;
        }
        if(positionController == null)
        {
            positionController = gameObject.GetComponent<PositionController>();
        }

        Assert.AreNotEqual(null, player);
        Assert.AreNotEqual(null, positionController);

        positionController.SetStorePositions();

        if (!storeController.GetComponent<StoreController>().IsStoreOpen())
            //Debug.Log("DABU");

        StartCoroutine(PrepareScenario());
    }

    IEnumerator PrepareScenario()
    {
        player.FreezePlayer();
        yield return null;
        //yield return StartCoroutine(uiController.GetComponent<UIController>().FadeToBlack(false));
        player.ReleasePlayer();

        if(player is MonoBehaviour)
        {
            //Debug.Log("PLAYER IS MONOBEHAVIOR");
        }
    }

}
