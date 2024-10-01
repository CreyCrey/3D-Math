using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public PlayerData Data;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    private bool isGrounded;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private float groundDrag;
    private float currentMoveSpeed;
    private float currentJumpForce;

    [SerializeField] private float rotationSpeed; 
    [SerializeField] private Transform orientation;  
    [SerializeField] private GameObject player;

    //InputSystem
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;

    private Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //Input system
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        jumpAction = playerInput.actions.FindAction("Jump");

        currentMoveSpeed = moveSpeed;
        currentJumpForce = jumpForce;
    }
    
    void Update()
    {
        JumpPlayer();

        RotateCharacter();

        GroundCheck();
    }

    private void FixedUpdate()
    {
        MovePlayer();      
    }

    void MovePlayer()
    {
        // Read the player's input (X for sideways, Y for forward/backward)
        Vector2 input = moveAction.ReadValue<Vector2>();

        // Convert the 2D input into a 3D direction relative to where the player is facing
        Vector3 moveDirection = orientation.right * input.x + orientation.forward * input.y;

        //Move player
        rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);
    }

    void JumpPlayer()
    {
        if (jumpAction.triggered)
        {
            Debug.Log(isGrounded);
        }

        if (jumpAction.triggered && isGrounded)
        {
            //Jump player
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            Debug.Log("Jump");
        }
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);

        // handle drag
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    void RotateCharacter()
    {
        // Calculate the desired rotation based on the orientation's direction
        Quaternion targetRotation = Quaternion.LookRotation(orientation.forward, Vector3.up);

        //MAKES IT LESS SNAPPY, BUT THE BODY FALLS BEHIND IN ROTATION
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        transform.rotation = targetRotation;
    }
}
