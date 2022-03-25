using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ModalWindowPanel : MonoBehaviour
{
    public int modalType;

    [Header("Header")]
    [SerializeField]
    private Transform headerArea;
    [SerializeField]
    private TextMeshProUGUI titleField;

    [Header("Content")]
    [SerializeField]
    private Transform contentArea;
    [SerializeField]
    private Transform _verticalLayoutArea;
    [SerializeField]
    private Image verticalImage;
    [SerializeField]
    private TextMeshProUGUI _verticalText;
    [Space()]
    [SerializeField]
    private Transform _horizontalLayoutArea;
    [SerializeField]
    private Transform _iconContainer;
    [SerializeField]
    private Image _iconImage;
    [SerializeField]
    private TextMeshProUGUI _iconText;

    [Header("Footer")]
    [SerializeField]
    private Transform _footerArea;
    [SerializeField]
    private Button _confirmButton;
    [SerializeField]
    private Button _declineButton;
    [SerializeField]
    private Button _alternateButton;

    // For use with Items for sale
    [SerializeField]
    private Button _removeButton;
    [SerializeField]
    private Button _swapButton;
    [SerializeField]
    private Button _undoButton;
    [SerializeField]
    private Button _resetButton;


    private Action onConfirmAction;
    private Action onDeclineAction;
    private Action onAlternateAction;

    void Awake()
    {
        if(modalType == 0)
        {
            _removeButton.onClick.AddListener(RemoveItem);
            _swapButton.onClick.AddListener(SwapItem);
            _undoButton.onClick.AddListener(UndoItemChanges);
            _resetButton.onClick.AddListener(ResetItemChanges);
        }

        _confirmButton.onClick.AddListener(Confirm);
        _declineButton.onClick.AddListener(Decline);
        _alternateButton.onClick.AddListener(Alternate);
    }

    public void RemoveItem()
    {

    }

    public void SwapItem()
    {

    }

    public void UndoItemChanges()
    {

    }

    public void ResetItemChanges()
    {

    }

    public void Confirm()
    {
        Debug.Log("Confirm");
        //onConfirmAction?.Invoke();
        //Close();
    }
    public void Decline()
    {
        Debug.Log("Decline");
        //onDeclineAction?.Invoke();
        //Close();
    }
    public void Alternate()
    {
        Debug.Log("Alternate");
        //onAlternateAction?.Invoke();
        //Close();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
