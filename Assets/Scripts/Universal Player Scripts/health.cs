using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public Slider slider;
    public float fillSpeed = 100f;
    public SceneLoader sceneLoader;


    void Start()
    {
        currentHealth = maxHealth;
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }
    private void Update()
    {
        // smoothly move slider value towards current health
        if (!Mathf.Approximately(slider.value, currentHealth))
        {
            slider.value = Mathf.MoveTowards(slider.value, currentHealth, fillSpeed * Time.deltaTime);
        }
    }

    // Made public so enemies can access it
    public void UpdateHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        //Debug.Log($"{gameObject.name} health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        Debug.Log($"{gameObject.name} died!");
        sceneLoader.next("LoseScene");
        //Destroy(gameObject);
    }

    public void setMaxHealth(int health)
    {
        slider.value = health;
    }

    public void RestoreMaxHealth()
    {
        currentHealth = maxHealth;
        //slider.value = currentHealth;
        //UpdateHealth(maxHealth);
    }
}

 