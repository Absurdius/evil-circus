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

    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float staminaRechargeSpeed = 3f;
    public float crouchSpeed = 3f;
    public float currentSpeed;
    public float jumpHeight = 7f;
    public float jumpStaminaCost = 20f;
    private bool jump;
    public float gravity = 10f;
    bool isRunning = false;
    bool isCrouching = false;
    public float crouchHeight = 1f;
    public bool isGrounded;
    public Transform groundCheck;
    UIStateManager stateManager;
    public AudioSource[] audioSources;
    public AudioClip[] audioClips;

    public float maxStamina = 100f;
    public float stamina;
    public float staminaDrain;
    public float staminaRecharge;
    private bool staminaNeedRecharge = false;

    float horizontalInput;
    float verticalInput;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0f;

    public bool canMove = true;

    CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        stateManager = GameObject.FindWithTag("StateManager").GetComponent<UIStateManager>();

        stamina = maxStamina;
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

        GetInputs();

        // Prevent us from moving if game is paused
        if (stateManager.currentState == UIStateManager.UIState.PAUSED && canMove)
        {
            canMove = false;
            foreach (AudioSource audioSource in audioSources)
            {
                audioSource.Stop();
            }
        }
        else if (stateManager.currentState == UIStateManager.UIState.PLAYING && !canMove)
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

            Footsteps();
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
            Run();
        }
        else if (Input.GetButtonUp("Run") && isRunning)
        {
            Run();
        }

        if (Input.GetButtonDown("Crouch"))
        {
            Crouch();
        }
        else if (Input.GetButtonUp("Crouch") && isCrouching)
        {
            Crouch();
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
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
        currentState = newState;
    }

    // Handle the different states
    private void Walking()
    {
        if (currentSpeed != walkSpeed)
        {
            currentSpeed = walkSpeed;
        }

    }

    private void Running()
    {
        if (currentSpeed != runSpeed)
        {
            currentSpeed = runSpeed;
        }
    }

    private void StaminaRecharge()
    {
        if (currentSpeed != staminaRechargeSpeed)
        {
            currentSpeed = staminaRechargeSpeed;
        }

        if (!audioSources[1].isPlaying && staminaNeedRecharge)
        {
            audioSources[1].Play();
        }
    }

    private void Crouching()
    {
        if (currentSpeed != crouchSpeed)
        {
            currentSpeed = crouchSpeed;
        }
    }

    private void Airborne()
    {
        if (currentSpeed != walkSpeed)
        {
            currentSpeed = walkSpeed;
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
        if (isGrounded && stamina > jumpStaminaCost && !isCrouching && !staminaNeedRecharge)
        {
            return true;
        }
        return false;
    }

    // Handle run, jump and crouch

    private void Run()
    {
        if (!isRunning && ReadyToRun())
        {
            isRunning = true;
            ChangeState(MovementState.Running);
        }
        else if (isRunning)
        {
            isRunning = false;
            if (staminaNeedRecharge)
            {
                ChangeState(MovementState.StaminaRecharge);
            }
            else
            {
                ChangeState(MovementState.Walking);
            }
        }
    }

    private void Crouch()
    {
        if (!isCrouching && ReadyToCrouch())
        {
            isCrouching = true;
            characterController.height -= crouchHeight;
            playerCamera.transform.position -= new Vector3(0, crouchHeight, 0);
            ChangeState(MovementState.Crouching);
        }
        else if (isCrouching)
        {
            isCrouching = false;
            characterController.height += crouchHeight;
            playerCamera.transform.position += new Vector3(0, crouchHeight, 0);
            if (staminaNeedRecharge)
            {
                ChangeState(MovementState.StaminaRecharge);
            }
            else
            {
                ChangeState(MovementState.Walking);
            }
        }
    }

    private void Jump()
    {
        if (ReadyToJump())
        {
            jump = true;
        }
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

                if (audioSources[1].isPlaying)
                {
                    audioSources[1].Stop();
                }
            }
        }
    }

    // Handle audio
    private void Footsteps()
    {
        if (characterController.velocity.magnitude > 0f && isGrounded)
        {
            if (currentState == MovementState.Running && (audioSources[0].clip == audioClips[1] || !audioSources[0].isPlaying))
            {
                audioSources[0].clip = audioClips[0];
                audioSources[0].Play();
            }
            else if (currentState != MovementState.Running && (audioSources[0].clip == audioClips[0] || !audioSources[0].isPlaying))
            {
                audioSources[0].clip = audioClips[1];
                audioSources[0].Play();
            }
        }
        else
        {
            audioSources[0].Stop();
        }
    }
}
