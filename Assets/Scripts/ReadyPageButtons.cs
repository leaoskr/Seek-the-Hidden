using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyPageButtons : MonoBehaviour
{
    public GameObject readyButton;
    public GameObject readyButtonSelected;

    public GameManager gameManager;

    public int playerIndex;

    private void Start()
    {
        // Ensure only the unselected version is active at the beginning
        SetButtonState(readyButton, readyButtonSelected, false);
    }

    // Function to toggle the button's selected/unselected state
    private void SetButtonState(GameObject unselected, GameObject selected, bool isSelected)
    {
        unselected.SetActive(!isSelected);
        selected.SetActive(isSelected);
    }

    // Function to show Ready button selected
    public void ShowReadyButtonSelected()
    {
        SetButtonState(readyButton, readyButtonSelected, true);
    }

    // Function to handle click events for Ready button
    public void OnReadyButtonClicked()
    {
        gameManager.OnReadyGame(playerIndex);
    }
}
