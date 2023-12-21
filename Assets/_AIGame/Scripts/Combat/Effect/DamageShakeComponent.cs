using UnityEngine;

public class DamageShakeComponent : MonoBehaviour
{
    public float shakeDuration = 0.5f; // Length of shake effect
    public float shakeMagnitude = 0.1f; // Intensity of shake effect

    private float currentShakeDuration;
    private Vector3 originalPosition;

    private void Start()
    {
        var healthComponent = GetComponent<HealthComponent>();
        
        healthComponent.OnTakeDamageObservers += StartShake;
        originalPosition = transform.localPosition;
    }

    private void Update()
    {
        if (currentShakeDuration > 0)
        {
            transform.localPosition = originalPosition + Random.insideUnitSphere * shakeMagnitude;
            currentShakeDuration -= Time.deltaTime;
        }
        else
        {
            currentShakeDuration = 0f;
            transform.localPosition = originalPosition;
        }
    }

    private void StartShake()
    {
        currentShakeDuration = shakeDuration;
    }
}