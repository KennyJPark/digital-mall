using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mall : MonoBehaviour
{
    public static Mall instance { get; private set; }

    //public delegate void DoorAction();
    //public static event DoorAction OnDoorOpen;

    public ItemDatabaseObject itemDatabase;

    static int[] doorTypes;
    private int doorType;

    enum DayPhase
    {
        dawn,
        morning,
        afternoon,
        evening,
        dusk,
        night
    }

    static DayPhase phase;

    public static int timeOfDay;
    public static int elapsedDays;

    public int testInventoryNum = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        instance = this;
        phase = DayPhase.morning;
    }

    void PassTime()
    {
        if (phase == DayPhase.dawn)
            phase = DayPhase.morning;

        else if (phase == DayPhase.morning)
            phase = DayPhase.afternoon;

        else if (phase == DayPhase.afternoon)
            phase = DayPhase.evening;

        else if (phase == DayPhase.evening)
            phase = DayPhase.dusk;

        else if (phase == DayPhase.dusk)
            phase = DayPhase.night;

        else if (phase == DayPhase.night)
        {
            phase = DayPhase.dawn;
            elapsedDays++;
        }

    }

    public void ChangeScene(int doorID)
    {
        Debug.Log("Changing Scene");
        StartCoroutine(LoadDoorAsyncScene(doorID));
    }

    IEnumerator LoadDoorAsyncScene(int doorID)
    {
        doorType = doorID;
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

    void DawnEvent()
    {

    }

    void MorningEvent()
    {

    }

    public void FreezeWorld()
    {

    }

}
