using UnityEngine;
using UnityEngine.InputSystem;

public class VRPlayerInteraction : MonoBehaviour
{
    public float interactRange = 10.0f;
    public Transform playerCamera; // VR Camera (usually XR Origin/Camera Offset/Main Camera)
    public uiBehaviour uiBehaviourScript;
    public wandBehaviour wandBehaviourScript;

    public bool canInteract;
    public int enemiesKilled;

    private enemyBehaviour enemy;

    void Start()
    {
        canInteract = false;
    }

    void Update()
    {
        canInteract = false;
        enemy = null;
        Color currentTargetColor = Color.clear;
        float sphereRadius = 0.2f;

        // Raycast from camera position forward
        Vector3 rayOrigin = playerCamera != null ? playerCamera.position : transform.position;
        Vector3 rayDirection = playerCamera != null ? playerCamera.forward : transform.forward;

        if (Physics.SphereCast(rayOrigin, sphereRadius, rayDirection, out RaycastHit hit, interactRange))
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
    }

    
}