using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// List of changes the player has made in editing the current store display
public class Changes : MonoBehaviour
{
    static Stack<StoreDisplayEdit> changes = new Stack<StoreDisplayEdit>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UndoChange()
    {

    }

    void ResetChanges()
    {

    }
}
