using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustScript : MonoBehaviour
{
    private void OnGUI()
    {
        if(GUI.Button(new Rect(10, 100, 100, 30), "Save"))
        {
            Mall.Instance.Save();
            Debug.Log("Saved...");
        }
        if(GUI.Button(new Rect(10, 140, 100, 30), "Load"))
        {
            Mall.Instance.Load();
            Debug.Log("Loaded...");
        }
    }
}
