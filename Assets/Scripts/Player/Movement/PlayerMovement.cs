using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    float activeModifier;

    public bool isMoving;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouch")]
    public float crouchHeightModifier;
    public float crouchSpeedModifier;
    public float crouchCooldown;
    bool readyToCrouch;
    public bool isCrouching;

    public PlayerCrouch playerCrouch;
    public CapsuleCollider capsuleCollider;

    [Header("Run")]
    public float runSpeedModifier;
    public float maxStamina;
    public float staminaDrain;
    public float staminaRegen;
    public float stamina;
    bool readyToRun;
    public bool isRunning;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        readyToCrouch = true;
        isCrouching = false;

        stamina = maxStamina;
        readyToRun = true;
        isRunning = false;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();

        if (MathF.Abs(verticalInput + horizontalInput) > 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        SpeedControl();

        // handle drag
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

        HandleStamina();

        if (isRunning)
        {
            if (!RunCheck())
            {
                Run();
            }
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetButtonDown("Jump") && readyToJump && grounded)
        {
            readyToJump = false;

            if (isCrouching)
            {
                Crouch();
            }


            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // when to crouch
        if (Input.GetButtonDown("Crouch") && grounded && readyToCrouch && !isRunning)
        {
            Crouch();
        }
        if (Input.GetButtonUp("Crouch"))
        {
            if (isCrouching)
            {
                Crouch();
            }
        }

        // when to run
        if (Input.GetButtonDown("Run") && grounded && readyToRun && RunCheck() && !isCrouching)
        {
            Run();
        }
        if (Input.GetButtonUp("Run"))
        {
            if (isRunning)
            {
                Run();
            }
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (grounded)
        {
            // while crouching
            if (isCrouching)
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * crouchSpeedModifier, ForceMode.Force);
            }
            else if (isRunning)
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * runSpeedModifier, ForceMode.Force);
            }
            else
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            }
        }

        // in air
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        if (isCrouching)
        {
            activeModifier = moveSpeed * crouchSpeedModifier;
        }
        else if (isRunning)
        {
            activeModifier = moveSpeed * runSpeedModifier;
        }
        else
        {
            activeModifier = moveSpeed;
        }


        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > activeModifier)
        {
            Vector3 limitedVel = flatVel.normalized * activeModifier;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }

    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void Crouch()
    {
        if (!isCrouching)
        {
            readyToCrouch = false;

            capsuleCollider.height = playerHeight * crouchHeightModifier;
            capsuleCollider.center = new Vector3(0, -(playerHeight * crouchHeightModifier)/2, 0);
        }
        else if (isCrouching)
        {
            capsuleCollider.height = playerHeight;
            capsuleCollider.center = Vector3.zero;

            Invoke(nameof(ResetCrouch), crouchCooldown);
        }
        isCrouching = playerCrouch.Crouch(isCrouching, playerHeight, crouchHeightModifier);
    }

    private void ResetCrouch()
    {
        readyToCrouch = true;
    }

    private void Run()
    {
        if (!isRunning)
        {
            readyToRun = false;
            isRunning = true;
        }
        else if (isRunning)
        {
            isRunning = false;
        }
    }

    private bool RunCheck()
    {
        if (verticalInput > 0)
        {
            return true;
        }
        return false;
    }

    private void ResetRun()
    {
        readyToRun = true;
    }

    private void HandleStamina()
    {
        if (isRunning)
        {
            stamina -= staminaDrain * Time.deltaTime;
        }
        else if (!isRunning)
        {
            if (stamina < maxStamina)
            {
                stamina += staminaRegen * Time.deltaTime;
            } 
        }

        if (stamina <= 0 && isRunning)
        {
            Run();
        }

        if (stamina > maxStamina * 0.1f && !isRunning && !readyToRun)
        {
            ResetRun();
        }
    }
}
