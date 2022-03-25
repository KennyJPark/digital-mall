using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreDisplayCollection : MonoBehaviour
{
    // The entry with key = -1 is empty and represents the patron not wanting to buy anything
    public Dictionary<int, (StoreDisplay, ItemForSale)> storeCollection = new Dictionary<int, (StoreDisplay, ItemForSale)>();
    public int storeDisplayCount;
    public StoreDisplay emptyStoreDisplay;

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to "Confirm" button from StoreDisplayController
        StoreDisplayController.OnConfirm += CompileCollection;

        StoreDisplayController.OnUpdate += CompileCollection;
    }

    void Awake()
    {
        //Debug.Log("Collection AWAKE");
        storeDisplayCount = storeCollection.Count;
        /*
        if (!storeCollection.ContainsKey(-1))
        {
            storeCollection.Add(-1, (null, null));
        }
        */


    }

    // When should this method be called? Before opening store? When player updates a store display?
    // Subscribe to StoreDisplayController event when player updates a store display?
    public void CompileCollection()
    {
        //Debug.Log("Compiling Collection");
        storeCollection.Clear();
        storeDisplayCount = storeCollection.Count;
        //storeCollection.Add(-1, (null, null));
        // iterate through children
        // add the store display, and its child ItemForSale as entries for the dictionary
        for (int i = 0; i < this.transform.childCount; ++i)
        {
            for (int j = 0; j < this.transform.GetChild(i).childCount; ++j)
            {
                //Debug.Log("adding item from display #" + i);
                storeCollection.Add(i, (this.transform.GetChild(i).GetComponent<StoreDisplay>(), this.transform.GetChild(i).GetChild(j).GetComponent<ItemForSale>()));
            }
        }
        storeDisplayCount = storeCollection.Count;

        /*
        for (int i = 0; i < this.transform.childCount; ++i)
        {
            for(int j = 0; j < this.transform.GetChild(i).childCount; ++j)
            {
                storeCollection.Add(i, (this.transform.GetChild(i).GetComponent<StoreDisplay>(), this.transform.GetChild(i).GetChild(j).GetComponent<ItemForSale>()));
            }
        }
        */
        PrintCollection();
    }

    public void UpdateDisplay(StoreDisplay sd)
    {
        //Debug.Log("Updating Display");

        if (storeCollection.ContainsValue((sd, sd.itemForSale)))
        {
            for (int i = 0; i < storeCollection.Count; ++i)
            {
                if (storeCollection[i].Item2 == sd.itemForSale)
                {
                    // nothing changed
                }
                else
                {
                    storeCollection[i] = (sd, sd.itemForSale);
                }
            }
        }
        else
        {
            storeCollection.Add(storeCollection.Count, (sd, sd.itemForSale));
        }
        storeDisplayCount = storeCollection.Count;
    }

    public void PrintCollection()
    {
        Debug.Log("### Store Collection:");
        Debug.Log(storeCollection.Count + " items");
        foreach (var kvp in storeCollection)
        {
            if (kvp.Key != -1 && kvp.Value.Item2.itemID != -1)
            {
                Debug.Log("( Index: " + kvp.Key + "," + "Display#: " + kvp.Value.Item1.gameObject.name + ", ItemID: " + kvp.Value.Item2.itemID + ", ItemName: (" + kvp.Value.Item2.itemName + ") ))");
            }
            else
            {
                Debug.Log("(-1, (null, null))");
            }
        }
        Debug.Log("###");
    }

    public StoreDisplay GetDisplay(int index)
    {
        return storeCollection[index].Item1;
    }

    public ItemForSale GetItemForSale(int index)
    {
        return storeCollection[index].Item2;
    }


    void OnDisable()
    {
        // Unsubscribe to "Confirm" button from StoreDisplayController
        StoreDisplayController.OnConfirm -= CompileCollection;
        StoreDisplayController.OnUpdate -= CompileCollection;
    }
}
