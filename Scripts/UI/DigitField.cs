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

    bool isTestScene;

    [SerializeField]
    int basePrice;
    [SerializeField]
    int currentPrice;
    [SerializeField]
    int suggestedPrice;
    [SerializeField]
    decimal suggestedMultiplier;

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

    const int MAX_VALUE = 9999999;
    int firstDigitIndex;
    int lastDigitIndex;

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
        Debug.Log("START DigitField.Start()");



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
        Debug.Log("AWAKE DigitField.Awake()");

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

        firstDigitIndex = 0;
        lastDigitIndex = 6;

    }

    void TestField()
    {

        Debug.Log("DigitField.TestField()");
        int temp = currentPrice;
        suggestedMultiplier = 1.3M;
        int remainder;
        int count = 0;
        //for(int i = 0; i < digits.Count; ++i)
        foreach(var d in digits)
        {
            //Debug.Log("For");
            remainder = temp % ((int)Math.Pow(10, d.GetIndex()));
            temp /= 10;
            //Debug.Log("Remainder: " + remainder);
            //Debug.Log("temp: " + temp);
            d.SetValue(remainder);
        }
        Debug.Log("### PROBLEM ###");

        if (OnPriceChange != null)
            OnPriceChange(currentPrice, basePrice);
        CollapseField();
        digits[3].SetValue(0);
    }

    void ComputeSuggestedPrice()
    {
        
        suggestedPrice = (int)(basePrice * suggestedMultiplier);
        Debug.Log(basePrice + " * " + suggestedMultiplier);
        Debug.Log("SuggestedPrice: " + suggestedPrice);
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

    public int GetCurrentPrice()
    {
        return currentPrice;
    }

    int GetDigitValue(Digit d)
    {
        
        return ((int)Math.Pow(10, d.GetIndex()));
    }

    // If the currentPrice goes to 0, set it to 1
    void ClampField()
    {
        if(currentPrice == 0)
        {
            IncrementDigit(digits[0]);
            digitSelector.ChangeSelection(digits[0]);
        }
    }

    // Clear fields upon closing store
    void ClearFields()
    {
        if (isTestScene)
        {
            basePrice = 500;
            currentPrice = 500;
            suggestedMultiplier = 1.3M;
            ComputeSuggestedPrice();
            TestField();
        }
        else
        {
            basePrice = 0;
            currentPrice = 0;
        }

    }

    // Remove Unecessary zeros
    // Example: when going from 1000 -> 099, remove the leading 0 past the first
    void CollapseField()
    {
        Debug.Log("DigitField.CollapseField()");
        foreach (Digit d in digits)
        {
            if (GetDigitValue(d) > currentPrice && d.GetValue() == 0)
            {
                d.ClearTextField();
            }
            else
            {
                if(GetDigitValue(d) <= currentPrice)
                {
                    digitSelector.ChangeSelection(d);
                }
            }
        }
        
    }

    ///*
    //void DecrementDigit()
    void DecrementDigit(Digit d)
    {
        Debug.Log("DigitField.DecrementDigit()");
        Debug.Log("Decrementing Digit: " + d.GetIndex());
        if(currentPrice == 1)
        {
            return;
        }
        if(currentPrice == 0)
        {
            IncrementDigit(digits[0]);
            return;
        }

        // Is the digit the first digit?
        if (d.GetIndex() == firstDigitIndex)
        {
            Debug.Log("FIRST DIGIT (Rightmost Digit)");
            if (d.GetValue() == 1 && basePrice == 1)
            {
                return;
            }
            else if(d.GetValue() == 0 && currentPrice > 0)
            {
                Debug.Log("Dabu");

                int carry = d.UpdateDigit(false);
                int digitIndex = d.GetIndex();

                while (carry < 0)
                {
                    carry = digits[++digitIndex].AddCarry(carry);
                    //digitIndex = digits.FindIndex(0, digits.Count, x => x == d);
                }
                CollapseField();
            }
            else if(d.GetValue() == 1 && currentPrice == 1)
            {
                //Debug.Log("Swobu");
                return;
            }
            else
            {                
                int carry = d.UpdateDigit(false);
                int digitIndex = d.GetIndex();

                ///*
                while (carry < 0)
                {
                    carry = digits[++digitIndex].AddCarry(carry);
                }
            }
        }
        // Is the digit the last digit?
        else if (d.GetIndex() == lastDigitIndex)
        {
            //Debug.Log("LAST DIGIT (Leftmost Digit)");
            //int carry = digitSelector.selectedDigit.UpdateDigit(true);

            if(d.GetValue() == 0)
            {
                return;
            }
            int carry = d.UpdateDigit(false);
            int digitIndex = digits.FindIndex(0, digits.Count, x => x == d);
            if (carry < 0)
            {
                DecrementDigit(digits[digitIndex - 1]);
            }
        }

        // d is a Middle digit
        else
        {
            Debug.Log("MIDDLE DIGIT: " + d.GetValue());
            if(d.GetValue() == 0)
            {
                // NEEDS FIXING
                //Debug.Log("d.Value == 0");

                if(d.GetImplicitValue() > currentPrice)
                {
                    Debug.Log("d.BaseValue > currentPrice");
                    // is digit the most significant digit?
                    if (FindMostSignificantDigit() != d)
                    {
                        CollapseField();
                    }
                    else
                    {
                        return;
                    }  
                }
                else
                {
                    //Debug.Log("NEEDS FIXING");
                    int carry = d.UpdateDigit(false);
                    int digitIndex = d.GetIndex();
                    //Debug.Log("carry: " + carry);
                    while (carry < 0)
                    {
                        carry = digits[++digitIndex].AddCarry(carry);
                    }
                }
            }
            else if(d.GetValue() == 1)
            {

                // if ImplicitValue > currentPrice
                if (((int)Math.Pow(10, d.GetIndex())) > currentPrice)
                {

                }
                // // if ImplicitValue == currentPrice
                else if (((int)Math.Pow(10, d.GetIndex())) == currentPrice)
                {
                    //Debug.Log("TARGET");
                    int carry = d.UpdateDigit(false);
                    //int digitIndex = digits.FindIndex(0, digits.Count, x => x == d);
                    int digitIndex = d.GetIndex();
                    while (carry < 0)
                    {
                        carry = digits[++digitIndex].AddCarry(carry);
                    }
                    DecrementDigit(d);
                }
                // d is the most significant digit
                else if (d == FindMostSignificantDigit())
                {
                    if (d.GetValue() == 1)
                    {
                        Debug.Log("TARGET");
                        int carry = d.UpdateDigit(false);
                        int digitIndex = d.GetIndex();
                        while (carry < 0)
                        {
                            carry = digits[++digitIndex].AddCarry(carry);
                        }
                        CollapseField();
                    }
                }
                else
                {
                    //Debug.Log("else");
                    int carry = d.UpdateDigit(false);
                    int digitIndex = d.GetIndex();
                    while (carry < 0)
                    {
                        carry = digits[++digitIndex].AddCarry(carry);
                    }
                    //ClampField();
                    //CollapseField();
                    //Debug.Log("TARGET");
                }
            }
            else
            {
                //Debug.Log("else");
                int carry = d.UpdateDigit(false);
                int digitIndex = d.GetIndex();
                while (carry < 0)
                {
                    carry = digits[++digitIndex].AddCarry(carry);
                    //digitIndex = digits.FindIndex(0, digits.Count, x => x == d);
                }
            }
        }

    }
    //*/
    // Increment the selected digit by 1
    // If the current value is 9, set the digit to 0, and add a 1 to the digit on the left
    // If the current digit is the leftmost digit, do nothing
    // This method should be written to be used recursively
    // Param(Digit d) or (GameObject digits[])?

    void IncrementDigit(Digit d)
    //void IncrementDigit()
    {
        //Debug.Log("DigitField.IncrementDigit() START");
        if (currentPrice >= MAX_VALUE)
        {
            return;
        }


        // Is the digit the last digit?
        //if (digitSelector.selectedDigit == digits[digits.Length - 1])
        if (d == digits[digits.Count - 1])
        {
            // ..and is the value already '9'? do nothing
            //if(digitSelector.selectedDigit.value == 9)
            if (d.GetValue() == 9)
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

        }
        // d is a Middle digit
        else
        {
            //Debug.Log("IncrementDigit -> else");
            int carry = d.UpdateDigit(true);
            //int digitIndex = digits.FindIndex(0, digits.Count, x => x == d);
            int digitIndex = d.GetIndex();
            while (carry > 0)
            {
                carry = digits[++digitIndex].AddCarry(carry);
            }

        }

        //if (d.GetImplicitValue() > currentPrice)
       // {
            //Debug.Log("THE TARGET");
            FillEmptyDigits();
       // }

        // read comments above declaration
        if (GetGreatestDigit() == d)
        {

            SelectGreatestDigit(d);
        }
        //FillDigitFields();
        Debug.Log("DigitField.IncrementDigit() END");
    }


    // Digit Field should have 1 leading zero by default
    void FillDigitFields()
    {
        Debug.Log("START DigitField.FillDigitFields()");
        Debug.Log("currentValue:" + currentPrice);
        int temp = currentPrice;
        int count = 0;


        while (temp > 0)
        {
            //Debug.Log("WHILE");
            digits[count].SetValue(temp % 10);
            //Debug.Log("HERE");
            //Debug.Log(temp % 10);
            //Debug.Log("THERE");
            temp /= 10;
            //digitSelector.ChangeSelection(digits[count]);
            count++;
        }
        //digits[count].SetValue(0);
        if (OnPriceChange != null)
            OnPriceChange(currentPrice, basePrice);

        Debug.Log("END DigitField.FillDigitFields()");
    }

    // if the recently incremented digit has blank values in between it and the lowest digit..
    // fill the digits in between with 0s
    void FillEmptyDigits()
    {
        Debug.Log("FillEmptyDigits");
        int index = FindMostSignificantDigit().GetIndex();
        
        // ALL TRUE
        for (int i = 0; i < digits.Count; ++i)
        {
            Debug.Log(i + " ; " + digits[i].IsSet());
        }
        ///*
        for (int i = index; i > 0; --i)
        {
            if (!digits[i].IsSet())
            {
                Debug.Log("ffffffff");
                digits[i].SetValue(0);
            }
        }
        //*/
    }

    void InitDigitField()
    {
        Debug.Log("DigitField.InitDigitField()");
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

        // init digits[]
        if (digits == null || digits.Count == 0)
        {
            for (int i = 0; i < digitsObj.Count; ++i)
            {
                digits.Add(digitsObj[i].GetComponent<Digit>());
                //Debug.Log("i:" + i + " - " + digits[i].value);
            }
            
        }
        digitFieldInitialized = true;
        for (int i = 0; i < digits.Count; ++i)
        {
            //Debug.Log("i:" + i);
            //Debug.Log(digits[i]);
            digits[i].SetIndex(i);

            digits[i].startValue = (int)Math.Pow(10, i);
        }

        digitSelector = digitSelectorObj.GetComponent<DigitSelector>();

    }

    // Starting from the left
    // Return first Digit with value > 0
    Digit FindMostSignificantDigit()
    {
        Debug.Log("FindMostSignificantDigit");
        for (int i = digits.Count-1; i > 0; --i)
        {
            if (digits[i].GetValue() > 0)
            {
                //Debug.Log(digits[i].GetIndex());
                return digits[i];
            }
        }
        //Debug.Log(digits[0].GetIndex());
        return digits[0];
    }

    Digit GetGreatestDigit()
    {
        for (int i = digits.Count - 1; i >= 0; --i)
        {
            if (digits[i].GetValue() >= currentPrice)
            {
                return digits[i];
            }
        }
        return null;
    }

    void SelectMostSignificantDigit()
    {
        Debug.Log("dd");
        if (currentPrice < 1)
        {
            return;
        }
        for (int i = digits.Count-1; i > 0; --i)
        {
            if (digits[i].GetValue() > 0)
            {
                digitSelector.ChangeSelection(digits[i]);
                return;
            }
        }
    }
    // Set all digits to base price
    public void ResetDigitField()
    {
        foreach(Digit d in digits)
        {
            d.SetValue(-1);
            //d.ClearTextField();
        }
        
        // incomplete
        currentPrice = basePrice;
        int temp = basePrice;
        int count = 0;
        while (temp > 0)
        {
            Debug.Log("COUNT: " + count + "; TEMP: " + temp);
            if(digits[count].GetImplicitValue() > basePrice)
            {
                Debug.Log("TARGET; BASE VALUE: " + digits[count].GetImplicitValue());

                digits[count].SetValue(-1);
            }
            else
            {
                Debug.Log("ELSE; BASE VALUE: " + digits[count].GetImplicitValue());
                digits[count].SetValue(temp % 10);
            }

            temp /= 10;
            count++;
        }
    }

    void SelectDigit(Digit d)
    {
        Debug.Log("SelectDigit");
        Debug.Log(d.digitType);

        digitSelector.ChangeSelection(d.gameObject);

        //SelectDigit();
    }

    public int GetBasePrice()
    {
        return basePrice;
    }


    // Should be called when incrementing a digit that IS the greatest digit
    // Should not be called when incrementing a digit that is not currently the greatest
    void SelectGreatestDigit(Digit d)
    {
        Debug.Log("DigitField.SelectGreatestDigit()");
        //if()

        for (int i = 0; i < digits.Count; ++i)
        {
            if (digits[i].GetValue() > 0)
            {
                digitSelector.ChangeSelection(digits[i]);
            }
        }
    }

    // Take base price of item and fill digit fields with correct values
    public void SetPrice(int price)
    {
        Debug.Log("START DigitField.SetPrice()");
        // convert baseprice to digitfields
        basePrice = price;
        currentPrice = basePrice;
        ResetDigitField();
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
        Debug.Log("DigitField.UpdateValue()");
        if (increment_or_decrement)
        {
            currentPrice += (int)Math.Pow(10, d.GetIndex());
        }
        else
        {
            currentPrice -= (int)Math.Pow(10, d.GetIndex());
        }

        Debug.Log("(UpdateValue) After Current Value: " + currentPrice);
        if (OnPriceChange != null)
            OnPriceChange(currentPrice, basePrice);
    }

    void OnEnable()
    {
        Debug.Log("OnEnable DigitField");

        isTestScene = false;
        //isTestScene = true;

        if (itemForSale != null)
        {
            basePrice = itemForSale.basePrice;
        }

        if (!digitFieldInitialized)
        {
            InitDigitField();
        }
        

        if (isTestScene)
        {
            basePrice = 500;
            currentPrice = 500;
            suggestedMultiplier = 1.3M;
            ComputeSuggestedPrice();
            //TestField();
        }


        DigitSelector.OnIncrClick += CallIncrement;
        DigitSelector.OnDecrClick += CallDecrement;

        Digit.OnDigitClick += SelectDigit;
        Digit.OnDigitChange += UpdateValue;
    }

    void OnDisable()
    {
        Debug.Log("OnDisable DigitField");
        DigitSelector.OnIncrClick -= CallIncrement;
        DigitSelector.OnDecrClick -= CallDecrement;
        Digit.OnDigitClick -= SelectDigit;
        Digit.OnDigitChange -= UpdateValue;
    }
}
