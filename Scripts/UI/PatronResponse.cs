using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;

public class PatronResponse : MonoBehaviour
{
    public Button confirmButton;

    public GameObject confirmObj;
    public bool confirmClicked;
    public bool transactionEnded;

    public GameObject acceptObject;
    public GameObject rejectObject;
    public GameObject haggleObject;

    public int patronChoice;

    public GameObject[] responses;

    public TMP_Text text;



    private CancellationTokenSource cancellationTokenSource;
    /*
    Gradient gradient;
    GradientColorKey[] colorKey;
    GradientAlphaKey[] alphaKey;
    */

    void Start()
    {
        cancellationTokenSource = new CancellationTokenSource();
        /*
        gradient = new Gradient();

        // Populate the color keys at the relative time 0 and 1 (0 and 100%)
        colorKey = new GradientColorKey[2];
        colorKey[0].color = Color.red;
        colorKey[0].time = 0.0f;
        colorKey[1].color = Color.blue;
        colorKey[1].time = 1.0f;

        // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
        alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 0.0f;
        alphaKey[1].time = 1.0f;

        gradient.SetKeys(colorKey, alphaKey);

        // What's the color at the relative time 0.25 (25 %) ?
        Debug.Log(gradient.Evaluate(0.25f));
        */
    }

    void OnEnable()
    {
        responses = new GameObject[3];
        responses[0] = rejectObject;
        responses[1] = acceptObject;
        responses[2] = haggleObject;
        if(confirmButton == null)
        {
            confirmButton = confirmObj.GetComponent<Button>();
        }
        //confirmButton.onClick.AddListener(CloseResponse);
        confirmButton.onClick.AddListener(async () => await CloseResponseAsync());
        Debug.Log("PatronResponse Setup");
    }

    // Async method to handle patron's response to player's offer asynchronously
    public async Task HandleResponseAsync(int choice)
    {
        Debug.Log("HandleResponseAsync");
        patronChoice = choice;
        DisplayResponse(choice);
        var confirmation = await WaitForConfirmation();

        Debug.Log("End HandleResponseAsync");
    }

    // Wait for player to close the DisplayResponse() window
    public async Task<bool> WaitForConfirmation()
    {
        while (!confirmClicked)
        {
            await Task.Delay(1);
        }
        return true;
    }


    // Display whether the patron accepts or rejects the offer
    public void DisplayResponse(int choice)
    {
        Debug.Log("DisplayResponse");
        confirmClicked = false;
        transactionEnded = false;
        confirmObj.SetActive(true);
        var color = responses[choice].transform.GetChild(0).GetComponent<TextMeshProUGUI>().colorGradient; // responses is empty
        var text = responses[choice].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;


        if(!responses[choice].activeInHierarchy)
        {
            responses[choice].SetActive(true);
        }

        // Animating the text

        /*
        //Debug.Log("Choice = " + choice);
        //Debug.Log("Text: " + text);
        //Debug.Log("Color: " + color);
        //color.a = 0;
        //responses[choice].transform.GetChild(0).GetComponent<TMP_Text>().color.a = 0;
        Debug.Log("DisplayResponse: " + color);
        Debug.Log("ColorGradients 1: " + color.bottomLeft);
        Debug.Log("ColorGradients 2: " + color.bottomRight);
        Debug.Log("ColorGradients 3: " + color.topLeft);
        Debug.Log("ColorGradients 4: " + color.topRight);
        */

        Debug.Log("End DisplayResponse");
    }

    //public void CloseResponse()
    public async Task CloseResponseAsync()
    {
        Debug.Log("CloseResponse");
        responses[patronChoice].SetActive(false);
        confirmClicked = true;
        confirmObj.SetActive(false);
        //transactionEnded = true;
        Debug.Log("End CloseResponse");
        //EndTransaction();
    }

    //public void EndTransaction()
    public async Task EndTransactionAsync()
    {   
        Debug.Log("Start EndTransactionAsync");
        while (!confirmClicked)
        {
            await Task.Delay(1);
        }
        transactionEnded = true;
        Debug.Log("End EndTransactionAsync");
        
    }
}
