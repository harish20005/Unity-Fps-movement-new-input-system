using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    private CharacterController controller;

    [SerializeField] [Range(0f, 10f)] private float walkSpeed = 5f;
    [SerializeField] [Range(0f, 5f)]private float jumpHeight = 3f;
    private Vector3 velocity;
    [SerializeField] private float gravity = -9.81f;
    private bool isGrounded;
    [SerializeField] private Transform grouncheck;
    [SerializeField] [Range(0f, 0.5f)] private float groundDistance;

    private InputController playerInput;

    private InputAction move;
    private InputAction jump;

    private void OnEnable() {
        move = playerInput.Player.Movement;
        move.Enable();
        jump = playerInput.Player.Jump;
        jump.performed += PlayerJump;
        jump.Enable();
    }

    private void OnDisable() {
        move.Disable();
        jump.Disable();
    }

    private void Awake() {
        playerInput = new InputController();
        controller = GetComponent<CharacterController>();
    }

    private void Update() {
        BooleansControls();
        VelocityControl();
        PlayerMove();
    }
    private void PlayerMove()
    {
        Vector2 moveDirection = move.ReadValue<Vector2>();
        Vector3 movePlayer = transform.right * moveDirection.x + transform.forward * moveDirection.y;

        controller.Move(movePlayer * walkSpeed * Time.deltaTime);

        controller.Move(velocity * Time.deltaTime);
    }
    private void PlayerJump(InputAction.CallbackContext context)
    {
        if(isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
    private void VelocityControl()
    {
        velocity.y += gravity * Time.deltaTime;

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }
    private void BooleansControls()
    {
        isGrounded = Physics.Raycast(grouncheck.position, Vector3.down, groundDistance);
    }
}
