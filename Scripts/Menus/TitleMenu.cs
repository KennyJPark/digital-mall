using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    public GameObject mainMenuSounds;
    public GameObject uiController;
    /*
    [SerializeField]
    Button StartButton;
    [SerializeField]
    Button NewGameButton;

    // Hidden if no saved games
    [SerializeField]
    Button LoadGameButton;

    [SerializeField]
    Button OptionsButton;
    [SerializeField]
    Button QuitButton;
    */

    public GameObject sceneChanger;

    // Opens sub-menu to either start new game..
    // ..or load game if saved data exists

    void Awake()
    {
        Assert.AreNotEqual(null, sceneChanger);
        Assert.AreNotEqual(null, mainMenuSounds);
        Assert.AreNotEqual(null, uiController);
    }

    public void StartGame()
    {
        // Edit Scene indices in:
        // FILE/BUILD SETTINGS
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
        Debug.Log("Start Game Sub-Menu");
        mainMenuSounds.GetComponent<SFXController>().gameStart.Play();
        //Debug.Log(gameObject.activeInHierarchy);
    }

    public void PreStartNewGame()
    {
        StartCoroutine(StartNewGame());
    }

    //public void StartNewGame()
    public IEnumerator StartNewGame()
    {
        Debug.Log("Start New Game");
        
        mainMenuSounds.GetComponent<SFXController>().newGameStartSound.Play();
        //yield return StartCoroutine(uiController.GetComponent<UIController>().FadeToBlack(true));
        
        StartCoroutine(sceneChanger.GetComponent<SceneChanger>().ChangeScene("Scenes/Prologue"));
        yield return null;
        //Debug.Log(gameObject.activeInHierarchy);
    }

    public void Options()
    {
        Debug.Log("Options");
        mainMenuSounds.GetComponent<SFXController>().gameStart.Play();
    }

    public void MenuBack()
    {
        Debug.Log("Back");
        mainMenuSounds.GetComponent<SFXController>().menuBack.Play();
    }

    public void LoadSavedGame()
    {
        Debug.Log("Load Game");
        //Debug.Log(gameObject.activeInHierarchy);
    }
    public void QuitGame()
    {
        Debug.Log("Quit");

        // Future: Add confirmation to quit
        Application.Quit();
    }

}
