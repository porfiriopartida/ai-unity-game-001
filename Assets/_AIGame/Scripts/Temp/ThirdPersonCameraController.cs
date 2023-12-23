using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    public Transform target; // Target to follow
    public Vector3 offset; // Offset from the target
    public float smoothSpeed = 0.125f; // Smoothing factor
    void Start()
    {
        if(target != null)
        {
            offset = transform.position - target.position;
        }     
    }
    void Update()
    {
        if(target == null) return;
        Vector3 desiredPosition = target.position + offset;
       // Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
       // transform.position = smoothedPosition;
       transform.position = desiredPosition;

        //transform.LookAt(target);
    }
}