using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : Singleton<UIController>
{
    //public GameObject blackOutSquare;
    public float fadeSpeed = 0.6f;

    private void Awake()
    {
        //if(Mall.Instance. = cam;)

        DontDestroyOnLoad(this.gameObject);

        /*
        if (blackOutSquare != null)
        {
            DontDestroyOnLoad(blackOutSquare);

            if (blackOutSquare.activeInHierarchy)
                blackOutSquare.SetActive(false);
        }
        */
        Mall.Instance.UIController = this;
    }

    /*public IEnumerator FadeToBlack(bool fadeToBlack = true)
    {
        blackOutSquare.SetActive(true);
        Color objectColor = blackOutSquare.GetComponent<Image>().color;
        float fadeAmount;

        if(fadeToBlack)
        {
            Debug.Log("Fading to Black");
            objectColor.a = 0;
            blackOutSquare.GetComponent<Image>().color = objectColor;
            while (blackOutSquare.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackOutSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        }
        else
        {
            Debug.Log("Fading From Black");
            objectColor.a = 1;
            blackOutSquare.GetComponent<Image>().color = objectColor;
            while (blackOutSquare.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackOutSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        }


        Debug.Log("DONE");
        //blackOutSquare.SetActive(false);
        //yield return new WaitForEndOfFrame();
    }
    public IEnumerator FadeToBlack()
    {
        blackOutSquare.SetActive(true);
        Color objectColor = blackOutSquare.GetComponent<Image>().color;
        float fadeAmount;

        Debug.Log("Fading to Black");
        objectColor.a = 0;
        blackOutSquare.GetComponent<Image>().color = objectColor;
        while (blackOutSquare.GetComponent<Image>().color.a < 1)
        {
            fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            blackOutSquare.GetComponent<Image>().color = objectColor;
            yield return null;
        }
        Debug.Log("Fade To Black DONE");
    }

    public IEnumerator FadeFromBlack()
    {
        blackOutSquare.SetActive(true);
        Color objectColor = blackOutSquare.GetComponent<Image>().color;
        float fadeAmount;

        Debug.Log("Fading From Black");
        objectColor.a = 1;
        blackOutSquare.GetComponent<Image>().color = objectColor;
        while (blackOutSquare.GetComponent<Image>().color.a > 0)
        {
            fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            blackOutSquare.GetComponent<Image>().color = objectColor;
            yield return null;
        }
        Debug.Log("Fade From Black DONE");
    }
    */
}
