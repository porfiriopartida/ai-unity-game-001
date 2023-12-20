using UnityEngine;

public class HangController : MonoBehaviour
{
    public PlayerController playerController;
    public LayerMask hangableObjectLayer;
    public float raycastDistance = 2f;

    private HangableObject currentHangableObject;

    void Update()
    {
        CheckForHangableObject();
    }

    private void CheckForHangableObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance, hangableObjectLayer))
        {
            currentHangableObject = hit.collider.GetComponent<HangableObject>();
            // if (playerController.GetVerticalVelocity() > 0)
            // {
            //     // Player is moving upwards and has hit a hangable object, Player can hang now
            playerController.SetMovingStatus(false); // Assuming you've a method to stop player's movement
            // }
        }
        else
        {
            currentHangableObject = null;
            // Player is not under a hangable object, thus player can not hang and should fall down
            playerController.SetMovingStatus(true); // Assuming you've a method to resume player's movement
        }
    }
    // Drawing Gizmos for ground check
    void OnDrawGizmos() 
    {
        Gizmos.color = Color.white;
        Gizmos.DrawRay(transform.position, transform.forward * raycastDistance);
    }
}