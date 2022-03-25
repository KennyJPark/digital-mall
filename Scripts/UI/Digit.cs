using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

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
    public int value;
    public int carry;
    public int index;

    public int startValue;

    bool selected;

    public TextMeshProUGUI digitText;
    public TMP_InputField digitField;

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
    }

    void Awake()
    {
        UpdateTextField();
        //digitField.text = value.ToString();
    }

    void UpdateTextField()
    {
        Debug.Log("Digit.UpdateTextField()");
        gameObject.GetComponent<TMP_Text>().text = value.ToString();
    }

    public int GetCarry()
    {
        return carry;
    }

    public int GetIndex()
    {
        return index;
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
        Debug.Log("Digit.SetValue()");
        if (val == -1)
        {
            gameObject.GetComponent<TMP_Text>().text = "";
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
}
