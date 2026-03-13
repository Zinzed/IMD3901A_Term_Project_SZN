using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class health : MonoBehaviour
{
    public int maxHealth = 5;
    int currentHealth;

    public TextMeshProUGUI healthText;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthText();
    }

    // Made public so enemies can access it
    public void UpdateHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UpdateHealthText();
        //Debug.Log($"{gameObject.name} health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        Debug.Log($"{gameObject.name} died!");
        //Destroy(gameObject);
    }

    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = $"Lives: {currentHealth}";
        }
        else
        {
            Debug.LogError("HealthText reference missing!");
        }
    }
}