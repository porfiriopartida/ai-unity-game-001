using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public delegate void HealthDelegate();
    public event HealthDelegate OnTakeDamageObservers;
    
    public int maxHealth = 100;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Notify observers:
            OnTakeDamageObservers?.Invoke();
        }
    }

    private void Die()
    {
        // Handle death here. For example, you may want to disable the game object:
        gameObject.SetActive(false);
        // You can also display any death effects, play sound effects, trigger a game over screen, etc. here.
    }
}