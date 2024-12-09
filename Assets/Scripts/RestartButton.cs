using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButton : MonoBehaviour
{
    public GameObject restartButton;
    public GameObject restartButtonSelected;

    public GameManager gameManager;

    private void Start()
    {
        // Ensure only the unselected version is active at the beginning
        SetButtonState(restartButton, restartButtonSelected, false);
    }

    // Function to toggle the button's selected/unselected state
    private void SetButtonState(GameObject unselected, GameObject selected, bool isSelected)
    {
        unselected.SetActive(!isSelected);
        selected.SetActive(isSelected);
    }

    // Function to show Restart button selected
    public void ShowRestartButtonSelected()
    {
        SetButtonState(restartButton, restartButtonSelected, true);
    }

    // Function to handle click events for Restart button
    public void OnRestartButtonClicked()
    {
        gameManager.OnRestartGame();
    }
}
