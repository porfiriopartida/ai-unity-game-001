using UnityEngine;

public static class CameraUtils
{
    public static Vector3 CalculateDirectionFromCamera(float horizontal, float vertical, Camera camera)
    {
        // Calculate the forward and right vectors of the camera's perspective
        Vector3 forward = camera.transform.forward;
        Vector3 right = camera.transform.right;

        // Remove any vertical (y) component from these vectors
        forward.y = 0f;
        right.y = 0f;

        // Normalize vectors so they have a magnitude of 1
        forward.Normalize();
        right.Normalize();

        // Calculate the desired direction relative to the camera's perspective
        Vector3 desiredDirection = forward * vertical + right * horizontal;

        return desiredDirection.normalized; // Ensure the resulting direction vector also has a magnitude of 1
    }
}