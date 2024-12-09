using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EscapeTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            other.tag = "Escape";
            other.GetComponentInChildren<PlayerInput>().actions["Jump"].Disable();

            for (int i = 1; i < 5; i++)
            {
                other.transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
            }
            //other.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
            GameManager.Instance.UpdateHiderIcons();
            GameManager.Instance.escaped++;
        }
    }
}
