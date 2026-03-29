using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class health : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;

    public Slider slider;


    void Start()
    {
        currentHealth = maxHealth;
    }

    // Made public so enemies can access it
    public void UpdateHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        slider.value = currentHealth;

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

    public void setMaxHealth(int health)
    {
        slider.value = health;
    }

    public void RestoreMaxHealth()
    {
        currentHealth = maxHealth;
        slider.value = currentHealth;
    }
}

 