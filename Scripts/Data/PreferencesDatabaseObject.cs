using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// List of Preferences for categories such as Man, Woman, Child, etc.
[CreateAssetMenu(fileName = "New Preference Database Object", menuName = "Assets/Database/Preference Database Object")]
public class PreferencesDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public NPCPreference[] preferences;

    int preferenceDatabaseID;

    public Dictionary<int, NPCPreference> getPreference = new Dictionary<int, NPCPreference>();

    public void OnBeforeSerialize()
    {

    }

    public void OnAfterDeserialize()
    {
        getPreference = new Dictionary<int, NPCPreference>();
        ///*
        for (int i = 0; i < preferences.Length; ++i)
        {
            getPreference.Add(i, preferences[i]);
        }
        //*/
    }

    void OnEnable()
    {
        /*
        for (int i = 0; i < getPreference.Count; ++i)
        {
            Debug.Log(getPreference[i]);
        }
        */
    }
}
