using UnityEngine;

public class health : MonoBehaviour
{
    public int maxHealth = 5;
    int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Made public so enemies can access it
    public void UpdateHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log($"{gameObject.name} health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        // Add death logic here
        Debug.Log($"{gameObject.name} died!");
        Destroy(gameObject);
    }
}