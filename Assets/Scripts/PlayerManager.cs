using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerManager : MonoBehaviour
{
    public GameObject hiderPrefab;
    public Transform seekerSpawnPoint;
    public Transform hiderSpawnPoint;

    [SerializeField]
    private Transform readyjoystickPlayer;

    void OnPlayerJoined(PlayerInput playerInput)
    {
        int index = playerInput.playerIndex;
        Debug.Log("playerindex" + playerInput.playerIndex);
        initPlayer(index, playerInput);
        
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void initPlayer(int index, PlayerInput playerInput)
    {
        if(index == 0)
        {
            GameObject target = playerInput.gameObject;
            target.name = "Seeker";
            target.tag = "Seeker";
            target.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            target.transform.GetComponent<CharacterController>().height = 2;
            target.transform.GetComponent<CharacterController>().center = new Vector3(0,1.1f,0);
            //target.transform.GetChild(0).GetComponentInChildren<Camera>().targetDisplay = index;
            target.transform.GetComponentInChildren<Camera>().targetDisplay = index;
            target.transform.GetChild(0).transform.localPosition = new Vector3(0, 1.5f, -0.1f);
            target.transform.GetChild(0).GetChild(0).transform.localPosition = new Vector3(0, 1.375f, 0);
            

            target.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
            target.transform.GetComponent<FirstPersonController>().initialPos = seekerSpawnPoint.position;
            //target.transform.GetComponent<ThirdPersonController>().initialPos = seekerSpawnPoint.position;
            //target.transform.position = seekerSpawnPoint.position;
            //StartCoroutine(delayChangePosition(target));
            GameManager.Instance.seeker = target;
            GameManager.Instance.seekerAnimator = target.transform.GetChild(1).GetChild(0).GetComponent<Animator>();
            target.transform.GetChild(2).gameObject.SetActive(false);
            target.transform.GetChild(3).gameObject.SetActive(true);
        }
        if(index > 0)
        {
            
            GameObject target = playerInput.gameObject;
            target.transform.GetChild(0).GetChild(0).tag = "Hider";
            target.name = $"{index}";
            target.transform.GetComponent<FirstPersonController>().enabled = false;
            target.transform.GetComponent<ThirdPersonController>().enabled = true;
            target.transform.GetComponent<ThirdPersonController>().initialPos = hiderSpawnPoint.GetChild(index - 1).position;
            //StartCoroutine(delayChangePosition(target, hiderSpawnPoint.GetChild(index)));
            //target.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            target.transform.GetChild(0).GetChild(0).GetComponent<AudioListener>().enabled = false;
            target.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            target.transform.GetChild(0).GetChild(1).gameObject.layer = index + 6;
            
            target.transform.GetChild(0).GetComponentInChildren<Camera>().targetDisplay = index;
            target.transform.GetChild(0).GetComponentInChildren<Camera>().cullingMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Water", "UI", "Seeker", LayerMask.LayerToName(index + 6), "RenderAbove");

            //
            target.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            target.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);

            GameManager.Instance.hiders[index-1] = target;

            
            if (target.GetComponent<StarterAssetsInputs>().jump)
            {
                Debug.Log("press ready");
                readyjoystickPlayer.GetChild(index+4).GetComponent< ReadyPageJoystickButton >().OnReadyButtonPressed();
                StartCoroutine(DelayedReadyGame(index));

            }
        }
        
    }

    private IEnumerator DelayedReadyGame(int index)
    {
        yield return new WaitForSeconds(1f); // Wait for 1 seconds
        GameManager.Instance.OnReadyGame(index);
    }

    private IEnumerator delayChangePosition(GameObject target, Transform destination)
    {
        yield return new WaitForSeconds(2f);
        target.transform.position = destination.position;
        Debug.Log(target.transform.position + ",,," + destination.position);
    }

}
