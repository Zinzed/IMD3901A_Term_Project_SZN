using UnityEngine;
using UnityEngine.InputSystem;

public class playerInteraction : MonoBehaviour
{
    public float interactRange = 10.0f;
    public Camera playerCamera;
    public uiBehaviour uiBehaviourScript;

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
                    Color detectedColour = enemyRenderer.sharedMaterial.GetColor("_BaseColor");
                    uiBehaviourScript.SetCrosshairColor(detectedColour);

                    // Store enemy reference
                    enemy = hit.collider.GetComponentInParent<enemyBehaviour>();

                    Debug.Log("Hitting " + hit.collider.name + " Color: " + enemyRenderer.sharedMaterial.GetColor("_BaseColor"));
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
            Destroy(enemy.gameObject, 1.2f);
        }
    }
}
