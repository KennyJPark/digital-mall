using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MainMenuSounds : MonoBehaviour
{
    public AudioSource newGameStartSound;
    public AudioSource GameStart;

    void Awake()
    {
        Assert.AreNotEqual(null, newGameStartSound);
        Assert.AreNotEqual(null, GameStart);
        DontDestroyOnLoad(this.gameObject);
    }
}
