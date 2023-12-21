using UnityEngine;

public class HangController : MonoBehaviour
{
    public PlayerController playerController;
    public LayerMask hangableObjectLayer;
    public float raycastDistance = 1.0f;
    public float hangThreshold = 0.5f;  // adjust as needed
    public Transform hangPoint;

    private HangableObject currentHangableObject;
    public float moveSpeed = 2.0f;
    private HangableObject lastLedge;

    void Start()
    {
        // If hangPoint is not assigned, default to current Transform
        if (hangPoint == null)
        {
            hangPoint = transform;
        }
    }

    void Update()
    {
        if (playerController.isGrounded() || playerController.GetVerticalVelocity() > 0)
        {
            lastLedge = null;
        }

        if (!(playerController.CurrentState is GroundedState) || playerController.isGrounded() || playerController.GetVerticalVelocity() > 0)
        {
            return;
        }
        CheckForHangableObject();
    }

    private void CheckForHangableObject()
    {
        // Debug.Log("Checking for Hangable objects.");
        RaycastHit hit;
        if (Physics.Raycast(hangPoint.position, hangPoint.forward, out hit, raycastDistance, hangableObjectLayer))
        {
            // Check if the player is already in a hang state
            if (!(playerController.CurrentState is HangingState))
            {
                HangableObject hangableObject = hit.collider.GetComponent<HangableObject>();
                if (hangableObject != null)
                {
                    // Check if player is close enough to hanging position. 
                    if (Mathf.Abs(hangPoint.transform.position.y - hangableObject.hangingPoint.position.y) <= hangThreshold)
                    {
                        TryToHang(hangableObject, hit);
                    }
                }
            }
        }
        else if (playerController.CurrentState is HangingState)
        {
            // If there's no hangable object in front of the player and the player is in the hanging state.
            // Then transition the player state back to GroundedState or any other appropriate state.
            playerController.TransitionToState(new GroundedState(playerController));
        }
    }
    // Drawing Gizmos for ground check
    void OnDrawGizmos() 
    {
        Gizmos.color = Color.white;
        Gizmos.DrawRay(hangPoint.position, hangPoint.transform.forward * raycastDistance);
    }
    public void TryToHang(HangableObject hangableObject, RaycastHit hit)
    {
        if (hangableObject != lastLedge)
        {
            // Create and transition to new HangingState
            playerController.TransitionToState(new HangingState(playerController, hit.point, moveSpeed, hit));
            this.lastLedge = hangableObject;
        }
    }
}