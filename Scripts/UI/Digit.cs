using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class Digit : MonoBehaviour, IPointerDownHandler
{
    public enum DigitType{
        one,
        ten,
        hundred,
        thousand,
        t_Thousand,
        h_Thousand,
        million
    }

    public delegate void DigitSelectAction(Digit d);
    public static event DigitSelectAction OnDigitClick;

    //public delegate void DigitChangeAction(Digit d, bool increment_or_decrement);
    public delegate void DigitChangeAction(Digit d, bool increment_or_decrement, int carry);
    public static event DigitChangeAction OnDigitChange;

    public DigitType digitType;


    //[SerializeField]
    // Set these to serializefield?
    [SerializeField]
    int value;
    [SerializeField]
    int carry;
    [SerializeField]
    int index;

    public int startValue;

    bool selected;
    bool isSet;

    public TextMeshProUGUI digitText;
    //public TMP_InputField digitField;

    // When digit is clicked
    public void OnPointerDown(PointerEventData eventData)
    {
        if(OnDigitClick != null)
            OnDigitClick(this);
    }

    // Increment(true) or Decrement(false) a digit
    // Returns a carry of either 1 or -1 to be used by DigitField
    public int UpdateDigit(bool increment)
    {
        if(increment)
        {
            if(value < 9)
            {
                value++;
                carry = 0;
            }
            else if(value == 9)
            {
                value = 0;
                carry = 1;
            }
            if(OnDigitChange != null)
                OnDigitChange(this, true, carry);
        }
        else if(!increment)
        {
            if(value > 0)
            {
                value--;
                carry = 0;
            }
            else if(value == 0)
            {
                value = 9;
                carry = -1;
            }
            if (OnDigitChange != null)
                OnDigitChange(this, false, carry);
        }
        UpdateTextField();
        //digitField.text = value.ToString();
        return carry;
    }

    public int AddCarry(int carry)
    {
        if(carry == 1)
        {
            if (value < 9)
            {
                value++;
                carry = 0;
            }
            else if (value == 9)
            {
                value = 0;
                carry = 1;
            }
        }
        else if(carry == -1)
        {
            if (value > 0)
            {
                value--;
                carry = 0;
            }
            else if (value == 0)
            {
                value = 9;
                carry = -1;
            }
        }

        UpdateTextField();
        return carry;
    }

    // Start is called before the first frame update
    void Start()
    {
        carry = 0;
        //UpdateTextField();
    }

    void Awake()
    {
        SetUpDigit();
        
        //digitField.text = value.ToString();
    }

    public void ClearTextField()
    {
        Debug.Log("Digit.ClearTextField()");
        //digitText.text = "";
        gameObject.GetComponent<TMP_Text>().text = "";
        isSet = false;

    }

    public int GetCarry()
    {
        return carry;
    }

    public int GetIndex()
    {
        return index;
    }

    public bool IsSet()
    {
        return isSet;
    }

    public int GetTrueValue()
    {
        return value * ((int)Math.Pow(10, index));
    }

    public int GetImplicitValue()
    {
        return (int)Math.Pow(10, index);
    }

    public int GetValue()
    {
        return value;
    }

    public void SetCarry(int c)
    {
        carry = c;
    }

    public void SetValue(int val)
    {
        // val can't be greater than 9
        if(val > 9)
        {
            return;
        }

        Debug.Log("Digit: " + index + " .SetValue() to " + val);
        if (val == -1)
        {
            value = 0;
            digitText.text = "";
            //gameObject.GetComponent<TMP_Text>().text = "";
            isSet = false;
        }
        else
        {
            value = val;
            UpdateTextField();
        }

    }


    public void SetIndex(int i)
    {
        index = i;
        if(i == 0)
        {
            digitType = DigitType.one;
        }
        else if(i == 1)
        {
            digitType = DigitType.ten;
        }
        else if (i == 2)
        {
            digitType = DigitType.hundred;
        }
        else if (i == 3)
        {
            digitType = DigitType.thousand;
        }
        else if (i == 4)
        {
            digitType = DigitType.t_Thousand;
        }
        else if (i == 5)
        {
            digitType = DigitType.h_Thousand;
        }
        else if (i == 6)
        {
            digitType = DigitType.million;
        }

    }

    void SetUpDigit()
    {
        if (digitText == null)
        {
            Debug.Log("DIGIT INIT");
            digitText = gameObject.GetComponent<TextMeshProUGUI>();
        }
    }

    void UpdateTextField()
    {
        Debug.Log("Digit.UpdateTextField()");
        digitText.text = value.ToString();
        //gameObject.GetComponent<TMP_Text>().text = value.ToString();
        isSet = true;
    }

}
