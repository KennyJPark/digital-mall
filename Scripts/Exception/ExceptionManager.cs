using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExceptionManager : MonoBehaviour
{
    public static ExceptionManager instance { get; private set; }

    void Awake()
    {
        Application.logMessageReceived += HandleException;
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
    }

    void HandleException(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Exception)
        {
            //handle here
        }
    }
}
