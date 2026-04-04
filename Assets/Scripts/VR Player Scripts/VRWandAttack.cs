using UnityEngine;
using System.Collections;

public class VRWandAttack : MonoBehaviour
{
    [Header("References")]
    public VRWandBehaviour wandBehaviour;      // gets the current wand light color
    public VRWandSwing wandSwing;              // checks if the player is actually swinging

    public VRPlayerInteraction vrPlayerInteractionScript;
    public playerInteraction playerInteractionScript; // for boss hits + enemy count
    public progressBar playerProgress;

    [Header("Settings")]
    public float colorMatchThreshold = 0.15f;  // allows small color differences
    public float hitCooldown = 0.25f;          // stops multiple hits instantly

    [Header("VFX")]
    public GameObject hitEffect;

    private float lastHitTime;

    //partcile effect for when enemy gets destroyed gets initialized
    void Start()
    {
        if (hitEffect != null)
            hitEffect.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // stop spam hits
        if (Time.time < lastHitTime + hitCooldown)
            return;

        // only hit enemies
        if (!other.CompareTag("Enemy") && !other.CompareTag("FinalEnemy"))
            return;

        // enemy script
        enemyBehaviour enemy = other.GetComponentInParent<enemyBehaviour>();
        if (enemy == null)
            return;

        Debug.Log("Enemy touched!");

        // make sure the wand is actually swinging
        if (wandSwing == null || !wandSwing.IsSwinging)
        {
            Debug.Log("Not swinging, so no hit.");
            return;
        }

        // get enemy color
        Renderer enemyRenderer = other.GetComponentInChildren<Renderer>();
        if (enemyRenderer == null)
            enemyRenderer = other.GetComponentInParent<Renderer>();

        if (enemyRenderer == null)
            return;

        Color enemyColor;

        if (vrPlayerInteractionScript != null)
            enemyColor = vrPlayerInteractionScript.GetColorFromMaterialName(enemyRenderer);
        else if (playerInteractionScript != null)
            enemyColor = playerInteractionScript.GetColorFromMaterialName(enemyRenderer);
        else
        {
            Debug.LogWarning("No interaction script assigned, so enemy color could not be checked.");
            return;
        }

        // get wand color
        Color wandColor = wandBehaviour.CurrentColor;

        Debug.Log("Enemy color: " + enemyColor);
        Debug.Log("Wand color: " + wandColor);

        // compare colors
        float colorDiff = Vector3.Distance(
            new Vector3(enemyColor.r, enemyColor.g, enemyColor.b),
            new Vector3(wandColor.r, wandColor.g, wandColor.b)
        );

        if (colorDiff < colorMatchThreshold)
        {
            lastHitTime = Time.time;

            // boss enemy
            if (other.CompareTag("FinalEnemy"))
            {
                if (playerInteractionScript != null)
                {
                    playerInteractionScript.HandleBossHit(enemy.gameObject);
                }
                else
                {
                    Debug.LogWarning("playerInteractionScript not assigned, so boss hit was not handled.");
                }
            }
            else
            {
                // spawn particle effect at enemy position
                if (hitEffect != null)
                {
                    // Create new instance of the particle effect at the enemy's position
                    GameObject tempEffect = Instantiate(hitEffect, enemy.transform.position, Quaternion.identity);

                    // Ensures it is active 
                    tempEffect.SetActive(true);

                    // destroy
                    Destroy(tempEffect, 2f);
                }
                AudioManager.Instance.PlaySFX("MagicCasting");
                Destroy(enemy.gameObject);

                if (playerInteractionScript != null)
                    playerInteractionScript.enemiesKilled++;

                if (vrPlayerInteractionScript != null)
                    vrPlayerInteractionScript.enemiesKilled++;

                if (playerProgress != null)
                    playerProgress.UpdateProgress(+10);

                Debug.Log("Correct color + swing! Enemy destroyed.");
            }
        }
        else
        {
            Debug.Log("Wrong color!");
        }
    }

    //particle effect
    IEnumerator DisableEffectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (hitEffect != null)
            hitEffect.SetActive(false);
    }
}