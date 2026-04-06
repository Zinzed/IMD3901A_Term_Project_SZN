using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class playerInteraction : MonoBehaviour
{
    public float interactRange = 10.0f;
    public Camera playerCamera;
    public uiBehaviour uiBehaviourScript;
    public wandBehaviour wandBehaviourScript;

    public List<progressBar> playerProgressBars = new List<progressBar>();

    public GameObject puzzleCover;

    public bool canInteract;
    public int enemiesKilled;

    [Header("VFX")]
    public GameObject hitEffect;

    private enemyBehaviour enemy;
    private int bossHits = 0;
    public int requiredBossHits = 10;
    private bool isBossDead = false;

    private Dictionary<string, Color> materialColorMap = new Dictionary<string, Color>()
{
    // enemy material names mapped to colours 
    { "fireEnemy_mat", new Color(253/255f, 156/255f, 183/255f) }, // pink
    { "earthEnemy_mat", new Color(143/255f, 240/255f, 112/255f) }, // green
    { "waterEnemy_mat", new Color(128/255f, 215/255f, 244/255f) }, // blue
    { "finalBoss_mat", new Color(38/255f, 29/255f, 91/255f) } // dark purple
};

    //PUZZLE
    public ConstellationPuzzle puzzleScript;
    public TelescopeView teleView;

    public int particleDamage = 1;
    public float particleDamageCooldown = 0.5f;

    private float lastParticleDamageTime;
    private health playerHealth;

    [SerializeField] private Animator wandAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerHealth = GetComponent<health>();
        canInteract = false;

        //particle effect for when player gets hit:
        if (hitEffect != null)
            hitEffect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        //Debug.Log("Update running");
        canInteract = false;
        enemy = null;
        Color currentTargetColor = Color.clear;
        float sphereRadius = 0.2f;

        RaycastHit hit;
        bool didHit = Physics.SphereCast(playerCamera.transform.position, sphereRadius, playerCamera.transform.forward, out hit, interactRange);

        if (didHit)
        {
            //Debug.Log("Hit: " + hit.collider.name + " Tag: " + hit.collider.tag);
            if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("FinalEnemy"))
            {
                canInteract = true;

                // Get enemy color from its material
                Renderer enemyRenderer = hit.collider.GetComponentInChildren<Renderer>();
                if (enemyRenderer != null)
                {
                    // Pass the detected enemy color to UI
                    currentTargetColor = GetColorFromMaterialName(enemyRenderer);
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
                        Renderer starRenderer = hit.collider.GetComponent<Renderer>();

                        if (starRenderer == null)
                        {
                            starRenderer = hit.collider.GetComponentInChildren<Renderer>();
                        }
                        // Pass the star's world position and ID to your puzzle logic
                        puzzleScript.AddStar(hit.collider.transform.position, star.starID, starRenderer); 
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

        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            wandAnimator.SetTrigger("cast");
            if (enemy == null && !hit.collider.CompareTag("FinalEnemy")) return;
            // Get the wand light color from the wand script
            Color wandColor = wandBehaviourScript.CurrentColor;

            // Calculate "distance" between colors to handle slight rounding differences
            float colorDiff = Vector4.Distance(currentTargetColor, wandColor);
            
            if (colorDiff < 0.1f) // 0.1 allows for tiny variations
            {
                // Check if this enemy is actually the Boss
                if (hit.collider.CompareTag("FinalEnemy"))
                {
                    Debug.Log("Boss was hit!");

                    if (!isBossDead)
                    {
                        HandleBossHit(hit.collider.gameObject);
                    }
                    return;
                }

                if (enemy == null) return;

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

                // Normal enemy logic
                enemy.Kill();
                enemiesKilled++;
                AudioManager.Instance.PlaySFX("MagicCasting");
            }
            else
            {
                Debug.Log("Wrong Color! Wand: " + wandColor + " vs Enemy: " + currentTargetColor);
            }
        }

    }

    public void HandleBossHit(GameObject bossObj)
    {
        if (isBossDead) return;

        if (hitEffect != null)
        {
            //Spawn the effect at the boss's current position
            GameObject tempEffect = Instantiate(hitEffect, bossObj.transform.position, Quaternion.identity);
            tempEffect.SetActive(true);
            Destroy(tempEffect, 2f); 
        }

        bossHits++;
        AudioManager.Instance.PlaySFX("BossMagicCasting");

        //Debug.Log($"Boss Hit! {bossHits}/{requiredBossHits}");
        Debug.Log("Boss was hit!");
        Debug.Log("Boss hits: " + bossHits);

        if (bossHits >= requiredBossHits)
        {
            isBossDead = true;
            FinalBossActions bossActions = bossObj.GetComponentInParent<FinalBossActions>();
            if (bossActions != null)
            {
                bossActions.Die();
                Debug.Log("Boss reached required hits!");
            }
            else
            {
                Debug.LogError("FinalBossActions not found on boss!");
            }
            Debug.Log("Final Boss Defeated!");

            //reveal puzzle
            if (puzzleCover != null)
            {
                Destroy(puzzleCover);
                Debug.Log("Puzzle cover removed!");
            }

            if (playerProgressBars != null && playerProgressBars.Count > 0)
            {
                foreach (progressBar bar in playerProgressBars)
                {
                    if (bar != null)
                    {
                        bar.UpdateProgress(10);
                    }
                }
            }
            else
            {
                Debug.LogError("Player has no progress script!"); // Debug missing component
            }
            
            //Trigger victory screen or next scene
        }
    }
    public Color GetColorFromMaterialName(Renderer renderer)
    {
        if (renderer == null || renderer.sharedMaterial == null)
            return Color.white;

        string materialName = renderer.sharedMaterial.name;

        // remove the " (Instance)" suffix if present
        materialName = materialName.Replace(" (Instance)", "");

        // check for exact match
        if (materialColorMap.TryGetValue(materialName, out Color mappedColor))
        {
            return mappedColor;
        }

        Debug.LogWarning($"No color mapping found for material: {materialName}");
        return Color.white;
    }

    //particle effect
    IEnumerator DisableEffectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (hitEffect != null)
            hitEffect.SetActive(false);
    }
}
