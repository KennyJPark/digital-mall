using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Percent : MonoBehaviour
{
    int percent;
    float percent_float;

    void Awake()
    {
        gameObject.GetComponent<TMP_Text>().text = null;

    }

    public void SetPercent(int salePrice, int basePrice)
    {
        Debug.Log("Percent.SetPercent()");
        percent = (int)(100.00 * ((float)salePrice / (float)basePrice));
        //Debug.Log("100.00 * (" + (float)salePrice + " / " + (float)basePrice + ")");
        percent_float = (100.00f * ((float)salePrice / (float)basePrice));
        //Debug.Log(percent_float + "%");
        //gameObject.GetComponent<TMP_Text>().text = percent.ToString();

        //gameObject.GetComponent<TMP_Text>().text += "Sale Price: ";
        //gameObject.GetComponent<TMP_Text>().text += salePrice.ToString();
        //gameObject.GetComponent<TMP_Text>().text += " = ";
        gameObject.GetComponent<TMP_Text>().text = percent_float.ToString("0.00");


        if (percent_float >= 130.00)
        {
            gameObject.GetComponent<TMP_Text>().color = new Color32(0, 255, 159, 255);
        }
        else
        {
            gameObject.GetComponent<TMP_Text>().color = new Color32(226, 32, 24, 227);
        }
        
        
    }
    
    void OnEnable()
    {
        DigitField.OnPriceChange += SetPercent;
    }

    void OnDisable()
    {
        DigitField.OnPriceChange -= SetPercent;
    }
}
