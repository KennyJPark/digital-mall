using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class DigitField : MonoBehaviour
{
    // Used by Percent.cs
    public delegate void PriceChangeAction(int currentVal, int baseVal);
    public static event PriceChangeAction OnPriceChange;

    public GameObject itemForSaleObj;
    public ItemForSale itemForSale;

    [SerializeField]
    int basePrice;
    int currentValue;

    [SerializeField]
    GameObject millions;
    [SerializeField]
    GameObject hundredThousands;
    [SerializeField]
    GameObject tenThousands;
    [SerializeField]
    GameObject thousands;
    [SerializeField]
    GameObject hundreds;
    [SerializeField]
    GameObject tens;
    [SerializeField]
    GameObject ones;

    int MAX_VALUE = 9999999;

    [SerializeField]
    //GameObject[] digitsObj;
    List<GameObject> digitsObj = new List<GameObject>();

    //Digit[] digits;
    [SerializeField]
    List<Digit> digits = new List<Digit>();

    [SerializeField]
    bool digitFieldInitialized = false;

    [SerializeField]
    GameObject digitSelectorObj;

    DigitSelector digitSelector;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("DigitField.Start()");



        // Normal
        //baseValue = itemForSale.baseValue;
        // currentValue = baseValue;

        // test
        
        /*
        Debug.Log("TEST DigitField");
        basePrice = 500;
        currentValue = basePrice;
        Debug.Log("total value: " + basePrice);
        FillDigitFields();
        
        if (OnPriceChange != null)
            OnPriceChange(currentValue, basePrice);
        */
        
    }

    void Awake()
    {
        Debug.Log("DigitField.Awake()");

        if (itemForSale != null)
        {
            basePrice = itemForSale.basePrice;
        }


        ones.GetComponent<Digit>().SetValue(1);
        ones.GetComponent<Digit>().SetIndex(0);

        tens.GetComponent<Digit>().SetValue(-1);
        tens.GetComponent<Digit>().SetIndex(1);

        hundreds.GetComponent<Digit>().SetValue(-1);
        hundreds.GetComponent<Digit>().SetIndex(2);

        thousands.GetComponent<Digit>().SetValue(-1);
        thousands.GetComponent<Digit>().SetIndex(3);

        tenThousands.GetComponent<Digit>().SetValue(-1);
        tenThousands.GetComponent<Digit>().SetIndex(4);

        hundredThousands.GetComponent<Digit>().SetValue(-1);
        hundredThousands.GetComponent<Digit>().SetIndex(5);

        millions.GetComponent<Digit>().SetValue(-1);
        millions.GetComponent<Digit>().SetIndex(6);


    }

    void Update()
    {

    }


    void CallIncrement()
    {
        Debug.Log("CallIncrement");
        IncrementDigit(digitSelector.selectedDigit);
    }

    void CallDecrement()
    {
        Debug.Log("CallDecrement");
        DecrementDigit(digitSelector.selectedDigit);
    }



    /*
    int[] vals = { 1, -2, 3, 4, 0, -3, 2, 1, 3 };

    var v1 = Array.FindIndex(vals, x => x == 3);
    Console.WriteLine(v1);

    */


    // Decrement the selected digit by 1
    // If the current value is 0, set the digit to 9, and subtract a 1 to the digit on the left
    // This method should be written to be used recursively
    // Param(Digit d) or (GameObject digits[])?

    //void DecrementDigit()
    void DecrementDigit(Digit d)
    {
        //Debug.Log("DigitField.DecrementDigit()");
        // Is the digit the last digit?
        //if (digitSelector.selectedDigit == digits[digits.Length - 1])
        if (d == digits[digits.Count - 1])
        {
            Debug.Log("LAST DIGIT");
            //int carry = digitSelector.selectedDigit.UpdateDigit(true);
            int carry = d.UpdateDigit(false);
            int digitIndex = digits.FindIndex(0, digits.Count, x => x == d);
            if (carry < 0)
            {
                    DecrementDigit(digits[digitIndex - 1]);
            }
        }
        // Is the digit the first digit?
        else if (d == digits[0])
        {
            Debug.Log("FIRST DIGIT");
            if (d.value == 1 && basePrice == 1)
            {
                return;
            }
            else if(d.value == 0 && currentValue == 0)
            {
                d.value = 1;
                return;
            }
            else
            {                
                int carry = d.UpdateDigit(false);
                int digitIndex = digits.FindIndex(0, digits.Count, x => x == d);

                ///*
                while (carry < 0)
                {
                    carry = digits[++digitIndex].AddCarry(carry);
                    //digitIndex = digits.FindIndex(0, digits.Count, x => x == d);
                }
                //*/
                /*
                if (carry < 0)
                {
                    DecrementDigit(digits[digitIndex + 1]);
                }
                */
            }
        }
        // d is a Middle digit
        else
        {
            Debug.Log("MIDDLE DIGIT: " + d.GetValue());
            if(d.GetValue() == 0)
            {

            }
            else if(d.GetValue() == 1)
            {
                if (((int)Math.Pow(10, d.GetIndex())) > currentValue)
                {
                    Debug.Log("TARGET");
                }
            }
            else
            {
                int carry = d.UpdateDigit(false);
                int digitIndex = digits.FindIndex(0, digits.Count, x => x == d);
                while (carry < 0)
                {
                    carry = digits[++digitIndex].AddCarry(carry);
                    //digitIndex = digits.FindIndex(0, digits.Count, x => x == d);
                }
                /*
                if (carry < 0)
                {
                    DecrementDigit(digits[digitIndex + 1]);
                    //DecrementDigit(digits[digitIndex - 1]);
                }
                */
            }

        }
    }

    // if the recently incremented digit has blank values in between it and the lowest digit..
    // fill the digits in between with 0s
    void FillDigitFields()
    {
        Debug.Log("START DigitField.FillDigitFields()");
        Debug.Log("basePrice:" + basePrice);
        int temp = basePrice;
        int count = 0;

        // Why zero?
        Debug.Log(digits.Count);

        Debug.Log("TEMP: " + temp);
        while(temp > 0)
        {
            Debug.Log("WHILE");
            digits[count].SetValue(temp % 10);
            Debug.Log("HERE");
            Debug.Log(temp % 10);
            Debug.Log("THERE");
            temp /= 10;
            digitSelector.ChangeSelection(digits[count]);
            count++;
            
        }
        Debug.Log("END WHILE");
        if (OnPriceChange != null)
            OnPriceChange(currentValue, basePrice);

        Debug.Log("END DigitField.FillDigitFields()");
    }


    // Increment the selected digit by 1
    // If the current value is 9, set the digit to 0, and add a 1 to the digit on the left
    // If the current digit is the leftmost digit, do nothing
    // This method should be written to be used recursively
    // Param(Digit d) or (GameObject digits[])?
    void IncrementDigit(Digit d)
    //void IncrementDigit()
    {

        if (currentValue >= MAX_VALUE)
        {
            return;
        }
        // Is the digit the last digit?
        //if (digitSelector.selectedDigit == digits[digits.Length - 1])
        if (d == digits[digits.Count - 1])
        {
            // ..and is the value already '9'? do nothing
            //if(digitSelector.selectedDigit.value == 9)
            if (d.value == 9)
            {
                return;
            }
            // 
            else
            {
                //int carry = digitSelector.selectedDigit.UpdateDigit(true);
                int carry = d.UpdateDigit(true);
                //currentValue = d.
                int digitIndex = digits.FindIndex(0, digits.Count, x => x == d);
                if (carry > 0)
                {
                    IncrementDigit(digits[digitIndex - 1]);
                }

            }
        }
        // Is the digit the first digit?
        else if (d == digits[0])
        {
            int carry = d.UpdateDigit(true);
            int digitIndex = digits.FindIndex(0, digits.Count, x => x == d);

            while (carry > 0)
            {
                //Debug.Log("while");
                //carry = digits[++digitIndex].UpdateDigit(true);
                carry = digits[++digitIndex].AddCarry(carry);
                //digitIndex = digits.FindIndex(0, digits.Count, x => x == d);
            }
            /*
            if (carry > 0)
            {
                IncrementDigit(digits[digitIndex + 1]);
                
            }
            */
        }
        // d is a Middle digit
        else
        {
            int carry = d.UpdateDigit(true);
            int digitIndex = digits.FindIndex(0, digits.Count, x => x == d);
            while (carry > 0)
            {
                carry = digits[++digitIndex].AddCarry(carry);
            }
            /*
            if (carry > 0)
            {
                IncrementDigit(digits[digitIndex + 1]);
            }
            */
        }

    }

    void InitDigitField()
    {

        if (digitSelector == null)
        {
            // Find and assign digitSelector
        }
        //init digitsObj[]
        if (digitsObj == null || digitsObj.Count == 0)
        {
            digitsObj.Add(ones);
            digitsObj.Add(tens);
            digitsObj.Add(hundreds);
            digitsObj.Add(thousands);
            digitsObj.Add(tenThousands);
            digitsObj.Add(hundredThousands);
            digitsObj.Add(millions);

            /*
            Debug.Log(digitsObj[0]);
            Debug.Log(digitsObj[1]);
            Debug.Log(digitsObj[2]);
            Debug.Log(digitsObj[3]);
            Debug.Log(digitsObj[4]);
            Debug.Log(digitsObj[5]);
            Debug.Log(digitsObj[6]);
            */
        }
        //Debug.Log("digitsObj.Count: " + digitsObj.Count);

        // init digits[]
        if (digits == null || digits.Count == 0)
        {
            for (int i = 0; i < digitsObj.Count; ++i)
            {
                digits.Add(digitsObj[i].GetComponent<Digit>());
                //Debug.Log("i:" + i + " - " + digits[i].value);
            }

            //Debug.Log("digits.count: " + digits.Count);
            
        }
        digitFieldInitialized = true;
        for (int i = 0; i < digits.Count; ++i)
        {
            //Debug.Log("i:" + i);
            //Debug.Log(digits[i]);
            digits[i].index = i;

            digits[i].startValue = (int)Math.Pow(10, i);
        }

        digitSelector = digitSelectorObj.GetComponent<DigitSelector>();

    }

    // Set all digits to 0
    void ResetDigitField()
    {

    }

    void SelectDigit(Digit d)
    {
        Debug.Log("SelectDigit");
        Debug.Log(d.digitType);

        digitSelector.ChangeSelection(d.gameObject);

        //SelectDigit();
    }

    // Take base price of item and fill digit fields with correct values
    public void SetPrice(int price)
    {
        Debug.Log("START DigitField.SetPrice()");
        // convert baseprice to digitfields
        basePrice = price;
        currentValue = basePrice;
        Debug.Log("base price is now: " + basePrice);
        Debug.Log("END DigitField.SetPrice()");
    }

    public void SetUpDigitField(GameObject ifsObj, ItemForSale ifs)
    {
        Debug.Log("START DigitField.SetUpDigitField()");
        if(itemForSaleObj == null || itemForSaleObj != ifsObj)
        {
            itemForSaleObj = ifsObj;
        }
        if (itemForSale == null || itemForSale != ifs)
        {
            itemForSale = ifs;
        }
        SetPrice(ifs.basePrice);
        FillDigitFields();
        Debug.Log("END DigitField.SetUpDigitField()");
    }

    //void UpdateValue(Digit d, bool increment_or_decrement)
    void UpdateValue(Digit d, bool increment_or_decrement, int carry)
    {
        if (increment_or_decrement)
        {
            currentValue += (int)Math.Pow(10, d.index);
        }
        else
        {
            currentValue -= (int)Math.Pow(10, d.index);
        }

        Debug.Log("(UpdateValue) After Current Value: " + currentValue);
        if (OnPriceChange != null)
            OnPriceChange(currentValue, basePrice);
    }

    void OnEnable()
    {
        if (!digitFieldInitialized)
        {
            InitDigitField();
        }
        DigitSelector.OnIncrClick += CallIncrement;
        DigitSelector.OnDecrClick += CallDecrement;

        Digit.OnDigitClick += SelectDigit;
        Digit.OnDigitChange += UpdateValue;
    }

    void OnDisable()
    {
        DigitSelector.OnIncrClick -= CallIncrement;
        DigitSelector.OnDecrClick -= CallDecrement;
        Digit.OnDigitClick -= SelectDigit;
        Digit.OnDigitChange -= UpdateValue;
    }
}
