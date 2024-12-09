using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static  GameManager Instance;

    public Transform blueLever;
    public Transform greenLever;
    public Transform orangeLever;
    public Transform blueDoor;
    public Transform greenDoor;
    public Transform orangeDoor;
    public GameObject leverHighlight;

    public bool blueDoorOpen = false;
    public bool orangeDoorOpen = false;
    public bool greenDoorOpen = false;
    public float blueDoorOpenTime = 0f;
    public float greenDoorOpenTime = 0f;
    public float orangeDoorOpenTime = 0f;
    public GameObject blueLight;
    public GameObject greenLight;   
    public GameObject orangeLight;
    public Animator siren;
    public AudioSource alarm;
    public bool phase2 = false;

    public Material ghost;

    public GameObject officeDoor;
    public GameObject seeker;
    public GameObject[] hiders = new GameObject[3];


    // UI
    public Canvas[] startCanvases;
    public Canvas[] readyCanvases;
    public Canvas[] endCanvases;
    public Canvas[] countdownCanvases;
    public Camera[] UICameras;
    public TextController[] textController;

    public bool gameReady = false;
    private int readyCount = 0;
    private bool readyCheck = false;

    public float gameDuration = 180f;

    // Sounds
    public GameObject buttonSound;
    public GameObject readyButtonSound;
    public GameObject countdownSound;
    public GameObject roomSound;

    // Animation
    public Animator seekerAnimator;

    public int killed = 0;
    public int escaped = 0;

    // Heart rate
    public int[] heartRates;

    bool isCursorShow = true;


    void Start()
    {

        roomSound.SetActive(false);
        Instance = this;
        ShowStartPage(true);
        ShowReadyPage(false);
        ShowCountDownPage(false);
        ShowEndPage(false);

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isCursorShow = !isCursorShow;
            if (!isCursorShow) Cursor.lockState = CursorLockMode.Locked;
            else Cursor.lockState = CursorLockMode.None;
            Cursor.visible = isCursorShow;
        }

        if (!readyCheck)
        {
            BeginCountdown();
        }

        // inner game 3 minutes countdown
        if (GameData.gameStart)
        {
            if (gameDuration >= 0)
            {
                gameDuration -= Time.deltaTime;
                UpdateTimer(gameDuration);
            }
            if (gameDuration < 122 && !phase2)
            {
                phase2 = true;
                blueLight.SetActive(true);
                greenLight.SetActive(true);
                orangeLight.SetActive(true);
                siren.SetBool("isOn", true);
                alarm.Play();
                leverHighlight.SetActive(true);
                StartCoroutine(turnOff());
            }
        }

        // Decide the winner
        if(escaped >= 2)
        {
            // Hiders win
            GameData.seekerWin = false;
            //GameEnd();
        }
        else if (escaped < 2 && escaped >=0)
        {
            // Seeker win
            GameData.seekerWin = true;
            //GameEnd();
        }

        // Check if game end
        // Either all hiders esccaped
        // Or the time ends
        //if (escaped == 3 || killed == 3)
        if ((escaped + killed) == 3)
        {
            GameData.gameEnd = true;
            GameEnd();
        }

        if (blueDoorOpen)
        {
            blueDoorOpenTime += Time.deltaTime;
        }

        if (orangeDoorOpen)
        {
            orangeDoorOpenTime += Time.deltaTime;
        }

        if (greenDoorOpen)
        {
            greenDoorOpenTime += Time.deltaTime;
        }

        if (blueDoorOpenTime > 60)
        {
            blueDoorOpenTime = 0;
            blueDoorOpen = false;
            blueDoor.GetComponent<Animator>().SetBool("DoorOpen", false);
            blueLever.GetComponentInChildren<Animator>().SetBool("isDownPressed", false);
        }

        if (greenDoorOpenTime > 60)
        {
            greenDoorOpenTime = 0;
            greenDoorOpen = false;
            greenDoor.GetComponent<Animator>().SetBool("DoorOpen", false);
            greenLever.GetComponentInChildren<Animator>().SetBool("isDownPressed", false);
        }

        if (orangeDoorOpenTime > 60)
        {
            orangeDoorOpenTime = 0;
            orangeDoorOpen = false;
            orangeDoor.GetComponent<Animator>().SetBool("DoorOpen", false);
            orangeLever.GetComponentInChildren<Animator>().SetBool("isDownPressed", false);
        }

    }

    private IEnumerator turnOff()
    {
        yield return new WaitForSeconds(10);
        siren.SetBool("isOn", false);
        alarm.Stop();
        leverHighlight.SetActive(false);
    }

    private void ShowStartPage(bool show)
    {
        foreach (Canvas canvas in startCanvases)
        {
            canvas.gameObject.SetActive(show);
        }
    }

    private void ShowReadyPage(bool show)
    {
        foreach (Canvas canvas in readyCanvases)
        {
            canvas.gameObject.SetActive(show);
        }
    }
    private void ShowCountDownPage(bool show)
    {
        foreach (Canvas canvas in countdownCanvases)
        {
            canvas.gameObject.SetActive(show);
        }
    }

    private void ShowEndPage(bool show)
    {
        foreach (Canvas canvas in endCanvases)
        {
            Transform seekerWinObject = canvas.transform.Find("Seeker Win");
            Transform hiderWinObject = canvas.transform.Find("Hider Win");
            TextMeshProUGUI overviewText = canvas.transform.Find("Overview Text").GetComponent<TextMeshProUGUI>();

            if (seekerWinObject != null && hiderWinObject != null && overviewText != null)
            {
                if (GameData.seekerWin)
                {
                    // Show "Seeker Win" and hide "Hider Win"
                    seekerWinObject.gameObject.SetActive(true);
                    hiderWinObject.gameObject.SetActive(false);
                    overviewText.text = $"Killed Hider Number: {killed}";
                }
                else
                {
                    // Hide "Seeker Win" and show "Hider Win"
                    seekerWinObject.gameObject.SetActive(false);
                    hiderWinObject.gameObject.SetActive(true);
                    overviewText.text = $"Escaped Hider Number: {escaped}";
                }
            }

            // Enable or disable the entire canvas
            canvas.gameObject.SetActive(show);
        }
    }

    public void OnStartGame()
    {
        Debug.Log("Start button clicked. Starting the game...");
        gameReady = true;
        buttonSound.GetComponentInParent<AudioSource>().Play();

        ShowStartPage(false);
        ShowReadyPage(true);
        // Only when All 3 Controller Players connects to their controller
        // The Start Button can be clicked
    }

    public void OnQuitGame()
    {
        Debug.Log("Quit button clicked. Quitting the game...");
        Application.Quit();
    }

    public void OnReadyGame(int playerIndex)
    {
        Debug.Log("Player " + playerIndex + " clicked ready. Turning off their Ready canvas...");
        
        if (playerIndex >= 0 && playerIndex < readyCanvases.Length)
        {
            readyButtonSound.GetComponentInParent<AudioSource>().Play();

            countdownCanvases[playerIndex].gameObject.SetActive(true);
            readyCanvases[playerIndex].gameObject.SetActive(false);
            UICameras[playerIndex].gameObject.SetActive(false);

        }
        else
        {
            Debug.LogWarning("Invalid player index: " + playerIndex);
        }

        textController[playerIndex].ShowReadyText();

        readyCount++;
    }

    public void BeginCountdown()
    {
        if (readyCount == 4)
        {
            readyCheck = true;

            countdownSound.GetComponentInParent<AudioSource>().Play();

            foreach (TextController text in textController)
            {
                StartCoroutine(text.StartCountdown());
            }
        }
    }

    public void GameEnd()
    {
        if (GameData.gameEnd)
        {
            ShowEndPage(true);
        }
    }

    public void OnRestartGame()
    {
        Debug.Log("Restart the game");

        // TODO: The beginning setting of the game
        // Show the title pages, hide ready pages, hide end pages, players set to start position
    }

    private void UpdateTimer(float timeToDisplay)
    {
        timeToDisplay -= 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        if (minutes == 0 && seconds == 0)
        {
            // time ends, hiders that are not escaped will be killed
            //foreach (GameObject hider in hiders)
            //{
            //    if (hider.tag != "Escape")
            //    {
            //        hider.tag = "Dead";
            //        killed++;
            //    }
            //}

            GameData.gameEnd = true;
            GameEnd();
        }

        foreach (Canvas canvas in countdownCanvases)
        {
            TextMeshProUGUI timerText = canvas.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
            timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        }
    }

    public void UpdateHiderIcons()
    {
        for (int i = 0; i < hiders.Length; i++)
        {
            foreach (Canvas canvas in countdownCanvases)
            {
                Transform hiderIcons = canvas.transform.GetChild(2);

                Transform hiderIcon = hiderIcons.GetChild(i);

                // Determine the state of the hider
                if (hiders[i].CompareTag("Dead")) 
                {
                    hiderIcon.Find("Hider Normal").gameObject.SetActive(false);
                    hiderIcon.Find("Hider Die").gameObject.SetActive(true);
                    hiderIcon.Find("Hider Escape").gameObject.SetActive(false);
                }
                else if (hiders[i].CompareTag("Escape"))
                {
                    hiderIcon.Find("Hider Normal").gameObject.SetActive(false);
                    hiderIcon.Find("Hider Die").gameObject.SetActive(false);
                    hiderIcon.Find("Hider Escape").gameObject.SetActive(true);
                }
                else // Default state is "Normal"
                {
                    hiderIcon.Find("Hider Normal").gameObject.SetActive(true);
                    hiderIcon.Find("Hider Die").gameObject.SetActive(false);
                    hiderIcon.Find("Hider Escape").gameObject.SetActive(false);
                }
            }
        }
    }

    public void GateNotification()
    {
        foreach (Canvas canvas in countdownCanvases)
        {
            GameObject gateHint = canvas.transform.GetChild(4).gameObject;
            StartCoroutine(ShowGateHint(gateHint));
        }
    }

    private IEnumerator ShowGateHint(GameObject gateHint)
    {
        gateHint.SetActive(true);
        yield return new WaitForSeconds(5f);
        gateHint.SetActive(false);
    }

    public void HeartHighAnimation(int hiderIndex)
    {
        GameObject heartIcon = countdownCanvases[hiderIndex].transform.Find("Heart Icon").gameObject;

        Animator heartIconAnimator = heartIcon.GetComponent<Animator>();

        if (heartIconAnimator != null)
        {
            heartIconAnimator.SetTrigger("heartHigh");
        }
        else
        {
            Debug.LogWarning("Animator component not found on Heart Icon.");
        }
    }

    public void updateBpm(int hiderIndex, string bpm)
    {
        countdownCanvases[hiderIndex].transform.GetChild(5).GetComponentInChildren<TextMeshProUGUI>().text = bpm;
    }
}
