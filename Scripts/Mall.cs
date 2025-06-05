using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
//using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

/// <summary>
/// The GameManager(Mall) is the entry point to all the game system. It's execution order is set very low to make sure
/// its Awake function is called as early as possible so the instance if valid on other Scripts. 
/// </summary>
[DefaultExecutionOrder(-9999)]

public class Mall : MonoBehaviour
{

    //public static Mall instance { get; private set; }


    private static Mall s_Instance;
    //private static Mall instance;

#if UNITY_EDITOR
    //As our manager run first, it will also be destroyed first when the app will be exiting, which lead to s_Instance
    //to become null and so will trigger another instantiate in edit mode (as we dynamically instantiate the Manager)
    //so this is set to true when destroyed, so we do not reinstantiate a new one
    private static bool s_IsQuitting = false;
#endif

    public static Mall Instance
    {
        get
        {
#if UNITY_EDITOR
            if (!Application.isPlaying || s_IsQuitting)
                return null;

            if (s_Instance == null)
            {

                // REMOVE LATER
                //in editor, we can start any scene to test, so we are not sure the game manager will have been
                //created by the first scene starting the game. So we load it manually. This check is useless in
                //player build as the 1st scene will have created the GameManager so it will always exists.
                Instantiate(Resources.Load<Mall>("Mall"));
            }
#endif
            return s_Instance;
        }
    }

    public UIController UIController;

    //public delegate void DoorAction();
    //public static event DoorAction OnDoorOpen;

    public PlayerController Player { get; set; }
    public ItemDatabaseObject itemDatabase;
    public CinemachineVirtualCamera MainCamera { get; set; }

    public SceneData LoadedSceneData { get; set; }

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

    private List<SpawnPoint> SpawnPoints = new List<SpawnPoint>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        Debug.Log("Mall awake");

        s_Instance = this;
        DontDestroyOnLoad(gameObject);

        phase = DayPhase.morning;
    }

    private void OnGUI()
    {
        GUI.color = Color.yellow;
        GUI.Label(new Rect(10, 60, 100, 30), "Phase: " + phase);
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        PlayerData data = new PlayerData();
        data.playerExperience = Player.playerExperience;
        data.playerMoney = Player.playerMoney;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            Player.playerExperience = data.playerExperience;
            Player.playerMoney = data.playerMoney;
        }
    }

#if UNITY_EDITOR
    private void OnDestroy()
    {
        s_IsQuitting = true;
    }
#endif


    public void Pause()
    {
        //Player.ToggleControl(false);
    }

    public void Resume()
    {
        //Player.ToggleControl(true);
    }
    public void RegisterSpawn(SpawnPoint spawn)
    {
        Debug.Log("Registering spawn index: " + spawn.SpawnIndex);
        /*
        if (Player == null && spawn.SpawnIndex == 0)
        { //if we have no player, we need to create one
            Instantiate(Resources.Load<PlayerController>("Character"));
            spawn.SpawnHere();
        }
        */
        SpawnPoints.Add(spawn);
    }

    public void UnregisterSpawn(SpawnPoint spawn)
    {
        SpawnPoints.Remove(spawn);
    }

    
    public void MoveTo(int targetScene, int targetSpawn)
    {
        /*
        Pause();
        SaveSystem.SaveSceneData();
        UIHandler.FadeToBlack(() =>
        {
            var asyncop = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Single);
            asyncop.completed += operation =>
            {
                m_IsTicking = true;

                foreach (var active in m_ActiveTransitions)
                {
                    if (active.SpawnIndex == targetSpawn)
                    {
                        active.SpawnHere();
                        SaveSystem.LoadSceneData();
                    }
                }

                UIHandler.SceneLoaded();
                UIHandler.FadeFromBlack(() =>
                {
                    Player.ToggleControl(true);
                });
            };
        });
        */
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
        int targetSpawn = 0;
        doorType = doorID;
        string destinationScene = "";
        //  If player is using door from their store
        if (doorType == 0)
        {
            destinationScene = "Scenes/MallFloor";
            targetSpawn = 0;
        }
        //  If player is using door to their store
        else if (doorType == 1)
        {
            destinationScene = "Scenes/PlayerStore";
            targetSpawn = 0;
        }

        //StartCoroutine(UIController.Instance.FadeToBlack());
        //StartCoroutine(UIController.Instance.FadeFromBlack());
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(destinationScene);


        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        foreach (var active in SpawnPoints)
        {
            if (active.SpawnIndex == targetSpawn)
            {
                active.SpawnHere();
                //SaveSystem.LoadSceneData();
            }
        }
        Debug.Log("Scene Loaded");
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

[Serializable]
class PlayerData
{
    public float playerMoney;
    public float playerExperience;
}

/*

using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace HappyHarvest
{
    /// <summary>
    /// The GameManager is the entry point to all the game system. It's execution order is set very low to make sure
    /// its Awake function is called as early as possible so the instance if valid on other Scripts. 
    /// </summary>
    [DefaultExecutionOrder(-9999)]
    public class GameManager : MonoBehaviour
    {
        private static GameManager s_Instance;
        
        
#if UNITY_EDITOR
        //As our manager run first, it will also be destroyed first when the app will be exiting, which lead to s_Instance
        //to become null and so will trigger another instantiate in edit mode (as we dynamically instantiate the Manager)
        //so this is set to true when destroyed, so we do not reinstantiate a new one
        private static bool s_IsQuitting = false;
#endif
        public static GameManager Instance 
        {
            get
            {
#if UNITY_EDITOR
                if (!Application.isPlaying || s_IsQuitting)
                    return null;
                
                if (s_Instance == null)
                {
                    //in editor, we can start any scene to test, so we are not sure the game manager will have been
                    //created by the first scene starting the game. So we load it manually. This check is useless in
                    //player build as the 1st scene will have created the GameManager so it will always exists.
                    Instantiate(Resources.Load<GameManager>("GameManager"));
                }
#endif
                return s_Instance;
            }
        }

        public TerrainManager Terrain { get; set; }
        public PlayerController Player { get; set; }
        public DayCycleHandler DayCycleHandler { get; set; }
        public WeatherSystem WeatherSystem { get; set; }
        public CinemachineVirtualCamera MainCamera { get; set; }
        public Tilemap WalkSurfaceTilemap { get; set; }
        
        public SceneData LoadedSceneData { get; set; }
        
        // Will return the ratio of time for the current day between 0 (00:00) and 1 (23:59).
        public float CurrentDayRatio => m_CurrentTimeOfTheDay / DayDurationInSeconds;

        [Header("Market")] 
        public Item[] MarketEntries;
        
        [Header("Time settings")]
        [Min(1.0f)] 
        public float DayDurationInSeconds;
        public float StartingTime = 0.0f;

        [Header("Data")] 
        public ItemDatabase ItemDatabase;
        public CropDatabase CropDatabase;

        public Storage Storage;

        private bool m_IsTicking;
        
        private List<DayEventHandler> m_EventHandlers = new();
        private List<SpawnPoint> m_ActiveTransitions = new List<SpawnPoint>();
        
        private float m_CurrentTimeOfTheDay;

        private void Awake()
        {
            s_Instance = this;
            DontDestroyOnLoad(gameObject);
            
            m_IsTicking = true;
            
            ItemDatabase.Init();
            CropDatabase.Init();
            
            Storage = new Storage();
            
            m_CurrentTimeOfTheDay = StartingTime;
            
            //we need to ensure that we don't have a day length at 0, otherwise we will get stuck into infinite loop in update
            //(and a day with 0 length makes no sense)
            if (DayDurationInSeconds <= 0.0f)
            {
                DayDurationInSeconds = 1.0f;
                Debug.LogError("The day length on the GameManager is set to 0, the length need to be set to a positive value");
            }
        }

        private void Start()
        {
            m_CurrentTimeOfTheDay = StartingTime;
            
            UIHandler.SceneLoaded();
        }

#if UNITY_EDITOR
        private void OnDestroy()
        {
            s_IsQuitting = true;
        }
#endif

        private void Update()
        {
            if (m_IsTicking)
            {
                float previousRatio = CurrentDayRatio;
                m_CurrentTimeOfTheDay += Time.deltaTime;

                while (m_CurrentTimeOfTheDay > DayDurationInSeconds)
                    m_CurrentTimeOfTheDay -= DayDurationInSeconds;

                foreach (var handler in m_EventHandlers)
                {
                    foreach (var evt in handler.Events)
                    {
                        bool prev = evt.IsInRange(previousRatio);
                        bool current = evt.IsInRange(CurrentDayRatio);
                    
                        if (prev && !current)
                        {
                            evt.OffEvent.Invoke();
                        }
                        else if (!prev && current)
                        {
                            evt.OnEvents.Invoke();
                        }
                    }
                }
                
                if(DayCycleHandler != null)
                    DayCycleHandler.Tick();
            }
        }

        public void Pause()
        {
            m_IsTicking = false;
            Player.ToggleControl(false);
        }

        public void Resume()
        {
            m_IsTicking = true;
            Player.ToggleControl(true);
        }

        public void RegisterSpawn(SpawnPoint spawn)
        {
            if (Player == null && spawn.SpawnIndex == 0)
            { //if we have no player, we need to create one
                Instantiate(Resources.Load<PlayerController>("Character"));
                spawn.SpawnHere();
            }
            
            m_ActiveTransitions.Add(spawn);
        }

        public void UnregisterSpawn(SpawnPoint spawn)
        {
            m_ActiveTransitions.Remove(spawn);
        }

        public void MoveTo(int targetScene, int targetSpawn)
        {
            Pause();
            SaveSystem.SaveSceneData();
            UIHandler.FadeToBlack(() =>
            {
                var asyncop = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Single);
                asyncop.completed += operation =>
                {
                    m_IsTicking = true;
                    
                    foreach (var active in m_ActiveTransitions)
                    {
                        if (active.SpawnIndex == targetSpawn)
                        {
                            active.SpawnHere();
                            SaveSystem.LoadSceneData();
                        }
                    }
                    
                    UIHandler.SceneLoaded();
                    UIHandler.FadeFromBlack(() =>
                    {
                        Player.ToggleControl(true);
                    });
                };
            });

            
        }
        
        /// <summary>
        /// Will return the current time as a string in format of "xx:xx" 
        /// </summary>
        /// <returns></returns>
        public string CurrentTimeAsString()
        {
            return GetTimeAsString(CurrentDayRatio);
        }

        /// <summary>
        /// Return in the format "xx:xx" the given ration (between 0 and 1) of time
        /// </summary>
        /// <param name="ratio"></param>
        /// <returns></returns>
        public static string GetTimeAsString(float ratio)
        {
            var hour = GetHourFromRatio(ratio);
            var minute = GetMinuteFromRatio(ratio);

            return $"{hour}:{minute:00}";
        }

        
        public static int GetHourFromRatio(float ratio)
        {
            var time = ratio * 24.0f;
            var hour = Mathf.FloorToInt(time);

            return hour;
        }

        public static int GetMinuteFromRatio(float ratio)
        {
            var time = ratio * 24.0f;
            var minute = Mathf.FloorToInt((time - Mathf.FloorToInt(time)) * 60.0f);

            return minute;
        }
        
        public static void RegisterEventHandler(DayEventHandler handler)
        {
            foreach (var evt in handler.Events)
            {
                if (evt.IsInRange(GameManager.Instance.CurrentDayRatio))
                {
                    evt.OnEvents.Invoke();
                }
                else
                {
                    evt.OffEvent.Invoke();
                }
            }
            
            Instance.m_EventHandlers.Add(handler);
        }

        public static void RemoveEventHandler(DayEventHandler handler)
        {
            Instance?.m_EventHandlers.Remove(handler);
        }
    }
}

*/