using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogController : MonoBehaviour, IPointerClickHandler
{

    public void OnPointerClick(PointerEventData eventData)
    {

    }
}

/*
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // needed for IPointerClickHandler when using the New Input System

public class DialogController : MonoBehaviour, IPointerClickHandler
{
    public DialogUI DialogUI;
    public DialogScriptableObject CurrentDialogObject;
    
    private void OnEnable(){
        DialogUI.NextButton.onClick.AddListener(OnInteract);
    }

    private void OnDisable(){
        DialogUI.NextButton.onClick.RemoveListener(OnInteract);
    }

    public void OnInteract(){
        Debug.Log("Interact!");
        DialogUI.ShowText(CurrentDialogObject.GetNextDialog(), true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
            OnInteract();
    }
}
 * */