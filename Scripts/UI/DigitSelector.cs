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

    }

    public void ChangeSelection(GameObject g)
    {
        Debug.Log("DigitSelector.ChangeSelection()");
        transform.SetParent(g.transform);
        selectedDigitObj = g;
        selectedDigit = selectedDigitObj.GetComponent<Digit>();
    }

    public void ChangeSelection(Digit d)
    {
        Debug.Log("DigitSelector.ChangeSelection()");
        transform.SetParent(d.gameObject.transform);
        selectedDigitObj = d.gameObject;
        selectedDigit = selectedDigitObj.GetComponent<Digit>();

    }

    public void IncrementClicked()
    {
        Debug.Log("IncrementClicked");
        if (OnIncrClick != null)
            OnIncrClick();
        
    }

    public void DecrementClicked()
    {
        Debug.Log("DecrementClicked");
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

    void OnEnable()
    {
        incArrow.onClick.AddListener(IncrementClicked);
        decArrow.onClick.AddListener(DecrementClicked);
    }

    void OnDisable()
    {
        incArrow.onClick.RemoveListener(IncrementClicked);
        decArrow.onClick.RemoveListener(DecrementClicked);
    }
}
