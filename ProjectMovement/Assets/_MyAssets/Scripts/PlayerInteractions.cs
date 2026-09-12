using System;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    PlayerManager playerManager;
    Animator animator;

    bool actionClimb;
    bool actionJump;
    bool actionSneak;
    bool actionOverPass;

    RaycastHit objectDetect;





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        animator = GameObject.Find("Iddle").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Physics.Raycast(transform.position, Vector3.forward, 0.25f, 2);
    }

    public void Interactions()
    {
        if (Physics.Raycast(transform.position, Vector3.forward, out objectDetect, 0.5f))
        {
            if (objectDetect.transform.tag == "ClimbObstacle" && !actionClimb)
            {
                actionClimb = true;
                Debug.Log("EUREKAAA");
            }
            else if (objectDetect.transform.tag == "LowerObstacle" && !actionJump)
            {
                actionJump = true;
                Debug.Log("EUREKAAA");
            }
            else if (objectDetect.transform.tag == "ShortObstacle" && !actionOverPass)
            {
                actionOverPass = true;
            }
            else if (objectDetect.transform.tag == "HighObstacle" && !actionSneak)
            {
                actionSneak = true;
            }
        }
        else
            return;

    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "ClimbObstacle" && !actionClimb)
        {
            actionClimb = true;
        }
        else if (other.gameObject.tag == "LowerObstacle" && !actionJump)
        {
            actionJump = true;
        }
        else if (other.gameObject.tag == "ShortObstacle" && !actionOverPass)
        {
            actionOverPass = true;
        }
        else if (other.gameObject.tag == "HighObstacle" && !actionSneak)
        {
            actionSneak = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "ClimbObstacle" && actionClimb)
        {
            actionClimb = false;
        }
        else if (other.gameObject.tag == "LowerObstacle" && actionJump)
        {
            actionJump = false;
        }
        else if (other.gameObject.tag == "ShortObstacle" && actionOverPass)
        {
            actionOverPass = false;
        }
        else if (other.gameObject.tag == "HighObstacle" && actionSneak)
        {
            actionSneak = false;
        }
        
    }
}
