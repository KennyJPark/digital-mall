using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScrollingText : MonoBehaviour
{
    //public ScrollingText instance { get; private set; }

    string text;

    float scrollTimer;
    float currentTimer;
    float skipTimer;

    private TMP_Text dialogueText;
    private bool hasTextChanged;

    void Awake()
    {
        dialogueText = GetComponent<TMP_Text>();
        scrollTimer = 0.05f;
        skipTimer = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScrollTimer(int timer)
    {
        scrollTimer = timer;
    }

    // Event received when the text object has changed.
    void ON_TEXT_CHANGED(Object obj)
    {
        hasTextChanged = true;
    }

    public void PrintText(string message)
    {
        StartCoroutine(PrintScrollingText(dialogueText, message));
    }

    /// Method revealing the text one character at a time.
    IEnumerator PrintScrollingText(TMP_Text textComponent, string message)
    {
        textComponent.text = message;
        textComponent.ForceMeshUpdate();

        TMP_TextInfo textInfo = textComponent.textInfo;

        int totalVisibleCharacters = textInfo.characterCount; // Get # of Visible Character in text object
        int visibleCount = 0;

        while (true)
        {
            if (hasTextChanged)
            {
                totalVisibleCharacters = textInfo.characterCount; // Update visible character count.
                hasTextChanged = false;
            }

            /*
            if (visibleCount > totalVisibleCharacters)
            {
                yield return new WaitForSeconds(1.5);
                visibleCount = 0;
            }
            */

            textComponent.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?

            visibleCount += 1;

            /*
            if (Time.deltaTime > skipTimer && Input.anyKey)
            {
                scrollTimer = 0.0f;
            }
            */

            yield return new WaitForSeconds(scrollTimer);
            //yield return null;
        }
    }
    
}
