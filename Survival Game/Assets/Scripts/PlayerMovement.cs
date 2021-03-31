using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float playerHeight = 2f;
    public bool canMove = true;

    [SerializeField] Transform orientation;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float movementMultiplier = 10f;
    [SerializeField] private float airMovementMultiplier = .2f;

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float acceleration = 10f;

    [Header("Jumping")]
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float gravity = 9.81f;

    [Header("KeyBindings")]
    [SerializeField] ScriptableObject keybinds;

    [Header("Drag")]
    [SerializeField] private float groundDrag = 6f;
    [SerializeField] private float airDrag = 2f;

    float horizontalMovement;
    float verticalMovement;

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    bool isGrounded;
    float groundDistance = 0.1f;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    Rigidbody rb;

    RaycastHit slopeHit;

    bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2f + 0.5f))
            if (slopeHit.normal != Vector3.up)
                return true;
            else
                return false;
        return false;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        MyInput();
        ControlDrag();
        ControlSpeed();
        
        if (canMove)
        {
            if (InputManager.instance.GetKeyDown("jump") && isGrounded)
                Jump();
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    void MyInput()
    {
        float forward, backward, right, left;
        
        _ = InputManager.instance.GetKey("forward") ? forward = 1 : forward = 0;
        _ = InputManager.instance.GetKey("backward") ? backward = -1 : backward = 0;
        _ = InputManager.instance.GetKey("right") ? right = 1 : right = 0;
        _ = InputManager.instance.GetKey("left") ? left = -1 : left = 0;

        if (!canMove)
        {
            forward = 0;
            backward = 0;
            right = 0;
            left = 0;
        }
        
        verticalMovement = forward + backward;
        horizontalMovement = right + left;
        
        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    void ControlSpeed()
    {
        if (verticalMovement > 0 && InputManager.instance.GetKey("run") && isGrounded)
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
        else
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
    }

    void ControlDrag()
    {
        if (isGrounded)
            rb.drag = groundDrag;
        else
            rb.drag = airDrag;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if (isGrounded && !OnSlope())
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        else if (isGrounded && OnSlope())
        {
            rb.useGravity = false;
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.useGravity = true;
            rb.AddForce(moveDirection.normalized * moveSpeed * airMovementMultiplier, ForceMode.Acceleration);
            rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
        }
    }
}
