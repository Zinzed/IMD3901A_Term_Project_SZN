using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 2f;

    public CharacterController controller;
    public Transform cameraTransform;
    public GameObject pauseMenuUI;

    float xRotation = 0f;

    public Animator wandAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Debug.Log("Scene has started!");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Scene is updating!");

        if (pauseMenuUI.activeInHierarchy)
        {
            // Stop the script here so it doesn't lock the mouse
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        // Only lock when the menu is closed
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector2 moveInput = Keyboard.current != null ? new Vector2 
            (
                (Keyboard.current.aKey.isPressed ? -1 : 0) + (Keyboard.current.dKey.isPressed ? 1 : 0),
                (Keyboard.current.sKey.isPressed ? -1 : 0) + (Keyboard.current.wKey.isPressed ? 1 : 0)
            ) : Vector2.zero;   

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * speed * Time.deltaTime);

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            wandAnimator.SetTrigger("cast");
            
        }


    }

    private void ToggleCursor(bool show)
    {
        Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = show;
    }
}
