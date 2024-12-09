using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEngine;
using UnityEngine.InputSystem;

public class TransformModels : MonoBehaviour
{
    private PlayerInput playerInput;
    //private bool blueDoorOpen;
    //private bool orangeDoorOpen;
    //private bool greenDoorOpen;

    //private float blueDoorOpenTime = 0f;
    //private float greenDoorOpenTime = 0f;
    //private float orangeDoorOpenTime = 0f;
    private int currentModelIndex = 1;
    private void OnEnable()
    {
        
        playerInput = GetComponentInParent<PlayerInput>();
        playerInput.actions["Jump"].started += transformModel;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(blueDoorOpen)
        //{
        //    blueDoorOpenTime += Time.deltaTime;
        //}

        //if (orangeDoorOpen)
        //{
        //    orangeDoorOpenTime += Time.deltaTime;
        //}

        //if (greenDoorOpen)
        //{
        //    greenDoorOpenTime += Time.deltaTime;
        //}

        //if (blueDoorOpenTime > 60)
        //{
        //    blueDoorOpenTime = 0;
        //    blueDoorOpen = false;
        //    GameManager.Instance.blueDoor.GetComponent<Animator>().SetBool("DoorOpen", false);
        //    GameManager.Instance.blueLever.GetComponentInChildren<Animator>().SetBool("isDownPressed", false);
        //}

        //if (greenDoorOpenTime > 60)
        //{
        //    greenDoorOpenTime = 0;
        //    greenDoorOpen = false;
        //    GameManager.Instance.greenDoor.GetComponent<Animator>().SetBool("DoorOpen", false);
        //    GameManager.Instance.greenLever.GetComponentInChildren<Animator>().SetBool("isDownPressed", false);
        //}

        //if (orangeDoorOpenTime > 60)
        //{
        //    orangeDoorOpenTime = 0;
        //    orangeDoorOpen = false;
        //    GameManager.Instance.orangeDoor.GetComponent<Animator>().SetBool("DoorOpen", false);
        //    GameManager.Instance.orangeLever.GetComponentInChildren<Animator>().SetBool("isDownPressed", false);
        //}
    }

    private void transformModel(InputAction.CallbackContext cb)
    {
        if(GameManager.Instance.gameDuration > 120 )
        {
            for (int i = 1; i < 5; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            currentModelIndex++;
            if (currentModelIndex == 5)
            {
                currentModelIndex = 1;
            }
            transform.GetChild(currentModelIndex).gameObject.SetActive(true);
        }
        else if (Vector3.Distance(transform.position, GameManager.Instance.blueLever.position) < 2.5f)
        {
            transform.GetChild(1).gameObject.SetActive(true);
            for (int i = 2; i < 5; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            GameManager.Instance.GateNotification();
            transform.GetChild(1).GetComponent<Animator>().SetTrigger("isPulling");
            GameManager.Instance.blueDoorOpen = true;
            GameManager.Instance.blueLever.GetComponentInChildren<Animator>().SetBool("isDownPressed",true);
            GameManager.Instance.blueDoor.GetComponent<Animator>().SetBool("DoorOpen", true);
            GameManager.Instance.blueDoor.GetComponent<AudioSource>().Play();
        }
        else if (Vector3.Distance(transform.position, GameManager.Instance.greenLever.position) < 2.5f)
        {
            transform.GetChild(1).gameObject.SetActive(true);
            for (int i = 2; i < 5; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            GameManager.Instance.GateNotification();
            GameManager.Instance.greenDoorOpen = true;
            transform.GetChild(1).GetComponent<Animator>().SetTrigger("isPulling");
            GameManager.Instance.greenLever.GetComponentInChildren<Animator>().SetBool("isDownPressed", true);
            GameManager.Instance.greenDoor.GetComponent<Animator>().SetBool("DoorOpen", true);
            GameManager.Instance.greenDoor.GetComponent<AudioSource>().Play();
        }
        else if (Vector3.Distance(transform.position, GameManager.Instance.orangeLever.position) < 2.5f)
        {
            transform.GetChild(1).gameObject.SetActive(true);
            for (int i = 2; i < 5; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            GameManager.Instance.GateNotification();
            GameManager.Instance.orangeDoorOpen = true;
            transform.GetChild(1).GetComponent<Animator>().SetTrigger("isPulling");
            GameManager.Instance.orangeLever.GetComponentInChildren<Animator>().SetBool("isDownPressed", true);
            GameManager.Instance.orangeDoor.GetComponent<Animator>().SetBool("DoorOpen", true);
            GameManager.Instance.orangeDoor.GetComponent<AudioSource>().Play();
        }
        else
        {
            for (int i = 1; i < 5; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            currentModelIndex++;
            if (currentModelIndex == 5)
            {
                currentModelIndex = 1;
            }
            transform.GetChild(currentModelIndex).gameObject.SetActive(true);
        }
        
    }
}
