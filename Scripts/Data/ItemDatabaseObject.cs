using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item Database", menuName = "Assets/Database/Item Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    //public static ItemDatabaseObject instance { get; private set; }

    public ItemObject[] allItems;
    public Dictionary<ItemObject, int> getId = new Dictionary<ItemObject, int>();
    public Dictionary<int, ItemObject> getItem = new Dictionary<int, ItemObject>();

    public void OnBeforeSerialize()
    {

        //Debug.Log("OnBeforeSerialize");
    }

    public void OnAfterDeserialize()
    {
        getId = new Dictionary<ItemObject, int>();
        getItem = new Dictionary<int, ItemObject>();

        for (int i = 0; i < allItems.Length; ++i)
        {
            getId.Add(allItems[i], i);
            getItem.Add(i, allItems[i]);
        }
        //Debug.Log("OnAfterDeserialize");
    }

    void Awake()
    {
        //instance = this;
    }




}
