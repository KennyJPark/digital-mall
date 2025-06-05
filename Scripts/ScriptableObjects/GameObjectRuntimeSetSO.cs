using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "GameObject Runtime Set", fileName
= "GORuntimeSet")]
public class GameObjectRuntimeSetSO : ScriptableObject
{
    private List<GameObject> items = new List<GameObject>();
    public List<GameObject> Items => items;
    public void Add(GameObject thingToAdd)
    {
        if (!items.Contains(thingToAdd))
            items.Add(thingToAdd);
    }
    public void Remove(GameObject thingToRemove)
    {
        if (items.Contains(thingToRemove))
            items.Remove(thingToRemove);
    }
}