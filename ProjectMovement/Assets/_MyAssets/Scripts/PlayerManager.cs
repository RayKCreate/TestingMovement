using System;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private InputActions newActions;
    CharacterController playerController;
    [SerializeField] LeanDetection leanDetectorL;
    [SerializeField] LeanDetection leanDetectorR;

    [SerializeField] Animator playerAnimator;


    [SerializeField] CinemachineOrbitalFollow cinemachineFollow;

    Vector2 walk;
    float rotate;

    float gravity;

    float initSpeed;
    float actualSpeed;
    float walkForwardSpeed;
    float progressiveWalkSpeed;
    [SerializeField] float walkExponent;
    float walkBackwardSpeed;
    float RunSpeed;

    float rotateSpeed;


    float fallVelocity;

    RaycastHit groundDetect;

    float cameraHorizontalValue;
    float cameraVerticalValue;


    bool isRunning;
    bool isJumping;

    [SerializeField] float jumpHeight; 

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
        walkBackwardSpeed = 1.5f;
        RunSpeed = 5.5f;

        rotateSpeed = 0.25f;

        jumpHeight = 1.5f;
        isJumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        SpeedCheck();
        CheckGrounded();

        Debug.DrawRay(transform.position, Vector3.down * 0.2f, Color.yellow);
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
        if (playerController.isGrounded && !isJumping)
        {
            isJumping = true;
            playerAnimator.SetTrigger("Jump");
            fallVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void UpdateAnimations()
    {
        playerAnimator.SetFloat("Walk", walk.y);
        playerAnimator.SetBool("Run", isRunning);
        playerAnimator.SetFloat("Rotate", rotate);
        playerAnimator.SetBool("IsGrounded", playerController.isGrounded);
        //playerAnimator.SetBool("LeanToLeft", leanDetectorR.leanToLeft);
        //playerAnimator.SetBool("LeanToRight", leanDetectorL.leanToRight);
        playerAnimator.SetBool("IsJumping", isJumping);

    }

    private void SpeedCheck()
    {
        progressiveWalkSpeed = Mathf.Pow(Mathf.Abs(walk.y), walkExponent) * Mathf.Sign(walk.y) * walkForwardSpeed;

        if (walk.y <= -0.2f)
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
        }

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
