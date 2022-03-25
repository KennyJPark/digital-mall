using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMoney : MonoBehaviour
{
    public static UIMoney instance { get; private set; }

    /*
    public static UIMoney instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(UIMoney)) as UIMoney;

            return instance;
        }
        set
        {
            instance = value;
        }
    }
    */
    public TMP_Text moneyText;

    void Awake()
    {
        instance = this;
        moneyText = GetComponent<TMP_Text>();
        moneyText.text = "Money: ";
        //float m = 1000.0f;
        //moneyText.text = "Money: " + m.ToString();
    }

    public void SetMoney(float money)
    {
        moneyText.text = "Money: " + money.ToString();
    }
}
