using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UI;


public class ColorTile : MonoBehaviour
//public class ColorTile 
{
    //private static readonly List<StandardPalette> standardColors = Enum.GetValues(typeof(StandardPalette)).Cast<StandardPalette>().ToList();
    Array standardColors = Enum.GetValues(typeof(StandardPalette));

    
    public StandardPalette color;
    bool isFlooded;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Awake()
    {
        ReflectColor();

    }

    public void RandomizeColor()
    {
        color = (StandardPalette)standardColors.GetValue(UnityEngine.Random.Range(0, standardColors.Length));
        isFlooded = false;
        ReflectColor();
    }

    public StandardPalette GetColor()
    {
        return color;
    }

    public void SetColor(StandardPalette c)
    {
        color = c;
        ReflectColor();
    }

    void ReflectColor()
    {

        if(color == StandardPalette.yellow)
        {
            gameObject.GetComponent<Image>().color = Color.yellow;
        }
        else if(color == StandardPalette.blue)
        {
            gameObject.GetComponent<Image>().color = Color.blue;
        }
        else if (color == StandardPalette.magenta)
        {
            gameObject.GetComponent<Image>().color = Color.magenta;
        }
        else if (color == StandardPalette.red)
        {
            gameObject.GetComponent<Image>().color = Color.red;
        }
        else if (color == StandardPalette.cyan)
        {
            gameObject.GetComponent<Image>().color = Color.cyan;
        }
        else if (color == StandardPalette.blue)
        {
            gameObject.GetComponent<Image>().color = Color.blue;
        }
        else if (color == StandardPalette.green)
        {
            gameObject.GetComponent<Image>().color = Color.green;
        }
    }

    public bool IsFlooded()
    {
        return isFlooded;
    }

    public void SetFlooded()
    {
        isFlooded = true;
    }

}
