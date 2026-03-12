using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class VRPlayerInteraction : MonoBehaviour
{
    public float interactRange = 10.0f;
    public Transform playerCamera;
    public uiBehaviour uiBehaviourScript;
    public wandBehaviour wandBehaviourScript;

    //vr controls references
    public InputActionReference grabAction;

    public bool canInteract;
    public int enemiesKilled;
    public bool isGrabbing;

    private enemyBehaviour enemy;
    private bool grabPressedThisFrame;
    private bool wasGrabPressed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canInteract = false;
        isGrabbing = false;

        // Enable the input action
        if (grabAction != null)
            grabAction.action.Enable();
    }

    void OnEnable()
    {
        if (grabAction != null)
            grabAction.action.Enable();
    }

    void OnDisable()
    {
        if (grabAction != null)
            grabAction.action.Disable();
    }


    // Update is called once per frame
    void Update()
    {
        // Handle toggle grab input
        HandleGrabToggle();

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

        // Check if we're grabbing AND have an enemy targeted
        if (enemy != null && isGrabbing)
        {
            // Get the wand light color from your wand script
            Color wandColor = wandBehaviourScript.CurrentColor;

            // Calculate "distance" between colors to handle slight rounding differences
            float colorDiff = Vector4.Distance(currentTargetColor, wandColor);

            if (colorDiff < 0.1f) // 0.1 allows for tiny variations
            {
                Destroy(enemy.gameObject, 1.2f);
                enemiesKilled++;
                Debug.Log("Color Match! Enemy Destroyed.");

                
            }
            else
            {
                Debug.Log("Wrong Color! Wand: " + wandColor + " vs Enemy: " + currentTargetColor);
            }
        }
    }

    void HandleGrabToggle()
    {
        if (grabAction == null) return;

        // Check if grab button was pressed this frame
        grabPressedThisFrame = grabAction.action.WasPressedThisFrame();

        // Toggle grab state on button press (not hold)
        if (grabPressedThisFrame && !wasGrabPressed)
        {
            isGrabbing = !isGrabbing;
            Debug.Log("Grab toggled: " + isGrabbing);

           
        }

        wasGrabPressed = grabPressedThisFrame;
    }

    

    // Optional: Public method to manually set grab state if needed
    public void SetGrabState(bool grabState)
    {
        isGrabbing = grabState;
    }

    // Optional: Toggle grab state manually
    public void ToggleGrab()
    {
        isGrabbing = !isGrabbing;
    }
}
