using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MainMenu : MonoBehaviour
{
    public GameObject uiController;
    public GameObject sfxController;
    public GameObject sceneChanger;

    void Awake()
    {
        // Mandatory Objects
        Assert.AreNotEqual(null, uiController);
        Assert.AreNotEqual(null, sfxController);
        Assert.AreNotEqual(null, sceneChanger);
    }

}
