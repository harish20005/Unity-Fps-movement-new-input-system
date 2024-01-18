using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{

    [SerializeField] Transform playerBody;

    [SerializeField] private float mouseSensitivity;


    public float xRotation = 0f;

    public Vector2 camDirection;

    private PlayerInput playerInput;

    private InputAction look;

    private InputAction keyEscape;


    private void Awake()
    {
        playerInput = new PlayerInput();
    }
    
    private void OnEnable()
    {
        look = playerInput.Player.Look;
        look.Enable();

        keyEscape = playerInput.Player.EscapeKey;
        keyEscape.Enable();
        keyEscape.performed += CursorUnhide;
    }

    private void OnDisable()
    {
        look.Disable();
        keyEscape.Disable();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        GetInput();
        MoveCamera();
    }

    private void GetInput()
    {
        camDirection = look.ReadValue<Vector2>();

        camDirection = camDirection * mouseSensitivity * Time.deltaTime;
    }

    private void MoveCamera()
    {
        xRotation -= camDirection.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * camDirection.x);
    }

    private void CursorUnhide(InputAction.CallbackContext context)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
