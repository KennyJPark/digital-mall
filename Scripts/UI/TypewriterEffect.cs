using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using TMPro;

// How to use:
// Attach to TMP Object; applies typewriter effect to the text field

[RequireComponent(typeof(TMP_Text))]

public class TypewriterEffect : MonoBehaviour
{
    private TMP_Text _textBox;

    // for prototyping
    //[Header("Test String")]
    //[SerializeField] private string testText;


    // basic functionality
    private int _currentVisibleCharacterIndex;
    private Coroutine _typewriterCoroutine;
    private bool _readyForNewText = true;
    private bool textCompleted = false;

    private WaitForSeconds _simpleDelay;
    private WaitForSeconds _interpunctuationDelay;

    [Header("Typewriter Settings")]
    [SerializeField] private float charactersPerSecond = 20;
    [SerializeField] private float interpunctuationDelay = 0.5f;

    // skipping functionality
    public bool CurrentlySkipping { get; private set; }
    private WaitForSeconds _skipDelay;

    [Header("Skip Options")] 
    [SerializeField] private bool quickSkip;
    [SerializeField] [Min(1)] private int skipSpeedup = 5;

    // Event Functionality
    private WaitForSeconds _textboxFullEventDelay;
    [SerializeField] [Range(0.1f, 0.5f)] private float sendDoneDelay = 0.25f;

    public static event Action CompleteTextRevealed;
    public static event Action<char> CharacterRevealed;

    private void Awake()
    {
        _textBox = GetComponent<TMP_Text>();
        _textBox.maxVisibleCharacters = 0;

        _simpleDelay = new WaitForSeconds(1 / charactersPerSecond);
        _interpunctuationDelay = new WaitForSeconds(interpunctuationDelay);

        _skipDelay = new WaitForSeconds(1 / (charactersPerSecond * skipSpeedup));
        _textboxFullEventDelay = new WaitForSeconds(sendDoneDelay);
    }

    private void OnEnable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(PrepareForNewText);
    }

    private void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(PrepareForNewText);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            //Debug.Log("Click");
            if(!textCompleted)
            {
                if (_textBox.maxVisibleCharacters != _textBox.textInfo.characterCount - 1)
                {
                    //Debug.Log("SKIPPING..");
                    Skip();
                }
            }

        }
    }

    // Example for how to implement it in the New Input system
    // You'd have to use a PlayerController component on this gameobject and write the function's name as On[Input Action name] for this to work.
    // In this case, my Input Action is called "RightMouseClick". But: There are a ton of ways to implement checking if a button
    // has been pressed in this system. Go watch Piti's video on the different ways of utilizing the new input system: https://www.youtube.com/watch?v=Wo6TarvTL5Q

    // private void OnRightMouseClick()
    // {
    //     if (_textBox.maxVisibleCharacters != _textBox.textInfo.characterCount - 1)
    //         Skip();
    // }

    public void PrepareForNewText(Object obj)
    {
        //Debug.Log("PrepareForNewText");
        if (!_readyForNewText)
        {
            return;
        }
        if(textCompleted)
        {
            return;
        }
        _readyForNewText = false;
        textCompleted = false;

        if (_typewriterCoroutine != null)
            StopCoroutine(_typewriterCoroutine);

        _textBox.maxVisibleCharacters = 0;
        _currentVisibleCharacterIndex = 0;

        _typewriterCoroutine = StartCoroutine(Typewriter());
    }

    private IEnumerator Typewriter()
    {
        //Debug.Log("START TYPEWRITER EFFECT");
        TMP_TextInfo textInfo = _textBox.textInfo;
        //Debug.Log("NUM CHARS:" + textInfo.characterCount);

        while (_currentVisibleCharacterIndex < textInfo.characterCount)
        {
            var lastCharacterIndex = textInfo.characterCount - 1;
            //Debug.Log("CURRENT INDEX:" + _currentVisibleCharacterIndex);
            if (_currentVisibleCharacterIndex == lastCharacterIndex)
            {
                _textBox.maxVisibleCharacters++;
                yield return _textboxFullEventDelay;

                
                //Debug.Log("LAST CHAR");
            }

            char character = textInfo.characterInfo[_currentVisibleCharacterIndex].character;

            _textBox.maxVisibleCharacters++;
            

            if(!CurrentlySkipping &&
                (character == '?' || character == '.' || character == ',' || character == ':' ||
                character == ';' || character == '!' || character == '-'))
            {
                yield return _interpunctuationDelay;
            }
            else
            {
                yield return CurrentlySkipping ? _skipDelay : _simpleDelay;

                /*
                if (CurrentlySkipping)
                    yield return _skipDelay;
                else
                    yield return _simpleDelay;
                */
            }

            _currentVisibleCharacterIndex++;
            //Debug.Log("WHILE( " + _currentVisibleCharacterIndex + " < " + (textInfo.characterCount + 1) + ")");
        }
        //Debug.Log("END TYPEWRITER EFFECT");
        _readyForNewText = true;
        textCompleted = true;
        CompleteTextRevealed?.Invoke();
    }

    void Skip()
    {
        if (CurrentlySkipping)
            return;

        CurrentlySkipping = true;

        if (!quickSkip)
        {
            //Debug.Log("NORMAL SKIPPING");
            StartCoroutine(SkipSpeedupReset());
            return;
        }
        //Debug.Log("QUICK SKIPPING");

        StopCoroutine(_typewriterCoroutine);
        
        _textBox.maxVisibleCharacters = _textBox.textInfo.characterCount;
        _readyForNewText = true;
        textCompleted = true;
        CompleteTextRevealed?.Invoke();
        //Debug.Log("END SKIP");
    }
    
    private IEnumerator SkipSpeedupReset()
    {
        yield return new WaitUntil(() => _textBox.maxVisibleCharacters == _textBox.textInfo.characterCount - 1);
        CurrentlySkipping = false;
    }

}
