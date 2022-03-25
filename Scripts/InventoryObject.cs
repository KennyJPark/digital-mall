using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

// Scriptable Object for Inventory objects
// Used to make various inventories; player inventory, demo inventory, merchant inventory

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject, ISerializationCallbackReceiver
{
    private ItemDatabaseObject database;

    public string savePath;
    //public string loadPath;
    public Inventory inventory;

    string databaseName = "Demo Database";
    
    
    public void AddItem(ItemObject _item, int _quantity)
    {
        for(int i = 0; i < inventory.Items.Count; ++i)
        {
            if(inventory.Items[i].item == _item)
            {
                inventory.Items[i].AddAmount(_quantity);
                return;
            }
        }
        inventory.Items.Add(new InventorySlot(database.getId[_item], _item, _quantity));
    }
    

    private void OnEnable()
    {
#if UNITY_EDITOR
        database = (ItemDatabaseObject)AssetDatabase.LoadAssetAtPath("Assets/Scripts/ScriptableObjects/ItemDatabase.asset", typeof(ItemDatabaseObject));
#else
        database = Resources.Load<ItemDatabaseObject>("Demo Database");
#endif
    }

    public void OnAfterDeserialize()
    {
        //Debug.Log("After Deserialize " + database);
        for (int i = 0; i < inventory.Items.Count; ++i)
        {
            if(database != null && database.getItem.ContainsKey(inventory.Items[i].itemID))
            {
                inventory.Items[i].item = database.getItem[inventory.Items[i].itemID];
            }

        }
    }

    public void OnBeforeSerialize()
    {

        //Debug.Log("Before Serialize " + database);
    }

    public void Save()
    {
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);
        file.Close();
    }

    public void Load()
    {
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
        }
    }
}

[System.Serializable]
public class Inventory
{
    public List<InventorySlot> Items = new List<InventorySlot>();
}


[System.Serializable]
public class InventorySlot
{
    public int itemID;
    public ItemObject item;
    public int quantity;
    public InventorySlot(int _itemID, ItemObject _item, int _quantity)
    {
        itemID = _itemID;
        item = _item;
        quantity = _quantity;
    }

    public void AddAmount(int value)
    {
        quantity += value;
    }
}