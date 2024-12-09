using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackTrigger : MonoBehaviour
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
        if(other.tag.Equals("Player"))
        {
            other.GetComponentInChildren<PlayerInput>().actions["Jump"].Disable();

            for (int i = 2; i < 5; i++)
            {
                other.transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
            }
            other.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
            other.transform.GetChild(2).GetComponent<AudioSource>().volume = 0;
            GameObject child = other.transform.GetChild(1).GetChild(1).gameObject;
            GameObject ghost = other.transform.GetChild(1).GetChild(5).gameObject;
            child.GetComponent<Animator>().SetTrigger("isDead");
            ghost.SetActive(true);
            child.transform.SetParent(null);
            child.transform.GetChild(1).GetChild(2).GetChild(1).gameObject.SetActive(false);
            //other.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
            //for (int i = 0; i < 4; i++)
            //{
            //    other.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(i).GetComponent<SkinnedMeshRenderer>().material = GameManager.Instance.ghost;
            //}
            //
            //other.transform.GetChild(1).GetChild(5).gameObject.SetActive(true);
            other.transform.GetComponentInChildren<CinemachineVirtualCamera>().Follow = null;
            other.transform.GetChild(0).GetChild(1).localPosition = new Vector3(0.2f, 0, -4);
            other.tag = "Dead";
            GameManager.Instance.UpdateHiderIcons();
            GameManager.Instance.killed++;

            Debug.Log("killed"+GameManager.Instance.killed);
        }
    }

}
