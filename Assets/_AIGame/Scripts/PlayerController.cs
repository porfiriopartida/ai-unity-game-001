using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
        public float jumpHeight = 2.0f;
        public Transform groundCheck;
        public float checkDistance = 0.2f;
        public LayerMask groundLayer;
        public bool canDoubleJump = true;
        public float jumpBufferTime = 0.2f;
    
        private CharacterController controller;
        private Camera mainCamera;
        private bool isGrounded;
        private string groundObjectName = "N/A";
        private int jumpCount = 0;
        private bool jumpPressed;
        private float lastJumpPressedTime = -1f;
        private Vector3 movement;
        private float verticalVelocity;
        private float jumpPressedTime = 0f;
        public float runSpeed = 10.0f;
        public float acceleration = 2.0f;

        private float currentSpeed;
    
        public bool canMove = true;
        private Vector3 direction = Vector3.zero; 
        private void Start()
        {
            controller = GetComponent<CharacterController>();
            mainCamera = Camera.main;
            currentSpeed = speed;
        }

        // private bool isGrounded()
        // {
        //     return controller.isGrounded || _isGrounded;
        // }
        void Update()
        {
            RaycastHit hit;
            bool wasGrounded = isGrounded;
            isGrounded = Physics.Raycast(groundCheck.position, -Vector3.up, out hit, checkDistance, groundLayer);
    
            if (isGrounded && verticalVelocity <= 0)
            {
                verticalVelocity = -2.0f; // Small value to keep player grounded
            }
            else if (!wasGrounded && isGrounded && verticalVelocity > 0)
            {
                // Buffer jump
                if (Time.time - jumpPressedTime <= jumpBufferTime)
                {
                    jumpCount = 0;
                    Jump();
                }
            }
            else if (!isGrounded)
            {
                verticalVelocity += Physics.gravity.y * Time.deltaTime;
            }

            if (jumpPressed && (isGrounded || (!isGrounded && canDoubleJump && jumpCount < 2)))
            {
                Jump();
                jumpPressed = false;
            }

            if (!canMove)
            {
                //TODO: Update for state pattern instead for hanging and ground locomotions.
                return;
            }
            // Moving the player
            movement.y = verticalVelocity;
            controller.Move(movement * Time.deltaTime);
        }
    
        public void Move(float horizontalAxis, float verticalAxis)
        {
            if (!canMove)
            {
                return;
            }
            Vector3 moveHorizontalDirection = mainCamera.transform.right * horizontalAxis;
            Vector3 moveVerticalDirection = mainCamera.transform.forward * verticalAxis;
    
            moveHorizontalDirection.y = 0;
            moveVerticalDirection.y = 0;
    
            moveHorizontalDirection.Normalize();
            moveVerticalDirection.Normalize();
    
            // Horizontal movement
            // movement = (moveHorizontalDirection + moveVerticalDirection) * speed;
            movement = (moveHorizontalDirection + moveVerticalDirection) * currentSpeed;
    
            if (horizontalAxis != 0 || verticalAxis != 0)
            {
                direction = new Vector3(movement.x, 0, movement.z).normalized;
            }
        
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, speed * Time.deltaTime);
            //jumpPressed = false;  // Reset for next input check
        }
    
        public void Jump()
        {
            if (controller.isGrounded || (!controller.isGrounded && canDoubleJump && jumpCount < 2))
            {
                jumpPressed = true;
                jumpPressedTime = Time.time;
                lastJumpPressedTime = Time.time;
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
                jumpCount++;
            }
        }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 20), $"Player Y position: {transform.position.y}");
        GUI.Label(new Rect(10, 30, 300, 20), $"Ground Object: {groundObjectName}");
        GUI.Label(new Rect(10, 50, 300, 20), $"Current Speed: {currentSpeed}");
    }

    // Drawing Gizmos for ground check
    void OnDrawGizmos() 
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(groundCheck.position, -Vector3.up * checkDistance);
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * 2);
    }

    public void StopMoving()
    {
        movement.x = 0;
        movement.z = 0;
        Debug.Log("Stop");
    }

    public void Run(bool isRunning)
    {
        currentSpeed = Mathf.Lerp(currentSpeed, isRunning ? runSpeed : speed, acceleration * Time.deltaTime);
    }
    public void SetMovingStatus(bool status)
    {
        canMove = status;
    }
}