using System.Collections.Generic;
using UnityEngine;

public abstract class RuntimeSetSO<T> : ScriptableObject
{
    [HideInInspector]
    public List<T> Items = new List<T>();
    public void Add(T thing)
    {
        if (!Items.Contains(thing))
            Items.Add(thing);
    }
    public void Remove(T thing)
    {
        if (Items.Contains(thing))
            Items.Remove(thing);
    }
}