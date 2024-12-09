using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextController : MonoBehaviour
{
    public TextMeshProUGUI statusText;
    public GameObject introUI;

    public void HideStatusText()
    {
        statusText.gameObject.SetActive(false);
        statusText.text = "";
    }

    // Function that turns on the text object and shows "Ready" when the player is ready
    public void ShowReadyText()
    {
        statusText.gameObject.SetActive(true);
        statusText.text = "Ready";
    }

    // Coroutine that shows countdown from "10" to "00" when all players are ready
    public IEnumerator StartCountdown()
    {
        statusText.gameObject.SetActive(true);

        // Countdown from 10 to 0
        for (int i = 10; i >= 0; i--)
        {
            statusText.text = i.ToString("D2"); // Format as two digits, e.g., "09"
            yield return new WaitForSeconds(1f); // Wait for 1 second between numbers
        }

        statusText.gameObject.SetActive(false); // Hide text after countdown
        introUI.gameObject.SetActive(false); // Hide intro UI after countdown

        GameData.gameStart = true; // -> start the inner game 3 minutes countdown

        GameManager.Instance.roomSound.SetActive(true);
        GameManager.Instance.officeDoor.GetComponent<Animator>().SetTrigger("isUnlocked");
    }
}
