using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartPageButtons : MonoBehaviour
{
    // UI
    public GameObject startButton;
    public GameObject startButtonSelected;
    public GameObject quitButton;
    public GameObject quitButtonSelected;

    public GameManager gameManager;

    private void Start()
    {
        // Ensure only the unselected versions are active at the beginning
        SetButtonState(startButton, startButtonSelected, false);
        SetButtonState(quitButton, quitButtonSelected, false);
    }

    // Function to toggle the button's selected/unselected state
    private void SetButtonState(GameObject unselected, GameObject selected, bool isSelected)
    {
        unselected.SetActive(!isSelected);
        selected.SetActive(isSelected);
    }

    // Functions to show/hide Start and Quit button selected images
    public void ShowStartButtonSelected()
    {
        SetButtonState(startButton, startButtonSelected, true);
        SetButtonState(quitButton, quitButtonSelected, false);
    }
        
    public void HideStartButtonSelected()
    {
        SetButtonState(startButton, startButtonSelected, false);
        SetButtonState(quitButton, quitButtonSelected, false);
    }

    public void ShowQuitButtonSelected()
    {
        SetButtonState(quitButton, quitButtonSelected, true);
        SetButtonState(startButton, startButtonSelected, false);
    }

    public void HideQuitButtonSelected()
    {
        SetButtonState(quitButton, quitButtonSelected, false);
        SetButtonState(startButton, startButtonSelected, false);
    }


    // Functions to handle click events for Start and Quit buttons
    public void OnStartButtonClicked()
    {
        gameManager.OnStartGame();
    }

    public void OnQuitButtonClicked()
    {
        gameManager.OnQuitGame();
    }
}
