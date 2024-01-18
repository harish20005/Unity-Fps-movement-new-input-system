/* This is Fps Character Controller Script With new Input System Made By: Harish */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]

public class InputManager : MonoBehaviour
{

    //Main Variables of this Script

    [Header("Move Variables")]
    [SerializeField] [Range(1,10)] private int walkSpeed;
    [SerializeField] [Range(1,100)] private int runSpeed;
    [SerializeField] [Range(0f,1f)] private float crouchHeight;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private int jumpHeight = 2;
    [SerializeField] private float groundDistance = 0.4f;

    /* [Header("Layers")]
    [SerializeField] private LayerMask groundMask; */

    [Header("Components")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform groundCheck;

    






    private Vector2 movementDirection;
    private Vector3 movePlayer;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isLShiftPressed;
    private bool isCrouchPressed;

    
    //New Input System Variable    
    private PlayerInput playerInput;    

    private InputAction move;
    private InputAction jump;
    private InputAction sprint;
    private InputAction crouch;
    private InputAction fire;


    private void Awake()
    {
        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        move = playerInput.Player.Move;
        move.Enable();

        jump = playerInput.Player.Jump;
        jump.Enable();
        jump.performed += Jump;

        sprint = playerInput.Player.Sprint;
        sprint.Enable();
        sprint.started += PlayerSprintBool;
        sprint.canceled += PlayerSprintBool;

        crouch = playerInput.Player.Crouch;
        crouch.Enable();
        crouch.started += PlayerCrouchBool;
        crouch.canceled += PlayerCrouchBool;
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        sprint.Disable();
        crouch.Disable();
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        GetPlayerMoveInput();
        PlayerMove();
        BooleansControl();
        VelocityControl();
        PlayerRun();
        PlayerCrouch();
    }

    private void GetPlayerMoveInput()
    {
        movementDirection = move.ReadValue<Vector2>();
        movePlayer = transform.right * movementDirection.x + transform.forward * movementDirection.y;
    }

    private void PlayerMove()
    {
        controller.Move(movePlayer * walkSpeed * Time.deltaTime);

        controller.Move(velocity * Time.deltaTime); 
    }

    private void BooleansControl()
    {
        // Debug.DrawRay(groundCheck.position, Vector3.down * groundDistance, Color.blue);

        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, groundDistance);
    }

    private void VelocityControl()
    {
        velocity.y += gravity * Time.deltaTime;

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if(isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void PlayerSprintBool(InputAction.CallbackContext context)
    {
        isLShiftPressed = !isLShiftPressed;
    }

    private void PlayerRun()
    {
        if(isLShiftPressed)
        {
            controller.Move(movePlayer * runSpeed * Time.deltaTime);
        }
        else
        {
            controller.Move(movePlayer * walkSpeed * Time.deltaTime);
        }
    }

    private void PlayerCrouchBool(InputAction.CallbackContext context)
    {
        isCrouchPressed = !isCrouchPressed;
    }

    private void PlayerCrouch()
    {
        if(isCrouchPressed)
        {
            transform.localScale = new Vector3(1, crouchHeight, 1);
            controller.Move(movePlayer * crouchSpeed * Time.deltaTime);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
            controller.Move(movePlayer * walkSpeed * Time.deltaTime);
        }
    }


}
