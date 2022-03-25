using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Insert this script into a GameObject with multiple child Buttons used for a selection menu
public class ButtonKeyboardSelect : MonoBehaviour
{
    public float key;
    public Button[] optionsList;

    int index = 0;

    bool start = true;

    void Awake()
    {
        start = false;
    }

    // Update is called once per frame
    public void Update()
    {
        key = Input.GetAxis("VerticalKB");

        if (key > 0)
        {
            if (start)
            {
                index = 0;
                start = false;
            }
            else
            {
                if (index == 0)
                    index = optionsList.Length - 1;
                else
                    index -= 1;
            }

        }
        else if(key < 0)
        {
            if (start)
            {
                index = 0;
                start = false;
            }
            else
            {
                if (index == optionsList.Length - 1)
                    index = 0;
                else
                    index += 1;
            }
        }
    }
}
