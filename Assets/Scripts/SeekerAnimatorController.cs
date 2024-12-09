using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class SeekerAnimatorController : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        bool isRunning = GetComponent<StarterAssetsInputs>().move != Vector2.zero;
        if (isRunning)
        {
            Debug.Log("Is Running");
        } else
        {
            Debug.Log("Not Running");
        }

        //start running
        animator.SetBool("isRunning", isRunning);
    }
}
