using UnityEngine;
using UnityEngine.InputSystem;

public class playerInteraction : MonoBehaviour
{
    public float interactRange = 10.0f;
    public Camera playerCamera;
    public uiBehaviour uiBehaviourScript;
    public wandBehaviour wandBehaviourScript;
    public progressBar playerProgress;

    public bool canInteract;
    public int enemiesKilled;

    private enemyBehaviour enemy;
    private int bossHits = 0;
    public int requiredBossHits = 3;

    //PUZZLE
    public ConstellationPuzzle puzzleScript;
    public TelescopeView teleView;

    public int particleDamage = 1;
    public float particleDamageCooldown = 0.5f;

    private float lastParticleDamageTime;
    private health playerHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerHealth = GetComponent<health>();
        canInteract = false;
    }

    // Update is called once per frame
    void Update()
    {
        canInteract = false;
        enemy = null;
        Color currentTargetColor = Color.clear;
        float sphereRadius = 0.2f;

        if (Physics.SphereCast(playerCamera.transform.position, sphereRadius, playerCamera.transform.forward, out RaycastHit hit, interactRange))
        {
            if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("FinalEnemy"))
            {
                canInteract = true;

                // Get enemy color from its material
                Renderer enemyRenderer = hit.collider.GetComponentInChildren<Renderer>();
                if (enemyRenderer != null)
                {
                    // Pass the detected enemy color to UI
                    currentTargetColor = enemyRenderer.sharedMaterial.GetColor("_BaseColor");
                    uiBehaviourScript.SetCrosshairColor(currentTargetColor);

                    // Store enemy reference
                    enemy = hit.collider.GetComponentInParent<enemyBehaviour>();

                    //Debug.Log("Hitting " + hit.collider.name + " Color: " + enemyRenderer.sharedMaterial.GetColor("_BaseColor"));
                }

            }
            //FOR STAR OBJECTS
            else if(hit.collider.CompareTag("Star"))
            {
                canInteract = true;
                //PUZZLE STAR POSITION DETECTION
                if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
                {
                    StarNum star = hit.collider.GetComponent<StarNum>();
                    if (star != null)
                    {
                        // Pass the star's world position and ID to your puzzle logic
                        puzzleScript.AddStar(star.transform.position, star.starID);
                    }
                }
            }
            //FOR TELESCOPE
            else if(hit.collider.CompareTag("Telescope"))
            {
                canInteract = true;
                if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
                {
  
                   teleView.ToggleTelescope();
                   return;
                }

            }
            //FOR OTHER INTERACTABLE OBJECTS
            else if (hit.collider.CompareTag("Interactable"))
            {
                canInteract = true;
            }
            else
            {
                enemy = null;
                uiBehaviourScript.SetCrosshairToDefault();
            }
        }
        else
        {
            enemy = null;
            uiBehaviourScript.SetCrosshairToDefault();
        }


        uiBehaviourScript.SetInteract(canInteract);

        if (enemy != null && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            // Get the wand light color from your wand script
            Color wandColor = wandBehaviourScript.CurrentColor;

            // Calculate "distance" between colors to handle slight rounding differences
            float colorDiff = Vector4.Distance(currentTargetColor, wandColor);

            if (colorDiff < 0.1f) // 0.1 allows for tiny variations
            {
                // Check if this enemy is actually the Boss
                if (enemy.CompareTag("FinalEnemy"))
                {
                    HandleBossHit(enemy.gameObject);
                }
                else
                {
                    // Normal enemy logic
                    Destroy(enemy.gameObject, 1.2f);
                    enemiesKilled++;
                }
            }
            else
            {
                Debug.Log("Wrong Color! Wand: " + wandColor + " vs Enemy: " + currentTargetColor);
            }
        }

    }

    public void HandleBossHit(GameObject bossObj)
    {
        bossHits++;
        //Debug.Log($"Boss Hit! {bossHits}/{requiredBossHits}");

        if (bossHits >= requiredBossHits)
        {
            Destroy(bossObj, 0.5f);
            Debug.Log("Final Boss Defeated!");
            if (playerProgress != null)
            {
                playerProgress.UpdateProgress(+10); // Increment progress

            }
            else
            {
                Debug.LogError("Player has no progress script!"); // Debug missing component
            }
            
            //Trigger victory screen or next scene
        }
    }
}
