using UnityEngine;

public class DamageShakeComponent : MonoBehaviour
{
    public float shakeDuration = 0.5f; // Length of shake effect
    public float shakeMagnitude = 0.1f; // Intensity of shake effect
    public float restoreSpeed = 1.0f; // Speed of restoring to the original position

    private float currentShakeDuration;
    private Vector3 originalPosition;
    private bool isShaking;

    private void Start()
    {
        var healthComponent = GetComponent<HealthComponent>();
        healthComponent.OnTakeDamageObservers += StartShake;
    }

    private void Update()
    {
        if (isShaking)
        {
            transform.localPosition = originalPosition + Random.insideUnitSphere * shakeMagnitude;
            currentShakeDuration -= Time.deltaTime;
            if (currentShakeDuration <= 0)
            {
                isShaking = false;
            }
        }
        // else 
        // {
        //     // Smoothly move back to the original position when not shaking
        //     transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, restoreSpeed * Time.deltaTime);
        // }
    }

    private void StartShake()
    {
        originalPosition = transform.localPosition;
        currentShakeDuration = shakeDuration;
        isShaking = true;
    }
}