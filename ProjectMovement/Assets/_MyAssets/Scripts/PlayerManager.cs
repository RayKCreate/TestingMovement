using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [Header("Components")]
    private InputActions newActions;
    CharacterController playerController;
    //[SerializeField] LeanDetection leanDetectorL;
    //[SerializeField] LeanDetection leanDetectorR;

    [SerializeField] Animator playerAnimator;
    [SerializeField] CinemachineOrbitalFollow cinemachineFollow;


    [Header("Walk and Rotate Variables")]
    Vector2 walk;
    float initSpeed;
    float actualSpeed;
    [SerializeField] float walkForwardSpeed;
    [SerializeField] float progressiveWalkSpeed;
    float walkExponent;
    [SerializeField] float walkBackwardSpeed;
    [SerializeField] float RunSpeed;
    float rotate;
    float rotateSpeed;
    [SerializeField] float crouchWalkForward;
    [SerializeField] float crouchWalkBackward;

    [Header("Jump Variables")]
    float gravity;
    float fallVelocity;
    [SerializeField] float jumpHeight; 
    RaycastHit groundDetect;

    float cameraHorizontalValue;
    float cameraVerticalValue;


    [Header("Check Variables")]
    bool isRunning;
    bool isJumping;
    bool isCrouching;
    


    private void Awake()
    {
        playerController = GetComponent<CharacterController>();
        playerAnimator = GameObject.Find("Iddle").GetComponent<Animator>();
        //leanDetectorL = GameObject.FindGameObjectWithTag("LeanDetectorL").GetComponent<LeanDetection>();
        //leanDetectorR = GameObject.FindGameObjectWithTag("LeanDetectorR").GetComponent<LeanDetection>();
        cinemachineFollow = GameObject.FindGameObjectWithTag("Camera").GetComponent<CinemachineOrbitalFollow>();
        ControlPlayer();

    }

    private void ControlPlayer()
    {
        newActions = new InputActions();

        newActions.PlayerMoveSet.Walk.performed += ctx => walk.y = ctx.ReadValue<float>();
        newActions.PlayerMoveSet.Walk.canceled += ctx => walk.y = 0f;

        newActions.PlayerMoveSet.Rotate.performed += ctx => rotate = ctx.ReadValue<float>();
        newActions.PlayerMoveSet.Rotate.canceled += ctx => rotate = 0f;

        newActions.PlayerMoveSet.Run.performed += _ =>
        {
            isRunning = !isRunning;
        };

        newActions.PlayerMoveSet.ResetCamera.performed += _ =>
        {
            ResetCameraValues();
        };

        newActions.PlayerMoveSet.Jump.started += _ => Jump();

        newActions.PlayerMoveSet.Crouch.performed += _ =>
        {
            if(_.control.device is Keyboard)
            {
                isCrouching = true;
                Debug.Log("Está Agachado");
            }
            else if (_.control.device is Gamepad)
            {
                isCrouching = !isCrouching;
                if(isCrouching)
                    Debug.Log("Está Levantado");
                else
                    Debug.Log("Está Agachado");

            }
        };

        newActions.PlayerMoveSet.Crouch.canceled += _ =>
        {
            if (_.control.device is Keyboard)
            {
                isCrouching = false;
                Debug.Log("Está Levantado");
            }
        };

    }


    private void ResetCameraValues()
    {
        if (cinemachineFollow.HorizontalAxis.Value != cinemachineFollow.HorizontalAxis.Center || cinemachineFollow.VerticalAxis.Value != cinemachineFollow.VerticalAxis.Center)
        {
            Debug.Log("cameraHorizontal Value: " + cinemachineFollow.HorizontalAxis.Value + " || CameraVertical Value: " + cinemachineFollow.VerticalAxis.Value);
            Debug.Log("cameraHorizontal Center: " + cinemachineFollow.HorizontalAxis.Center + " || CameraVertical Center: " + cinemachineFollow.VerticalAxis.Center);
            cinemachineFollow.HorizontalAxis.Value = cinemachineFollow.HorizontalAxis.Center;
            cinemachineFollow.VerticalAxis.Value = cinemachineFollow.VerticalAxis.Center;
        }

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        VariablesValue();
    }

    private void VariablesValue()
    {
        gravity = Physics.gravity.y;
        walkExponent = 2.75f;
        initSpeed = 0f;
        actualSpeed = initSpeed;
        walkForwardSpeed = 3.5f;
        walkBackwardSpeed = 2f;
        RunSpeed = 6.5f;

        crouchWalkForward = 2f;
        crouchWalkBackward = 1f;

        rotateSpeed = 0.25f;

        jumpHeight = 1.5f;
        isJumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        SpeedCheck();
        CheckGrounded();

        //Debug.DrawRay(transform.position, Vector3.down * 0.2f, Color.yellow);
        UpdateAnimations();
        MovePlayer();

    }

    private void CheckGrounded()
    {

        if (Physics.Raycast((transform.position - new Vector3(0f, 0.9f, 0f)), Vector3.down, out groundDetect, 0.25f) && fallVelocity < 2.1f && !playerController.isGrounded)
            isJumping = false;

        if (playerController.isGrounded && fallVelocity <= 2f)
        {
            isJumping = false;
        }

    }

    private void MovePlayer()
    {
        //This Vector controls the movement of the player
        Vector3 move = transform.forward * walk.y * actualSpeed;

        //In the if I ensure that gravity works correctly when the player falls
        if (playerController.isGrounded && fallVelocity < 0)
            fallVelocity = -2f;
        else if (!playerController.isGrounded)
        {
            fallVelocity += gravity * Time.deltaTime;
        }

        move.y = fallVelocity;
        
        playerController.Move(move * Time.deltaTime);

        transform.Rotate(Vector3.up * rotate * rotateSpeed * Time.deltaTime * -360f);
    }


    private void Jump()
    {
        if (playerController.isGrounded && !isJumping && !isCrouching)
        {
            isJumping = true;
            playerAnimator.SetTrigger("Jump");
            fallVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
    private void Crouch()
    {
        if (!isCrouching)
        {
            isCrouching = true;
            Debug.Log("Está Agachado");
        }
        else
        {
            isCrouching = false;
            Debug.Log("Levantarse");
        }
    }

    private void UpdateAnimations()
    {
        if(isCrouching)
            playerAnimator.SetFloat("Walk", walk.y, 0.2f, Time.deltaTime);
        else
            playerAnimator.SetFloat("Walk", walk.y);

        playerAnimator.SetBool("Run", isRunning);
        playerAnimator.SetFloat("Rotate", rotate);
        playerAnimator.SetBool("IsGrounded", playerController.isGrounded);
        //playerAnimator.SetBool("LeanToLeft", leanDetectorR.leanToLeft);
        //playerAnimator.SetBool("LeanToRight", leanDetectorL.leanToRight);
        playerAnimator.SetBool("IsJumping", isJumping);
        playerAnimator.SetBool("IsCrouching", isCrouching);

    }

    private void SpeedCheck()
    {
        progressiveWalkSpeed = Mathf.Pow(Mathf.Abs(walk.y), walkExponent) * Mathf.Sign(walk.y) * walkForwardSpeed;

        if (walk.y <= -0.2f)
            actualSpeed = walkBackwardSpeed;
        else if (isRunning && !isCrouching)
            actualSpeed = RunSpeed;
        else if (isCrouching && !isRunning)
        {
            if (walk.y <= -0.2f)
                actualSpeed = crouchWalkBackward;
            else
                actualSpeed = crouchWalkForward;
        }
        else if (!isRunning && !isCrouching && walk.y >= 0.2f)
            actualSpeed = progressiveWalkSpeed;

        /*if (walk.y <= -0.2f)
        {
            actualSpeed = walkBackwardSpeed;
        }
        else if (isRunning)
        {
            actualSpeed = RunSpeed;
        }
        else
        {
            actualSpeed = progressiveWalkSpeed;
        }*/

    }

    private void OnEnable()
    {
        newActions.Enable();
    }

    private void OnDisable()
    {
        newActions.Disable();
    }
}
