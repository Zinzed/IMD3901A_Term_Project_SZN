using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VRPlayerInteraction : MonoBehaviour
{
    public float interactRange = 10.0f;
    public Transform playerCamera; 
    public uiBehaviour uiBehaviourScript;
    public VRWandBehaviour wandBehaviourScript;

    public List<progressBar> playerProgressBars = new List<progressBar>();
    public GameObject puzzleCover;

    public bool canInteract;
    public int enemiesKilled;

    private enemyBehaviour enemy;

    private float lastHitTime;
    public float hitCooldown = 0.5f;

    [Header("VFX")]
    public GameObject hitEffect;

    private Dictionary<string, Color> materialColorMap = new Dictionary<string, Color>()
{
    // enemy material names mapped to colours 
    { "fireEnemy_mat", new Color(253/255f, 156/255f, 183/255f) }, // pink
    { "earthEnemy_mat", new Color(143/255f, 240/255f, 112/255f) }, // green
    { "waterEnemy_mat", new Color(128/255f, 215/255f, 244/255f) }, // blue
    { "finalBoss_mat", new Color(38/255f, 29/255f, 91/255f) } // dark purple
};

    public VRWandSwing wandSwing;
    private int bossHits = 0;
    public int requiredBossHits = 10;
    private bool isBossDead = false;
    private RaycastHit currentHit;
    private Color currentTargetColor;

    void Start()
    {
        canInteract = false;
    }

    void Update()
    {
        canInteract = false;
        enemy = null;
        //Color currentTargetColor = Color.clear;
        float sphereRadius = 0.9f;

       
        Vector3 rayOrigin = playerCamera != null ? playerCamera.position : transform.position;
        Vector3 rayDirection = playerCamera != null ? playerCamera.forward : transform.forward;

        if (Physics.SphereCast(rayOrigin, sphereRadius, rayDirection, out currentHit, interactRange))
        {
            if (currentHit.collider.CompareTag("Enemy") || currentHit.collider.CompareTag("FinalEnemy"))
            {
                canInteract = true;

                //get enemy color from its material
                Renderer enemyRenderer = currentHit.collider.GetComponentInChildren<Renderer>();
                if (enemyRenderer != null)
                {
                    //pass the detected color to UI
                    currentTargetColor = GetColorFromMaterialName(enemyRenderer);
                    uiBehaviourScript.SetCrosshairColor(currentTargetColor);

                    //Store reference
                    enemy = currentHit.collider.GetComponentInParent<enemyBehaviour>();
                }
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

        if (wandSwing != null && wandSwing.IsSwinging && canInteract)
        {
            Color wandColor = wandBehaviourScript.CurrentColor;

            float colorDiff = Vector4.Distance(currentTargetColor, wandColor);

            if (colorDiff < 0.1f)
            {
                if (currentHit.collider.CompareTag("FinalEnemy"))
                {
                    Debug.Log("VR Boss hit!");

                    if (!isBossDead)
                    {
                        HandleBossHit(currentHit.collider.gameObject);
                    }
                    return;
                }

                if (enemy != null)
                {
                    enemy.Kill();
                    enemiesKilled++;
                }
            }
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

    void HandleBossHit(GameObject bossObj)
    {
        if (isBossDead || Time.time < lastHitTime + hitCooldown) return;

        if (hitEffect != null)
        {
            //Spawn the effect at the boss's current position
            GameObject tempEffect = Instantiate(hitEffect, bossObj.transform.position, Quaternion.identity);
            tempEffect.SetActive(true);
            Destroy(tempEffect, 2f);
        }
        // Set the time of this hit
        lastHitTime = Time.time;

        bossHits++;
        Debug.Log("VR Boss hits: " + bossHits);

        if (bossHits >= requiredBossHits)
        {
            isBossDead = true;

            FinalBossActions bossActions = bossObj.GetComponentInParent<FinalBossActions>();

            if (bossActions != null)
            {
                bossActions.Die();
                Debug.Log("VR Boss defeated!");
            }
            else
            {
                Debug.LogError("FinalBossActions not found on boss!");
            }

            //2.reveal the Puzzle
            if (puzzleCover != null)
            {
                Destroy(puzzleCover);
                Debug.Log("VR Logic: Puzzle cover removed!");
            }
            

            if (playerProgressBars != null && playerProgressBars.Count > 0)
            {
                foreach (progressBar bar in playerProgressBars)
                {
                    if (bar != null)
                    {
                        Debug.Log($"VR Logic: Updating bar: {bar.name} to value {bar.slider.value + 10}");
                        bar.UpdateProgress(10);
                    }
                }
            }

            
        }
    }

}