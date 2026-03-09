using UnityEngine;
using UnityEngine.InputSystem;

public class playerInteraction : MonoBehaviour
{
    public float interactRange = 10.0f;
    public Camera playerCamera;
    public uiBehaviour uiBehaviourScript;
    public wandBehaviour wandBehaviourScript;

    public bool canInteract;

    private enemyBehaviour enemy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
            if (hit.collider.CompareTag("Enemy"))
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
                Destroy(enemy.gameObject, 1.2f);
                Debug.Log("Color Match! Enemy Destroyed.");
            }
            else
            {
                Debug.Log("Wrong Color! Wand: " + wandColor + " vs Enemy: " + currentTargetColor);
            }
        }
    }
}
