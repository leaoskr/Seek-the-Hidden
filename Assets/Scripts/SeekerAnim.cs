using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SeekerAnim : MonoBehaviour
{
    public GameObject walkSound;
    private PlayerInput playerInput;
    public AudioClip attackSound;
    private float cooldown = 5f;
    public GameObject attackTrigger;
    private void OnEnable()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        playerInput.actions["Move"].started += seekerAnimation;
        playerInput.actions["Move"].canceled += seekerAnimationStop;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(cooldown <=0 && Input.GetMouseButtonDown(0))
        {
            Debug.Log("attack");
            GetComponent<AudioSource>().PlayOneShot(attackSound);
            GameManager.Instance.seekerAnimator.SetTrigger("isHitting");
            attackTrigger.SetActive(true);
            StartCoroutine(attack());
            cooldown = 4;
        }
        else
        {
            cooldown -= Time.deltaTime;
        }
    }

    private void seekerAnimation(InputAction.CallbackContext cb)
    {
        walkSound.GetComponent<AudioSource>().Play();
        GameManager.Instance.seekerAnimator.SetBool("isWalking", true);
    }

    private void seekerAnimationStop(InputAction.CallbackContext cb)
    {
        walkSound.GetComponent<AudioSource>().Pause();
        GameManager.Instance.seekerAnimator.SetBool("isWalking", false);
    }

    private IEnumerator attack()
    {
        yield return new WaitForSeconds(0.5f);
        attackTrigger.gameObject.SetActive(false);
    }
}
