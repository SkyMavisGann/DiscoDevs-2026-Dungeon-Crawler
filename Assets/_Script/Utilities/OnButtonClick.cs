using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OnButtonClick : MonoBehaviour
{
    public InputAction buttonToPress;
    public InstantiateGraphic buttonClickGraphic;
    public Button.ButtonClickedEvent onButtonClickEvent;

    [HideInInspector]
    public InputAction UIAction;

    private void OnEnable()
    {
        
        UIAction = buttonToPress;
        UIAction.Enable();
        UIAction.performed += OptionToClickAppear;
    }

    private void OnDisable()
    {
        UIAction.Disable();
    }


    private void Update()
    {
        if (buttonClickGraphic != null)
        {
            var submitAction = buttonToPress.ToString();
            buttonClickGraphic.createdObject.GetComponentInChildren<TMP_Text>().text = $"\'{submitAction}\' to Interact";
        }
    }
    private void OptionToClickAppear(InputAction.CallbackContext context)
    {
        onButtonClickEvent.Invoke();
    }
}
