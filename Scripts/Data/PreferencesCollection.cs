using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Preference Collection", menuName = "Assets/Database/Preference Collection")]
public class PreferencesCollection : ScriptableObject, ISerializationCallbackReceiver
{
    public PreferencesDatabaseObject[] allPreferences;

    public Dictionary<string, PreferencesDatabaseObject> preferencesCollectionByName = new Dictionary<string, PreferencesDatabaseObject>();
    public Dictionary<int, PreferencesDatabaseObject> preferencesCollectionByID = new Dictionary<int, PreferencesDatabaseObject>();

    public void OnBeforeSerialize()
    {

    }

    public void OnAfterDeserialize()
    {
        preferencesCollectionByName = new Dictionary<string, PreferencesDatabaseObject>();
        preferencesCollectionByID = new Dictionary<int, PreferencesDatabaseObject>();

        for (int i = 0; i < allPreferences.Length; ++i)
        {
            preferencesCollectionByID.Add(i, allPreferences[i]);
        }

    }
}
