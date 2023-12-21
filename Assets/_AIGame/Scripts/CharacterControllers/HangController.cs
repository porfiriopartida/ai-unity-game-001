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
        if (!(playerController.CurrentState is GroundedState))
        {
            return;
        }
        CheckForHangableObject();
    }

    private void CheckForHangableObject()
    {
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
                    if (Mathf.Abs(hangPoint.transform.position.y - hit.transform.position.y) <= hangThreshold)
                    {
                        // Vector3 hangColliderNormal = hit.normal;
                        playerController.TransitionToState(new HangingState(playerController, hit.point, moveSpeed, hit));
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
}