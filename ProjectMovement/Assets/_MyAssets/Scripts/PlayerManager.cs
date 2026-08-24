using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private InputActions controls;
    CharacterController characterController;

    [SerializeField] Animator playerAnimator;



    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimator = GameObject.Find("Iddle").GetComponent<Animator>();
        ControlPlayer();

    }

    private void ControlPlayer()
    {
        controls = new InputActions();


    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
