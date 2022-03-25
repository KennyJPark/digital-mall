using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DigitSelector : MonoBehaviour
{
    public delegate void IncrementAction();
    public static event IncrementAction OnIncrClick;

    public delegate void DecrementAction();
    public static event DecrementAction OnDecrClick;

    
    public GameObject selectedDigitObj;
    public Digit selectedDigit;

    [SerializeField]
    Button incArrow;
    [SerializeField]
    Button decArrow;

    void Awake()
    {


        if(selectedDigitObj == null)
        {
            selectedDigitObj = transform.parent.gameObject;
            selectedDigit = selectedDigitObj.GetComponent<Digit>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        incArrow.onClick.AddListener(IncrementClicked);
        decArrow.onClick.AddListener(DecrementClicked);
    }

    public void ChangeSelection(GameObject g)
    {
        transform.SetParent(g.transform);
        selectedDigitObj = g;
        selectedDigit = selectedDigitObj.GetComponent<Digit>();
    }

    public void ChangeSelection(Digit d)
    {
        transform.SetParent(d.gameObject.transform);
        selectedDigitObj = d.gameObject;
        selectedDigit = selectedDigitObj.GetComponent<Digit>();

    }

    public void IncrementClicked()
    {
        Debug.Log("IncrementDigit");
        if (OnIncrClick != null)
            OnIncrClick();
        
    }

    public void DecrementClicked()
    {
        Debug.Log("DecrementDigit");
        if (OnDecrClick != null)
            OnDecrClick();
    }
 
    public Digit GetSelectedDigit()
    {
        if(selectedDigitObj != null)
        {
            return selectedDigit;
        }
        return null;
    }

    public void ShiftSelectedDigit()
    {

    }

}
