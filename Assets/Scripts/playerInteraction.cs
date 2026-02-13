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
        float sphereRadius = 0.2f;

        if (Physics.SphereCast(playerCamera.transform.position, sphereRadius, playerCamera.transform.forward, out RaycastHit hit, interactRange))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                canInteract = true;

                enemy = hit.collider.GetComponentInParent<enemyBehaviour>();
            }
        }

        uiBehaviourScript.SetInteract(canInteract);

        if (enemy != null && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            Destroy(enemy.gameObject, 1.2f);
        }
    }
}
