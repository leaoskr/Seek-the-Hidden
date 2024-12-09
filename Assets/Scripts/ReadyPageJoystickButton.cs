using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReadyPageJoystickButton : MonoBehaviour
{
    public GameObject readyButton;
    public GameObject readyButtonSelected;
    public GameManager gameManager;

    private bool isReady = false;

    private void Start()
    {
        // Ensure only the unselected version is active at the beginning
        SetButtonState(readyButton, readyButtonSelected, false);
    }

    // Callback for when the A button (Transform action) is pressed
    public void OnReadyButtonPressed()
    {
        if (!isReady)
        {
            ShowReadyButtonSelected();
            isReady = true; // Prevent multiple triggers
        }
    }

    // Function to toggle the button's selected/unselected state
    private void SetButtonState(GameObject unselected, GameObject selected, bool isSelected)
    {
        unselected.SetActive(!isSelected);
        selected.SetActive(isSelected);
    }

    // Function to show Ready button selected
    private void ShowReadyButtonSelected()
    {
        SetButtonState(readyButton, readyButtonSelected, true);
    }
}
