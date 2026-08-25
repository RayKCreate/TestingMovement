using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private InputActions newActions;
    CharacterController playerController;

    [SerializeField] Animator playerAnimator;


    Vector2 walk;

    float gravity;

    float initSpeed;
    float actualSpeed;
    float walkForwardSpeed;
    float walkBackwardSpeed;
    float RunSpeed;

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
        initSpeed = 0f;
        actualSpeed = initSpeed;
        walkForwardSpeed = 2f;
        walkBackwardSpeed = 1f;
        RunSpeed = 3f;
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
    }


    private void UpdateAnimations()
    {
        playerAnimator.SetFloat("Walk", walk.y);
        playerAnimator.SetBool("Run", isRunning);
    }

    private void SpeedCheck()
    {
        AnimatorStateInfo playerInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
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
            actualSpeed = walkForwardSpeed;
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
