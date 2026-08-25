using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private InputActions newActions;
    CharacterController playerController;

    [SerializeField] Animator playerAnimator;


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


    bool isRunning;


    private void Awake()
    {
        playerController = GetComponent<CharacterController>();
        playerAnimator = GameObject.Find("Iddle").GetComponent<Animator>();
        ControlPlayer();

    }

    private void ControlPlayer()
    {
        newActions = new InputActions();

        newActions.PlayerMoveSet.Walk.performed += ctx => walk.y = ctx.ReadValue<float>();
        newActions.PlayerMoveSet.Walk.canceled += ctx => walk.y = 0f;

        newActions.PlayerMoveSet.Rotate.performed += ctx => rotate = ctx.ReadValue<float>();
        newActions.PlayerMoveSet.Rotate.canceled += ctx => rotate = 0f;

        newActions.PlayerMoveSet.Run.started += _ =>
        {
            isRunning = true;
        };
        newActions.PlayerMoveSet.Run.canceled += _ =>
        {
            isRunning = false;
        };
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
        walkForwardSpeed = 2f;
        walkBackwardSpeed = 1.5f;
        RunSpeed = 4f;

        rotateSpeed = 0.25f;
    }

    // Update is called once per frame
    void Update()
    {
        SpeedCheck();

        UpdateAnimations();
        MovePlayer();
    }

    private void MovePlayer()
    {
        //This Vector controls the movement of the player
        Vector3 move = transform.forward * walk.y * actualSpeed;

        //In the if I ensure that gravity works correctly when the player falls
        if (playerController.isGrounded)
            fallVelocity = -2f;
        else
        fallVelocity += gravity * Time.deltaTime;

        move.y = fallVelocity;

        playerController.Move(move * Time.deltaTime);

        transform.Rotate(Vector3.up * rotate * rotateSpeed * Time.deltaTime * -360f);
    }


    private void UpdateAnimations()
    {
        playerAnimator.SetFloat("Walk", walk.y);
        playerAnimator.SetBool("Run", isRunning);
        playerAnimator.SetFloat("Rotate", rotate);
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
