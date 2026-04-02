using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VRPlayerInteraction : MonoBehaviour
{
    public float interactRange = 10.0f;
    public Transform playerCamera; 
    public uiBehaviour uiBehaviourScript;
    public VRWandBehaviour wandBehaviourScript;

    public bool canInteract;
    public int enemiesKilled;

    private enemyBehaviour enemy;

    private Dictionary<string, Color> materialColorMap = new Dictionary<string, Color>()
{
    // enemy material names mapped to colours 
    { "fireEnemy_mat", new Color(253/255f, 156/255f, 183/255f) }, // pink
    { "earthEnemy_mat", new Color(143/255f, 240/255f, 112/255f) }, // green
    { "waterEnemy_mat", new Color(128/255f, 215/255f, 244/255f) }, // blue
    { "darkPurple_TEMP", new Color(38/255f, 29/255f, 91/255f) } // dark purple
};

    void Start()
    {
        canInteract = false;
    }

    void Update()
    {
        canInteract = false;
        enemy = null;
        Color currentTargetColor = Color.clear;
        float sphereRadius = 0.9f;

       
        Vector3 rayOrigin = playerCamera != null ? playerCamera.position : transform.position;
        Vector3 rayDirection = playerCamera != null ? playerCamera.forward : transform.forward;

        if (Physics.SphereCast(rayOrigin, sphereRadius, rayDirection, out RaycastHit hit, interactRange))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                canInteract = true;

                //get enemy color from its material
                Renderer enemyRenderer = hit.collider.GetComponentInChildren<Renderer>();
                if (enemyRenderer != null)
                {
                    //pass the detected color to UI
                    currentTargetColor = GetColorFromMaterialName(enemyRenderer);
                    uiBehaviourScript.SetCrosshairColor(currentTargetColor);

                    //Store reference
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

}