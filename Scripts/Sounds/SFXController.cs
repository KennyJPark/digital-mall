using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SFXController : MonoBehaviour
{
    public AudioSource newGameStartSound;
    public AudioSource gameStart;
    public AudioSource menuBack;

    void Awake()
    {
        Assert.AreNotEqual(null, newGameStartSound);
        Assert.AreNotEqual(null, gameStart);
        Assert.AreNotEqual(null, menuBack);
        DontDestroyOnLoad(this.gameObject);
    }
}
