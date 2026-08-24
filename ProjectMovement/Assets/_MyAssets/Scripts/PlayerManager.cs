using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputSystem_Actions inputActions;


    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions = GetComponent<InputSystem_Actions>();




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
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}
