using _AIGame.Scripts.States;
using UnityEngine;

public class AirState : IPlayerState, IRunnableState
{
    public float speed = 5.0f;
    public float jumpHeight = 2.0f;
    public float checkDistance = 0.2f;
    public bool canDoubleJump = true;
    public float jumpBufferTime = 0f;
    private Camera mainCamera;
    private bool isGrounded;
    private string groundObjectName = "N/A";
    private int jumpCount = 0;
    private bool jumpPressed;
    private float lastJumpPressedTime = -1f;
    private Vector3 movement;
    private float jumpPressedTime = 0f;
    public float runSpeed = 10.0f;
    public float acceleration = 2.0f;

    private float currentSpeed;
    
    public bool canMove = true;
    private Vector3 direction = Vector3.zero; 
    private readonly PlayerController playerController;
    
    public AirState(PlayerController playerController)
    {
        this.playerController = playerController;
        mainCamera = Camera.main;
        currentSpeed = speed;
        SetupMotionParameters();
    }

    public void SetupMotionParameters()
    {
        speed = playerController.motionParameters.speed;
        jumpHeight = playerController.motionParameters.jumpHeight;
        jumpBufferTime = playerController.motionParameters.jumpBufferTime;
        runSpeed = playerController.motionParameters.runSpeed;
        acceleration = playerController.motionParameters.acceleration;
    }

    public void Enter() { }

    public void HandleUpdate()
    {
        
        if (!canMove)
        {
            //TODO: Update for state pattern instead for hanging and ground locomotions.
            return;
        }
        
        RaycastHit hit;
        bool wasGrounded = isGrounded;
        isGrounded = Physics.Raycast(playerController.groundCheck.position, Vector3.down, out hit, checkDistance, playerController.groundLayer);
        
        if (isGrounded && playerController.verticalVelocity <= 0 || playerController.isGrounded())
        {
            playerController.verticalVelocity = -2.0f; // Small value to keep player grounded
            playerController.TransitionToState(new GroundedState(playerController));
        }
        else if (!isGrounded)
        {
            playerController.verticalVelocity += Physics.gravity.y * Time.deltaTime;
            if (playerController.verticalVelocity < playerController.motionParameters.maxFallingSpeed)
            {
                //TODO: Update configurable capped falling speed.
                playerController.verticalVelocity = playerController.motionParameters.maxFallingSpeed;
            }
            
        }
        // Moving the player
        movement.y = playerController.verticalVelocity;
        playerController.controller.Move(movement * Time.deltaTime);
    }

    public void Move(float horizontal, float vertical)
    {
        if (!canMove)
        {
            return;
        }

        // Debug.Log($"{horizontal} - {vertical}");

        Vector3 direction = CameraUtils.CalculateDirectionFromCamera(horizontal, vertical, Camera.main);
        // Vector3 moveHorizontalDirection = mainCamera.transform.right * horizontal;
        // Vector3 moveVerticalDirection = mainCamera.transform.forward * vertical;
        //
        direction.y = 0;
        // moveVerticalDirection.y = 0;
        //
        // moveHorizontalDirection.Normalize();
        // moveVerticalDirection.Normalize();
        //
        // Horizontal movement
        // movement = (moveHorizontalDirection + moveVerticalDirection) * speed;
        movement = (direction) * currentSpeed;
        //
        if (horizontal != 0 || vertical != 0)
        {
            direction = new Vector3(movement.x, 0, movement.z).normalized;
        }
    
        // Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
        // transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, speed * Time.deltaTime);
        playerController.transform.rotation = Quaternion.LookRotation(direction);
        //jumpPressed = false;  // Reset for next input check
    }


    public void Run(bool isRunning)
    {
        currentSpeed = Mathf.Lerp(currentSpeed, isRunning ? runSpeed : speed, acceleration * Time.deltaTime);
    }
    public void SetMovingStatus(bool status)
    {
        canMove = status;
    }


    public float GetVerticalVelocity()
    {
        return playerController.verticalVelocity;
    }
    public void StopMoving()
    {
        movement.x = 0;
        movement.z = 0;
        Debug.Log("Stop");
    }
    
    public void Exit() { }
    public override string ToString()
    {
        return "AirState[]";
    }
}