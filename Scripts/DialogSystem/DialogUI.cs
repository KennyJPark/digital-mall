using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogUI : MonoBehaviour
{
    // Need Speaker
    // Need Portrait
    // Need Text dialog
}




/*
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
    public event System.Action TypingTextEnded;

    public Text DialogBoxText;
    public Button NextButton;
    public float TypeTextDelay = 0.05f;

    public void ShowText(string text, bool shouldType){

        this.gameObject.SetActive(true);
        if(shouldType){
            StartCoroutine(TypeText(text));
        } else {
            DialogBoxText.text = text;
        }
    }

    // Typing effect
    private IEnumerator TypeText(string text)
    {
        string fullText = text;
        string currentText;
        for(int i = 0; i < fullText.Length + 1; i++)
        {
            currentText = fullText.Substring(0, i);
            DialogBoxText.text = currentText;
            yield return new WaitForSeconds(TypeTextDelay);
        }
        TypingTextEnded?.Invoke();
        yield return null;
    }
}

 * */