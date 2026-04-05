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

    public float bobSpeed = 6.0f;
    public float bobAmount = 0.05f;

    private float defaultYPos;
    private float timer = 0;

    public AudioSource footstepSource;
    public float maxFootstepVolume = 0.5f;
    public float fadeSpeed = 5.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Debug.Log("Scene has started!");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        defaultYPos = cameraTransform.localPosition.y;

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

        if (moveInput != Vector2.zero)
        {
            timer += Time.deltaTime * bobSpeed;
            float bobOffset = Mathf.Sin(timer) * bobAmount;

            Vector3 newPos = cameraTransform.localPosition;
            newPos.y = defaultYPos + bobOffset;
            cameraTransform.localPosition = newPos;
        }
        else
        {
            timer = 0;

            Vector3 newPos = cameraTransform.localPosition;
            newPos.y = Mathf.Lerp(newPos.y, defaultYPos, Time.deltaTime * 5f);
            cameraTransform.localPosition = newPos;
        }

        bool isMoving = moveInput != Vector2.zero;

        if (isMoving)
        {
            if (!footstepSource.isPlaying)
            {
                footstepSource.Play();
            }

            footstepSource.volume = Mathf.Lerp(
                footstepSource.volume,
                maxFootstepVolume,
                Time.deltaTime * fadeSpeed
            );
        }
        else
        {
            footstepSource.volume = Mathf.Lerp(
                footstepSource.volume,
                0f,
                Time.deltaTime * fadeSpeed
            );

            if (footstepSource.volume < 0.01f && footstepSource.isPlaying)
            {
                footstepSource.Stop();
            }
        }

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
