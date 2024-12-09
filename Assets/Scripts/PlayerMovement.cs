using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;
using static UnityEngine.InputSystem.InputAction;

public class PlayerMovement : MonoBehaviour
{
    public JoystickControls joystickControls;

    public int hiderMoveSpeed = 10;
    [SerializeField] private int playerIndex = 0;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private CharacterController _controller;


    public float SpeedChangeRate = 10.0f;
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;
    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;
    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;
    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;

    private float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;

    private GameObject _mainCamera;

    private bool correctControl = false;
    private Vector2 moveValue;
    private Vector2 lookValue;

    private const float _threshold = 0.01f;
    // cinemachine
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    private void Awake()
    {
        joystickControls = new JoystickControls();
        // get a reference to our main camera
        if (_mainCamera == null)
        {
            //_mainCamera = GameObject.FindGameObjectWithTag("Hider");
            _mainCamera = transform.GetChild(0).GetChild(0).gameObject;
        }
    }

    void OnEnable()
    {
        joystickControls.Enable();
        joystickControls.Hider.Move.performed += hiderMove;
        joystickControls.Hider.Move.canceled += hiderMoveStop;
        joystickControls.Hider.Look.performed += hiderLook;
        joystickControls.Hider.Look.canceled += hiderLookStop;
    }

    void OnDisable()
    {
        joystickControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

    }

    // Update is called once per frame
    void Update()
    {
        Move();

        //transform.position += new Vector3(moveAction.ReadValue<Vector2>().x, 0f, moveAction.ReadValue<Vector2>().y) * hiderMoveSpeed * Time.deltaTime;
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    public void Move()
    {
        //if (!correctControl)
        //{
        //    return;
        //}
        float targetSpeed = 3;

        // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (moveValue == Vector2.zero) targetSpeed = 0.0f;

        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = 1f;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * SpeedChangeRate);

            // round speed to 3 decimal places
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f;

        // normalise input direction
        Vector3 inputDirection = new Vector3(moveValue.x, 0.0f, moveValue.y).normalized;

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (moveValue != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        // move the player
        _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                         new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
    }

    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (lookValue.sqrMagnitude >= _threshold)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = Time.deltaTime;

            _cinemachineTargetYaw += lookValue.x * deltaTimeMultiplier;
            _cinemachineTargetPitch += lookValue.y * deltaTimeMultiplier;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }
    
    public void hiderLook(InputAction.CallbackContext context)
    {
        if (!PlayerDeviceManager.GetInstance().IsPlayerDevice(playerIndex, context.control.device.deviceId))
        {
            return;
        }
        lookValue = context.ReadValue<Vector2>();
    }

    public void hiderMove(InputAction.CallbackContext context)
    {
        if (!PlayerDeviceManager.GetInstance().IsPlayerDevice(playerIndex, context.control.device.deviceId)) {
            correctControl = false;
            return; 
        }
        transform.GetChild(1).GetChild(1).GetComponent<Animator>().SetBool("isRunning", true);
        
        correctControl = true;
        moveValue = context.ReadValue<Vector2>();
    }

    private void hiderMoveStop(InputAction.CallbackContext context)
    {
        if (!PlayerDeviceManager.GetInstance().IsPlayerDevice(playerIndex, context.control.device.deviceId)) { return; }
        transform.GetChild(1).GetChild(1).GetComponent<Animator>().SetBool("isRunning", false);
        moveValue = new Vector2(0, 0);
    }

    private void hiderLookStop(InputAction.CallbackContext context)
    {
        if (!PlayerDeviceManager.GetInstance().IsPlayerDevice(playerIndex, context.control.device.deviceId)) { return; }
        lookValue = new Vector2(0, 0);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

}

