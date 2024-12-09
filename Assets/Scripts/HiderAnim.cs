using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HiderAnim : MonoBehaviour
{
    private PlayerInput playerInput;
    private void OnEnable()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        playerInput.actions["Move"].started += hiderAnimation;
        playerInput.actions["Move"].canceled += hiderAnimationStop;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void hiderAnimation(InputAction.CallbackContext cb)
    {
        GetComponent<Animator>().SetBool("isRunning", true);
    }

    private void hiderAnimationStop(InputAction.CallbackContext cb)
    {
        GetComponent<Animator>().SetBool("isRunning", false);
    }

    
}
