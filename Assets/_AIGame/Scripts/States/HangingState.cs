using UnityEngine;

public class HangingState : IPlayerState
{
    private readonly PlayerController playerController;
    private readonly Vector3 hangPosition;
    private float moveSpeed;
    
    private float climbThreshold = 0.8f; // INCREASE this threshold to make it HARDER to move from hanging to climbing
    private float moveAlongEdgeThreshold;
    
    private RaycastHit hit;

    public HangingState(PlayerController playerController, Vector3 hangPosition, float moveSpeed,
        RaycastHit hit)
    {
        moveAlongEdgeThreshold = 2*(1 - climbThreshold);
        this.playerController = playerController;
        this.hangPosition = hangPosition;
        this.moveSpeed = moveSpeed;
        this.hit = hit;
    }
    public void Enter()
    {
        var transform = playerController.transform;
        var newPosition = transform.position;
        newPosition.y = hangPosition.y;
        transform.position = newPosition;

        // Calculate rotation needed to face hanging edge
        //Quaternion desiredRotation = Quaternion.LookRotation(hangingEdgeDirection, Vector3.up);
        Vector3 projectedNormal = Vector3.ProjectOnPlane(hit.normal, Vector3.up);
        Quaternion desiredRotation = Quaternion.LookRotation(-projectedNormal);

        // Set character's rotation to face hanging edge
        playerController.transform.rotation = desiredRotation;
    }

    public void SetMovingStatus(bool newStatus)
    {
    }

    public float GetVerticalVelocity()
    {
        return 0;
    }

    public void StopMoving()
    {
        // movement.x = 0;
        // movement.z = 0;
        // Debug.Log("Stop Moving while Hanging.");
    }

    public void HandleUpdate()
    {
        // Hanging logic here
    }

    public void Exit()
    {
        // Any logic that needs to be carried out when exiting the hanging state goes here
    }
    public void Move(float horizontal, float vertical)
    {
        // Input with respect to camera's forward direction 
        Vector3 inputDirection = CameraUtils.CalculateDirectionFromCamera(horizontal, vertical, Camera.main);
        inputDirection.y = 0;
        inputDirection.Normalize();
        
        // Get the direction vector along the hanging edge
        Vector3 alongEdgeDirection = Vector3.Cross(hit.normal, Vector3.up);
        alongEdgeDirection.y = 0f; // Ensure the direction is perfectly horizontal
        alongEdgeDirection.Normalize(); 
        
        float dotWithNormal = Vector3.Dot(inputDirection, hit.normal); 
        Vector3 crossProduct = Vector3.Cross(inputDirection, hit.normal);
        if (Mathf.Abs(dotWithNormal) <= moveAlongEdgeThreshold) // Move along edge
        {
            Debug.DrawRay(hit.point, inputDirection, Color.white,
                2f); // Draws a red line for the along edge direction

            if (crossProduct.y < 0)
            { 
                // Debug.Log("Moving to the left along the edge"); 
                // Move the character to the left along the edge
                playerController.transform.position -= alongEdgeDirection.normalized * (moveSpeed * Time.deltaTime);
            }
            else if (crossProduct.y > 0)
            {
                // Debug.Log("Moving to the right along the edge");
                // Move the character to the right along the edge
                playerController.transform.position += alongEdgeDirection.normalized * (moveSpeed * Time.deltaTime);
            }
        }
        else if(Mathf.Abs(dotWithNormal) > climbThreshold)
        {
            Debug.Log($"dotWithNormal: {dotWithNormal}");
            if (dotWithNormal < climbThreshold)
            {
                Debug.Log("Climb");
                Debug.DrawRay(hit.point, inputDirection, Color.red,
                    2f); // Draws a red line for the along edge direction
            }
            else if (dotWithNormal > -climbThreshold)
            {
                Debug.Log("Drop");
                Debug.DrawRay(hit.point, inputDirection, Color.cyan,
                    2f); // Draws a red line for the along edge direction
                Drop();
            }
        } else {
            Debug.DrawRay(hit.point, inputDirection, Color.black,
                2f); // Draws a red line for the along edge direction
        }
    }
    private void Drop()
    {
        playerController.TransitionToState(new GroundedState(playerController));
    }
}