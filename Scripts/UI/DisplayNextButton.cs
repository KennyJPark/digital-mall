using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayNextButton : MonoBehaviour
{
    [SerializeField] private GameObject nextButton;

    private void OnEnable()
    {
        TypewriterEffect.CompleteTextRevealed += ShowNextButton;
    }

    private void OnDisable()
    {
        TypewriterEffect.CompleteTextRevealed -= ShowNextButton;
    }

    private void ShowNextButton()
    {
        nextButton.SetActive(true);
    }

    public void HideNextButton()
    {
        nextButton.SetActive(false);
    }
}

