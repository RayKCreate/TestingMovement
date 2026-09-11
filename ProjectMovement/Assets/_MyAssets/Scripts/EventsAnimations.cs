using System;
using Unity.VisualScripting;
using UnityEngine;

public class EventsAnimations : MonoBehaviour
{
    PlayerManager playerManager;
    Animator playerAnimator;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();

        Variables();
    }

    private void Variables()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CrouchCharacterHeight()
    {
    }

    public void RollReset()
    {
        if (playerManager.isRolling)
            playerManager.isRolling = false;
    }
}
