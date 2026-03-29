using UnityEngine;

public class VRWandAttack : MonoBehaviour
{
    [Header("References")]
    public VRWandBehaviour wandBehaviour;      // gets the current wand light color
    public VRWandSwing wandSwing;              // checks if the player is actually swinging
    public playerInteraction playerInteractionScript; // for boss hits + enemy count
    public progressBar playerProgress;

    [Header("Settings")]
    public float colorMatchThreshold = 0.15f;  // allows small color differences
    public float hitCooldown = 0.25f;          // stops multiple hits instantly

    private float lastHitTime;

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

        Color enemyColor = enemyRenderer.material.GetColor("_BaseColor");

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
                Destroy(enemy.gameObject);

                if (playerInteractionScript != null)
                {
                    playerInteractionScript.enemiesKilled++;

                    if (playerProgress != null)
                    {
                        playerProgress.UpdateProgress(+1); // Increment progress
                    }
                    else
                    {
                        Debug.LogError("Player has no progress script!"); // Debug missing component
                    }
                }

                Debug.Log("Correct color + swing! Enemy destroyed.");
            }
        }
        else
        {
            Debug.Log("Wrong color!");
        }
    }
}