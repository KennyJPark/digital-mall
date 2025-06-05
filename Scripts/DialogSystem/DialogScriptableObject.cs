using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Dialog Object", menuName = "Dialog System")]
public class DialogScriptableObject : ScriptableObject
{
    public event Action DialogComplete;

    public List<string> dialogText;

    public bool isLoopable;

    public bool isOrdered;

    string _dialogID;
    bool _hasCompleted;
    bool _readHistory;

    //public DialogScriptableObject nextDialog;
}

/*
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // used for the _readHistory.All(x => true) so we can check an array for all values


[CreateAssetMenu(fileName = "Dialog Object", menuName = "Dialog System")]
public class DialogScriptableObject : ScriptableObject
{
    public event System.Action DialogComplete;

    public List<string> DialogText;
    public bool IsLoopable;
    public bool IsOrdered;
    public DialogScriptableObject NextDialog; // will be added in Part 2

    protected string _dialogID;     // used to have a unique id for each dialog object, expand system to enforce uniqueness 
    protected bool _hasCompleted;
    protected Queue<string> _ordededDialog;
    protected bool[] _readHistory;  // will be added in Part 2
    

    public virtual string GetNextDialog(){


        if (IsOrdered)
        {
            if(_ordededDialog.Count > 0)
            {
                return _ordededDialog.Dequeue();
            }
            else
            {
                if (IsLoopable)
                {
                    BuildTextQueue();
                    return _ordededDialog.Dequeue();
                } else
                {
                    OnDialogComplete();
                }
            }
        }

        // Return Random Index if not orderable
        int randomIndex = Random.Range(0, DialogText.Count);
        return DialogText[randomIndex];

        
    }

    protected virtual void OnDialogComplete()
    {
        // can save dialog progress here using playerprefs or some other option
        DialogComplete?.Invoke();
    }


    private void OnEnable()
    {
        if (IsOrdered)
        {
            BuildTextQueue();
        }
    }

    private void BuildTextQueue()
    {
        _ordededDialog = new Queue<string>();
        for (int i = 0; i < DialogText.Count; i++)
        {
            _ordededDialog.Enqueue(DialogText[i]);
        }
    }

}

 */
