using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private enum MovementState
    {
        Walking,
        Running,
        StaminaRecharge,
        Crouching,
        Airborne
    }

    [SerializeField] private MovementState currentState = MovementState.Walking;
    private MovementState lastState;

    [Header("Walking")]
    public float walkSpeed;


    [Header("Running")]
    public float runSpeed;
    public bool isRunning = false;

    [Header("Stamina Recharge")]
    public float staminaRechargeSpeed;
    private bool staminaNeedRecharge = false;

    [Header("Crouching")]
    public float crouchSpeed;
    public bool isCrouching = false;
    public float crouchHeight;

    [Header("Ariborne")]
    public float jumpHeight;
    public float jumpStaminaCost;
    private bool jump;

    [Header("Stamina")]
    public float maxStamina;
    public float stamina;
    public float staminaDrain;
    public float staminaRecharge;

    [Header("Sound Generation")]
    public float currentDetectionRange;
    public float walkDetectionRange;
    public float runDetectionRange;
    public float staminaRechargeDetectionRange;
    public float crouchDetectionRange;
    public float jumpDetectionRange;

    [Header("Misc")]
    public Camera playerCamera;
    public float currentSpeed;    
    public float gravity;
    public bool isGrounded;
    public bool changedState;

    float horizontalInput;
    float verticalInput;
    public float lookSpeed;
    public float lookXLimit;

    Vector3 moveDirection = Vector3.zero;
    float rotationX;

    public bool canMove = true;

    CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        stamina = maxStamina;
        currentSpeed = walkSpeed;
    }

    private void Update()
    {
        switch (currentState)
        {
            case MovementState.Walking:
                Walking();
                break;
            case MovementState.Running:
                Running();
                break;
            case MovementState.StaminaRecharge:
                StaminaRecharge();
                break;
            case MovementState.Crouching:
                Crouching();
                break;
            case MovementState.Airborne:
                Airborne();
                break;
            default:
                break;
        }

        // Check if we're grounded
        isGrounded = Physics.CheckSphere(transform.position -= new Vector3(0, characterController.height / 2, 0), 0.2f, 1 << 10);
        if (!isGrounded && currentState != MovementState.Airborne)
        {
            ChangeState(MovementState.Airborne);
        }

        GetInputs();

        // Prevent us from moving if game is paused
        if (GameStateManager.currentState == GameStateManager.GameState.Paused && canMove)
        {
            canMove = false;
        }
        else if (GameStateManager.currentState == GameStateManager.GameState.Playing && !canMove)
        {
            canMove = true;
        }

        if (canMove)
        {
            if (isRunning && characterController.velocity.magnitude > 0f)
            {
                HandleStamina(true, 0);
            }
            else
            {
                HandleStamina(false, 0);
            }
            if (isGrounded && characterController.velocity.magnitude > 0f && Time.frameCount % 10 == 0)
            {
                SoundGeneration(currentDetectionRange);
            }
        }

        MovePlayer();
    }

    // Get input from player
    private void GetInputs()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Run"))
        {
            if (ReadyToRun())
            {
                ChangeState(MovementState.Running);
            }
        }
        else if (Input.GetButtonUp("Run") && isRunning)
        {
            ChangeState(MovementState.Walking);
        }

        if (Input.GetButtonDown("Crouch"))
        {
            if (ReadyToCrouch())
            {
                ChangeState(MovementState.Crouching);
            }
        }
        else if (Input.GetButtonUp("Crouch") && isCrouching)
        {
            ChangeState(MovementState.Walking);
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (ReadyToJump())
            {
                jump = true;
            }
        }
    }

    // Apply input to character controller
    private void MovePlayer()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float curSpeedX = canMove ? currentSpeed * verticalInput : 0;
        float curSpeedY = canMove ? currentSpeed * horizontalInput : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (jump && canMove)
        {
            moveDirection.y = jumpHeight * gravity;
            ChangeState(MovementState.Airborne);
            HandleStamina(true, jumpStaminaCost);
            jump = false;

            // Disable run when we jump
            if (isRunning)
            {
                isRunning = false;
            }
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    // Change the current state
    private void ChangeState(MovementState newState)
    {
        if (!isGrounded && currentState == MovementState.Airborne)
        {
            return;
        }
        else
        {
            lastState = currentState;
            currentState = newState;
            changedState = true;
        }

        if (lastState == MovementState.Crouching)
        {
            ResetCrouch();
        }
        else if (lastState == MovementState.Running)
        {
            isRunning = false; ;
        }
    }

    // Handle the different states
    private void Walking()
    {
        if (changedState)
        {
            currentSpeed = walkSpeed;
            currentDetectionRange = walkDetectionRange;
            changedState = false;
        }
    }

    private void Running()
    {
        if (changedState)
        {
            currentSpeed = runSpeed;
            currentDetectionRange = runDetectionRange;
            isRunning = true;
            changedState = false;
        }
    }

    private void StaminaRecharge()
    {
        if (changedState)
        {
            currentSpeed = staminaRechargeSpeed;
            currentDetectionRange = staminaRechargeDetectionRange;
            changedState = false;
        }
    }

    private void Crouching()
    {
        if (changedState)
        {
            currentSpeed = crouchSpeed;
            currentDetectionRange = crouchDetectionRange;
            isCrouching = true;
            //characterController.height -= crouchHeight;
            //transform.position -= new Vector3(0, crouchHeight, 0);
            playerCamera.transform.position -= new Vector3(0, crouchHeight, 0);

            changedState = false;
        }
    }

    private void Airborne()
    {
        if (changedState)
        {
            currentSpeed = walkSpeed;
            changedState = false;
        }

        if (isGrounded)
        {
            SoundGeneration(jumpDetectionRange);
            ChangeState(lastState);
        }
    }

    // Check our ability to perform actions
    private bool ReadyToRun()
    {
        if (isGrounded && !isCrouching && !staminaNeedRecharge)
        {
            return true;
        }
        return false;
    }

    private bool ReadyToCrouch()
    {
        if (isGrounded && !isRunning)
        {
            return true;
        }
        return false;
    }

    private bool ReadyToJump()
    {
        if (isGrounded && stamina > jumpStaminaCost && !staminaNeedRecharge)
        {
            return true;
        }
        return false;
    }

    // Handle Detection
    private void SoundGeneration(float detectionRange)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, 1<<9);
        foreach (Collider collider in colliders)
        {
            collider.gameObject.GetComponentInParent<EnemyController>().HearPlayer();
        }
    }

    // Reset crouch
    private void ResetCrouch()
    {
        isCrouching = false;
        //characterController.height += crouchHeight;
        //transform.position += new Vector3(0, crouchHeight, 0);
        playerCamera.transform.position += new Vector3(0, crouchHeight, 0);
    }

    // Handle stamina
    private void HandleStamina(bool drain, float drainAmount)
    {
        if (drain)
        {
            if (drainAmount != 0)
            {
                stamina -= drainAmount;
                if (stamina < 0)
                {
                    stamina = 0;
                }
            }
            else
            {
                stamina -= staminaDrain * Time.deltaTime;
            }

            if (stamina <= 0)
            {
                staminaNeedRecharge = true;
                ChangeState(MovementState.StaminaRecharge);
            }
        }
        else
        {
            if (stamina < maxStamina)
            {
                stamina += staminaRecharge * Time.deltaTime;
            }

            if (stamina >= maxStamina && staminaNeedRecharge)
            {
                staminaNeedRecharge = false;
                if (currentState == MovementState.StaminaRecharge)
                {
                    ChangeState(MovementState.Walking);
                }
            }
        }
    }
}
