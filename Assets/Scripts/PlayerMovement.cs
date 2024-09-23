using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private bool isGrounded;
    private PlayerCam playerCam;
    //InputSystem
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;

    //Balance
    [SerializeField] private float moveSpeed = 20;
    [SerializeField] private float jumpForce = 20f;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform orientation;
    [SerializeField] private float rotationSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        jumpAction = playerInput.actions.FindAction("Jump");
    }
    
    void Update()
    {      
        JumpPlayer();
        
        GroundCheck();

        RotateCharacter();
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

        // Apply the move direction, scaled by moveSpeed
        Vector3 moveVelocity = moveDirection.normalized * moveSpeed;

        // Set the velocity for the Rigidbody (leave Y velocity as it is, for gravity and jumping)
        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
    }

    void JumpPlayer()
    {
        if (jumpAction.triggered && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);
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
